Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic

<Serializable>
Public Class Associado
    Public Property NomeAssociado As String
    Public Property AssociadoID As Integer
    Private dbAccess As New DatabaseAccess()

    Public Function AdicionarAssociado(nome As String, cpf As String, dataNascimento As DateTime) As Integer
        If IsCpfUnique(cpf) Then
            Dim command As New SqlCommand("INSERT INTO associado (nome, cpf, data_nascimento) VALUES (@Nome, @Cpf, @DataNascimento); SELECT SCOPE_IDENTITY();")
            command.Parameters.AddWithValue("@Nome", nome)
            command.Parameters.AddWithValue("@Cpf", cpf)
            command.Parameters.AddWithValue("@DataNascimento", dataNascimento)

            Dim insertedId As Integer = Convert.ToInt32(dbAccess.ExecuteScalar(command))
            Return insertedId
        Else
            Throw New Exception("CPF já cadastrado.")
        End If
    End Function

    Public Sub AtualizarAssociado(id As Integer, nome As String, cpf As String, dataNascimento As DateTime)
        If IsCpfUnique(cpf, id) Then
            Dim command As New SqlCommand("UPDATE associado SET nome = @Nome, cpf = @Cpf, data_nascimento = @DataNascimento WHERE id = @Id")
            command.Parameters.AddWithValue("@Id", id)
            command.Parameters.AddWithValue("@Nome", nome)
            command.Parameters.AddWithValue("@Cpf", cpf)
            command.Parameters.AddWithValue("@DataNascimento", dataNascimento)
            dbAccess.ExecuteNonQuery(command)
        Else
            Throw New Exception("CPF já cadastrado para outro associado.")
        End If
    End Sub

    Public Sub RemoverAssociado(id As Integer)
        Dim command As New SqlCommand("DELETE FROM associado WHERE id = @Id")
        command.Parameters.AddWithValue("@Id", id)
        dbAccess.ExecuteNonQuery(command)
    End Sub

    Public Function BuscarAssociado(Optional cpf As String = Nothing, Optional nome As String = Nothing, Optional dataNascimento As String = Nothing, Optional id As Integer? = Nothing) As DataTable
        Dim command As New SqlCommand()
        Dim query As String = "SELECT * FROM associado WHERE 1=1"

        If Not String.IsNullOrEmpty(cpf) Then
            query &= " AND cpf like @Cpf"
            command.Parameters.AddWithValue("@Cpf", cpf)
        End If

        If Not String.IsNullOrEmpty(nome) Then
            query &= " AND nome like @Nome"
            command.Parameters.AddWithValue("@Nome", nome)
        End If

        If Not String.IsNullOrEmpty(dataNascimento) Then
            query &= " AND data_nascimento = @DataNascimento"
            command.Parameters.AddWithValue("@DataNascimento", dataNascimento)
        End If

        If id.HasValue Then
            query &= " AND id = @Id"
            command.Parameters.AddWithValue("@Id", id.Value)
        End If

        command.CommandText = query
        Return dbAccess.ExecuteCommand(command)
    End Function

    Public Function BuscaEmpresasRelacionadas(cpf As String) As DataTable
        Dim query As String = "SELECT e.id as EmpresaID, e.nome as NomeEmpresa FROM empresa_associado ae JOIN empresa e ON ae.empresa_id = e.id JOIN associado a on a.id = ae.associado_id WHERE a.cpf = @Cpf"
        Using command As New SqlCommand(query)
            command.Parameters.AddWithValue("@Cpf", cpf)
            Return dbAccess.ExecuteCommand(command)
        End Using
    End Function

    Public Function BuscarAssociadoIdPorCpf(cpf As String) As Integer
        Dim query As String = "SELECT Id FROM associado WHERE cpf = @Cpf"
        Using command As New SqlCommand(query)
            command.Parameters.AddWithValue("@Cpf", cpf)
            Dim id As Object = dbAccess.ExecuteScalar(command)
            Return If(id IsNot Nothing, Convert.ToInt32(id), 0)
        End Using
    End Function

    Private Function IsCpfUnique(cpf As String, Optional id As Integer = 0) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM associado WHERE cpf = @Cpf"
        If id > 0 Then
            query += " AND id <> @Id"
        End If

        Using command As New SqlCommand(query)
            command.Parameters.AddWithValue("@Cpf", cpf)
            If id > 0 Then
                command.Parameters.AddWithValue("@Id", id)
            End If

            Dim result As Integer = Convert.ToInt32(dbAccess.ExecuteScalar(command))
            Return result = 0
        End Using
    End Function
End Class
