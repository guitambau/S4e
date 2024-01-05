Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic

<Serializable>
Public Class DatabaseAccess
    Private connectionString As String = "Server=NOTE-026464\MSSQLSERVER01;Database=master;Trusted_Connection=True;"
    Public Function GetConnection() As SqlConnection
        Dim connection As New SqlConnection(connectionString)
        connection.Open()
        Return connection
    End Function

    Public Function ExecuteCommand(command As SqlCommand) As DataTable
        Dim result As New DataTable()
        Using connection As SqlConnection = GetConnection()
            command.Connection = connection
            Using reader As SqlDataReader = command.ExecuteReader()
                result.Load(reader)
            End Using
        End Using
        Return result
    End Function

    Public Sub ExecuteNonQuery(command As SqlCommand)
        Using connection As SqlConnection = GetConnection()
            command.Connection = connection
            command.ExecuteNonQuery()
        End Using
    End Sub

    Public Function ExecuteScalar(command As SqlCommand) As Object
        Using connection As SqlConnection = GetConnection()
            command.Connection = connection
            Return command.ExecuteScalar()
        End Using
    End Function

End Class
