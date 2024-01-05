Imports Microsoft.VisualBasic
<Serializable>
Public Class FuncoesGlobais
    Public Shared Function RemoveCaracteresEspeciais(ByVal doc As String) As String
        If doc.Contains(".") Then
            doc = doc.Replace(".", "")
        End If
        If doc.Contains("-") Then
            doc = doc.Replace("-", "")
        End If
        If doc.Contains("/") Then
            doc = doc.Replace("/", "")
        End If
        Return doc
    End Function

    Public Shared Function isCPF(ByVal CPF As String) As Boolean
        CPF = removeCaracteresEspeciais(CPF)
        If CPF.Equals("00000000000") OrElse CPF.Equals("11111111111") OrElse CPF.Equals("22222222222") OrElse
            CPF.Equals("33333333333") OrElse CPF.Equals("44444444444") OrElse CPF.Equals("55555555555") OrElse
            CPF.Equals("66666666666") OrElse CPF.Equals("77777777777") OrElse CPF.Equals("88888888888") OrElse
            CPF.Equals("99999999999") OrElse (CPF.Length <> 11) Then
            Return False
        End If

        Dim dig10, dig11 As Char
        Dim sm, i, r, num, peso As Integer

        Try
            sm = 0
            peso = 10
            For i = 0 To 8
                num = Asc(CPF.Chars(i)) - 48
                sm += num * peso
                peso -= 1
            Next

            r = 11 - (sm Mod 11)
            If r = 10 OrElse r = 11 Then
                dig10 = "0"c
            Else
                dig10 = Chr(r + 48)
            End If

            sm = 0
            peso = 11
            For i = 0 To 9
                num = Asc(CPF.Chars(i)) - 48
                sm += num * peso
                peso -= 1
            Next

            r = 11 - (sm Mod 11)
            If r = 10 OrElse r = 11 Then
                dig11 = "0"c
            Else
                dig11 = Chr(r + 48)
            End If

            Return dig10 = CPF.Chars(9) AndAlso dig11 = CPF.Chars(10)
        Catch erro As FormatException
            Return False
        End Try
    End Function

    Public Shared Function isCNPJ(ByVal CNPJ As String) As Boolean
        CNPJ = RemoveCaracteresEspeciais(CNPJ)
        If CNPJ.Equals("00000000000000") OrElse CNPJ.Equals("11111111111111") OrElse CNPJ.Equals("22222222222222") OrElse
            CNPJ.Equals("33333333333333") OrElse CNPJ.Equals("44444444444444") OrElse CNPJ.Equals("55555555555555") OrElse
            CNPJ.Equals("66666666666666") OrElse CNPJ.Equals("77777777777777") OrElse CNPJ.Equals("88888888888888") OrElse
            CNPJ.Equals("99999999999999") OrElse (CNPJ.Length <> 14) Then
            Return False
        End If

        Dim dig13, dig14 As Char
        Dim sm, i, r, num, peso As Integer
        Try
            sm = 0
            peso = 2
            For i = 11 To 0 Step -1
                num = Asc(CNPJ.Chars(i)) - 48
                sm += num * peso
                peso += 1
                If peso = 10 Then
                    peso = 2
                End If
            Next

            r = sm Mod 11
            If r = 0 OrElse r = 1 Then
                dig13 = "0"c
            Else
                dig13 = Chr((11 - r) + 48)
            End If

            sm = 0
            peso = 2
            For i = 12 To 0 Step -1
                num = Asc(CNPJ.Chars(i)) - 48
                sm += num * peso
                peso += 1
                If peso = 10 Then
                    peso = 2
                End If
            Next

            r = sm Mod 11
            If r = 0 OrElse r = 1 Then
                dig14 = "0"c
            Else
                dig14 = Chr((11 - r) + 48)
            End If

            Return dig13 = CNPJ.Chars(12) AndAlso dig14 = CNPJ.Chars(13)
        Catch erro As FormatException
            Return False
        End Try
    End Function

End Class
