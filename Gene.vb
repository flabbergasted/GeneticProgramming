Imports System.Configuration

Public Class Gene
    'DNA is an odd length string that starts and ends with a number, the letters represent mathematical operations, see enumGenome
    Private myDNA As String
    Private minNumber As Integer = 1
    Private maxNumber As Integer = 9
    Private minLength As Integer = 3
    Private maxLength As Integer = 15
    Public parentA As String = ""
    Public parentB As String = ""
    Public stDNAScore As Integer

    Public Function DNAScore(ByVal gene As Gene, ByVal desiredResult As Integer) As Double
        Dim dna As String
        Dim result, nxt As Double
        dna = gene.DNA.ToString

        nxt = 0

        For i As Integer = 0 To dna.Length - 1
            If i = 0 Then
                result = Integer.Parse(dna(i))
                Continue For
            End If

            nxt = Integer.Parse(dna(i + 1))
            If dna(i) = "A" Then
                result = result + nxt
            ElseIf dna(i) = "B" Then
                result = result - nxt
            ElseIf dna(i) = "C" Then
                result = result * nxt
            ElseIf dna(i) = "D" Then
                result = result / nxt
            End If
            i = i + 1
        Next

        result = result - desiredResult

        If result < 0 Then
            result = result * -1
        End If

        Return result

    End Function

    Property DNA()
        Get
            Return myDNA
        End Get
        Set(ByVal value)
            myDNA = value
        End Set
    End Property

    Public Sub New()
        myDNA = GenerateDNA()
        stDNAScore = DNAScore(Me, CType(ConfigurationSettings.AppSettings("DesiredValue"), Integer))
    End Sub

    Public Sub New(ByVal DNA As String)
        myDNA = DNA

        stDNAScore = DNAScore(Me, CType(ConfigurationSettings.AppSettings("DesiredValue"), Integer))
    End Sub

    Public Overrides Function ToString() As String
        Return myDNA
    End Function

    Public Function Mate(ByVal mateDNA As String) As Gene
        Dim resultDNA, newValue As String
        Dim randomNumber, mySplitPoint, mateSplitPoint As Integer
        Dim a, b As Char

        mySplitPoint = myDNA.Length \ 2
        a = myDNA(mySplitPoint)
        mateSplitPoint = mateDNA.Length \ 2
        b = mateDNA(mateSplitPoint)

        'Ensure the split point for both parents is at the same location, 
        'to preserve the equation (otherwise you could get something like 7++8-3 for example, can't have 2 operators next to each other)
        If Not isEven(mySplitPoint) Then
            mySplitPoint = mySplitPoint + 1
        End If

        If Not isEven(mateSplitPoint) Then
            mateSplitPoint = mateSplitPoint + 1
        End If

        'Make the child DNA out of the 2 halves of the parents
        newValue = ""
        resultDNA = myDNA.Substring(0, mySplitPoint) + mateDNA.Substring(mateSplitPoint)

        'loop through and randomly change individual chars to simulate transcription errors (A.K.A Mutation, affected by the Mutation Chance in the config)
        For i As Integer = 0 To resultDNA.Length - 1
            randomNumber = enumGenome.ran.Next(0, 100)
            'Make sure and swap out the old char with another valid character, to preserve the equation
            If isEven(i) Then
                newValue = enumGenome.ran.Next(minNumber, maxNumber).ToString
            Else
                newValue = enumGenome.RandomGenome()
            End If

            If randomNumber < CType(ConfigurationSettings.AppSettings("MutationChance"), Integer) Then
                resultDNA = resultDNA.Remove(i, 1)
                resultDNA = resultDNA.Insert(i, newValue)
            End If
        Next
        Dim result As New Gene(resultDNA)
        result.parentA = mateDNA
        result.parentB = myDNA
        Return result

    End Function

    'Takes the mod 2 of the number passed in, if it's a 1 the number is odd, if it's a 0 the number is even.
    Public Function isEven(ByVal i As Integer) As Boolean
        Return Not CType(i Mod 2, Boolean)
    End Function

    Private Function GenerateDNA() As String
        Dim length As Integer
        Dim nextValue As String
        Dim resultDNA As String = ""

        length = enumGenome.ran.Next(minLength, maxLength)

        If isEven(length) Then
            length = length + 1
        End If

        For i As Integer = 0 To length - 1
            If isEven(i) Then
                nextValue = ran.Next(minNumber, maxNumber).ToString
            Else
                nextValue = enumGenome.RandomGenome()
            End If
            resultDNA += nextValue
        Next

        Return resultDNA

    End Function

End Class
