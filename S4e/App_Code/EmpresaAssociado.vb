Imports System.Data.SqlClient
Imports Microsoft.VisualBasic

Public Class EmpresaAssociado
    Private dbAccess As New DatabaseAccess()

    Public Sub adicionaRelacionamento(associadoId As Long, empresaId As Long)
        If verificaRelacionamento(associadoId, empresaId) Then
            Dim command As New SqlCommand("INSERT INTO empresa_associado (associado_id, empresa_id) VALUES (@AssociadoId, @EmpresaId)")
            command.Parameters.AddWithValue("@AssociadoId", associadoId)
            command.Parameters.AddWithValue("@EmpresaId", empresaId)
            dbAccess.ExecuteNonQuery(command)
        End If
    End Sub

    Public Sub ExcluirRelacionamentoAssociadoEmpresa(associadoId As Integer, empresaId As Integer)
        Dim query As String = "DELETE FROM empresa_associado WHERE associado_id = @AssociadoId and empresa_id = @EmpresaId"
        Using command As New SqlCommand(query)
            command.Parameters.AddWithValue("@AssociadoId", associadoId)
            command.Parameters.AddWithValue("@EmpresaId", empresaId)
            dbAccess.ExecuteNonQuery(command)
        End Using
    End Sub

    Private Function verificaRelacionamento(associadoId As Integer, empresaId As Integer)
        Dim query As String = "SELECT COUNT(1) FROM empresa_associado WHERE associado_id = @AssociadoId AND empresa_id = @EmpresaId"
        Using command As New SqlCommand(query)
            command.Parameters.AddWithValue("@AssociadoId", associadoId)
            command.Parameters.AddWithValue("@EmpresaId", empresaId)
            Dim result As Integer = Convert.ToInt32(dbAccess.ExecuteScalar(command))
            Return result = 0
        End Using
    End Function
End Class
