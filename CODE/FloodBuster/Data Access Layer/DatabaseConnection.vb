Imports System.Data.SqlClient

Public Class DatabaseConnection

    Private Const CONNECTION_STRING As String = "Server=MAVI\SQLEXPRESS;Database=FloodBusterDB;Integrated Security=True;TrustServerCertificate=True;"

    Public Shared Function GetConnection() As SqlConnection
        Return New SqlConnection(CONNECTION_STRING)
    End Function

End Class


