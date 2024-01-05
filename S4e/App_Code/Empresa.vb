Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic

<Serializable>
Public Class Empresa
    Public Property NomeEmpresa As String
    Public Property EmpresaID As Integer

    Private dbAccess As New DatabaseAccess()
    Private funcoesGlobais As New FuncoesGlobais()

    Public Function AdicionarEmpresa(nome As String, cnpj As String)
        If isCnpjUnique(cnpj) Then
            Dim command As New SqlCommand("INSERT INTO empresa (nome, cnpj) VALUES (@Nome, @Cnpj); SELECT SCOPE_IDENTITY();")
            command.Parameters.AddWithValue("@Nome", nome)
            command.Parameters.AddWithValue("@Cnpj", cnpj)

            Dim insertedId As Integer = Convert.ToInt32(dbAccess.ExecuteScalar(command))
            Return insertedId
        Else
            Throw New Exception("CNPJ já cadastrado.")
        End If
    End Function

    Public Sub AtualizarEmpresa(id As Integer, nome As String, cnpj As String)
        If isCnpjUnique(cnpj, id) Then
            Dim command As New SqlCommand("UPDATE empresa SET nome = @Nome, cnpj = @Cpf WHERE id = @Id")
            command.Parameters.AddWithValue("@Id", id)
            command.Parameters.AddWithValue("@Nome", nome)
            command.Parameters.AddWithValue("@Cpf", cnpj)
            dbAccess.ExecuteNonQuery(command)
        Else
            Throw New Exception("CNPJ já cadastrado para outra empresa.")
        End If
    End Sub

    Public Sub RemoverEmpresa(id As Integer)
        Dim command As New SqlCommand("DELETE FROM empresa WHERE id = @Id")
        command.Parameters.AddWithValue("@Id", id)
        dbAccess.ExecuteNonQuery(command)
    End Sub

    Public Function BuscarEmpresa(Optional cnpj As String = Nothing, Optional nome As String = Nothing, Optional id As Integer? = Nothing) As DataTable
        Dim command As New SqlCommand()
        Dim query As String = "SELECT * FROM empresa WHERE 1=1"

        If Not String.IsNullOrEmpty(cnpj) Then
            query &= " AND cnpj like @Cnpj"
            command.Parameters.AddWithValue("@Cnpj", cnpj)
        End If

        If Not String.IsNullOrEmpty(nome) Then
            query &= " AND nome like @Nome"
            command.Parameters.AddWithValue("@Nome", nome)
        End If

        If id.HasValue Then
            query &= " AND id = @Id"
            command.Parameters.AddWithValue("@Id", id.Value)
        End If

        command.CommandText = query
        Return dbAccess.ExecuteCommand(command)
    End Function

    Public Function BuscaAssociadoRelacionadas(cnpj As String) As DataTable
        Dim query As String = "SELECT a.id as AssociadoId, a.nome as NomeAssociado FROM empresa_associado ae JOIN empresa e ON ae.empresa_id = e.id JOIN associado a on a.id = ae.associado_id WHERE e.cnpj = @Cnpj"
        Using command As New SqlCommand(query)
            command.Parameters.AddWithValue("@Cnpj", cnpj)
            Return dbAccess.ExecuteCommand(command)
        End Using
    End Function

    Public Function BuscarEmpresaIdPorCnpj(cnpj As String) As Integer
        Dim query As String = "SELECT Id FROM empresa WHERE cnpj = @Cnpj"
        Using command As New SqlCommand(query)
            command.Parameters.AddWithValue("@Cnpj", cnpj)
            Dim id As Object = dbAccess.ExecuteScalar(command)
            Return If(id IsNot Nothing, Convert.ToInt32(id), 0)
        End Using
    End Function

    Public Sub ExcluirRelacionamentoAssociadoEmpresa(associadoId As Integer, empresaId As Integer)
        Dim query As String = "DELETE FROM empresa_associado WHERE associado_id = @AssociadoId and empresa_id = @EmpresaId"
        Using command As New SqlCommand(query)
            command.Parameters.AddWithValue("@AssociadoId", associadoId)
            command.Parameters.AddWithValue("@EmpresaId", empresaId)
            dbAccess.ExecuteNonQuery(command)
        End Using
    End Sub
    Private Function isCnpjUnique(cnpj As String, Optional id As Integer = 0) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM empresa WHERE cnpj = @Cnpj"
        If id > 0 Then
            query += " AND id <> @Id"
        End If

        Using command As New SqlCommand(query)
            command.Parameters.AddWithValue("@Cnpj", cnpj)
            If id > 0 Then
                command.Parameters.AddWithValue("@Id", id)
            End If

            Dim result As Integer = Convert.ToInt32(dbAccess.ExecuteScalar(command))
            Return result = 0
        End Using
    End Function
End Class

