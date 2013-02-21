Option Strict On
Imports System.Configuration
Module Module1

    Sub Main()
        Dim father As New Gene("3A6A1B8C8C2")
        Dim mother As New Gene("1D4C2C1A5D3A5")
        father.Mate(mother.DNA.ToString)
        System.Console.WriteLine(SolveFor(CType(ConfigurationSettings.AppSettings("DesiredValue"), Integer)))
        System.Console.Read()
    End Sub

    'generates a mathematical equation with the answer being desired result, generates equations for results from 0 - 135
    Function SolveFor(ByVal desiredResult As Integer) As String
        Dim arGeneCollection As New ArrayList
        Dim arMatingGenes As New ArrayList
        Dim score, avgScore, avgScore1, avgScore2 As Double
        Dim random, limit, harshness, ogharshness, generation As Integer
        Dim gene, mateResult, father, mother As Gene

        ogharshness = CType(ConfigurationSettings.AppSettings("Harshness"), Integer)
        harshness = ogharshness

        For i = 0 To 100
            arGeneCollection.Add(New Gene)
        Next

        While True

            For j = 0 To arGeneCollection.Count - 1
                harshness = ogharshness
                gene = CType(arGeneCollection(j), Gene)
                score = DNAScore(gene, desiredResult)

                System.Console.Write(gene.DNA.ToString + "    ")
                System.Console.WriteLine(score)

                If score > harshness Then
                    score = harshness
                End If
                If score = 0 Then
                    harshness = 100
                End If
                random = enumGenome.ran.Next(0, 100)
                limit = harshness - CType(score, Integer)

                If random < limit Then
                    arMatingGenes.Add(arGeneCollection(j))
                End If

            Next

            avgScore2 = AverageDNAScore(arGeneCollection)

            For k = 0 To arGeneCollection.Count - 1
                random = enumGenome.ran.Next(0, arMatingGenes.Count - 1)
                father = CType(arMatingGenes(random), Gene)
                random = enumGenome.ran.Next(0, arMatingGenes.Count - 1)
                mother = CType(arMatingGenes(random), Gene)
                mateResult = father.Mate(mother.DNA.ToString)
                arGeneCollection(k) = mateResult
            Next
            avgScore1 = AverageDNAScore(arGeneCollection)
            avgScore = AverageDNAScore(arMatingGenes)
            generation += 1
            If avgScore < 1 Then
                System.Console.WriteLine()
                System.Console.WriteLine(generation.ToString + " Generations until first average DNA score < 1 (means within 1 of the desired number)")
                Exit While
            End If
            arMatingGenes.Clear()
        End While

        System.Console.Read()
        Return "0"
    End Function

    Public Function AverageDNAScore(ByVal arGenes As ArrayList) As Double
        Dim result As Double

        For i As Integer = 0 To arGenes.Count - 1
            result = result + DNAScore(CType(arGenes(i), Gene), CType(ConfigurationSettings.AppSettings("DesiredValue"), Integer))
        Next

        Return result / arGenes.Count
    End Function

    'scores the DNA's 'fitness' level  by comparing it's answer to the desired result, golf scoring
    Function DNAScore(ByVal gene As Gene, ByVal desiredResult As Integer) As Double
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

End Module
