Public Module enumGenome
    Public ReadOnly A As String = "+"
    Public ReadOnly B As String = "-"
    Public ReadOnly C As String = "*"
    Public ReadOnly D As String = "/"

    Public ran As New Random

    Public Function RandomGenome() As String
        Dim random As Integer

        random = ran.Next(0, 100)

        If random <= 25 Then
            Return "A"
        ElseIf random > 25 And random <= 50 Then
            Return "B"
        ElseIf random > 50 And random <= 75 Then
            Return "C"
        Else
            Return "D"
        End If

    End Function
End Module
