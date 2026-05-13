Imports System.Data
Imports System.Data.SqlClient

Public Class UserRepository

    ''' <summary>
    ''' Validates user credentials and returns the Role if successful.
    ''' Matches 'Username' and 'PasswordHash' columns from Data Dictionary.
    ''' </summary>
    Public Function ValidateLogin(username As String, password As String) As String
        Dim role As String = ""
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                ' Consolidated query: Returns Role for the matching User
                Dim sql As String = "SELECT Role FROM Users WHERE Username = @User AND PasswordHash = @Pass"
                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@User", username)
                cmd.Parameters.AddWithValue("@Pass", password)

                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    role = result.ToString()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Database Error: " & ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return role ' Returns 'Admin', 'StandardUser', or empty string if not found
    End Function

    ' READ - Get all users (Useful for an Admin Management Grid)
    Public Function GetAll() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                ' DateCreated has a DEFAULT GETDATE() constraint in your schema
                Dim cmd As New SqlCommand(
                    "SELECT UserID, Username, Role, DateCreated FROM Users ORDER BY Username", conn)
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(dt)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading users: " & ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function

    ' CREATE - Register new user (Matches Admin/StandardUser Check Constraint)
    Public Function Add(username As String, passwordHash As String, role As String) As Boolean
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                ' Fixed multi-line string syntax for VB.NET
                Dim sql As String = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@User, @Pass, @Role)"
                Dim cmd As New SqlCommand(sql, conn)

                cmd.Parameters.AddWithValue("@User", username)
                cmd.Parameters.AddWithValue("@Pass", passwordHash)
                cmd.Parameters.AddWithValue("@Role", role) ' Must be 'Admin' or 'StandardUser'

                cmd.ExecuteNonQuery()
                Return True
            End Using
        Catch ex As Exception
            MessageBox.Show("Error adding user: " & ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' DELETE - Remove user
    Public Function Delete(userId As Integer) As Boolean
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                Dim cmd As New SqlCommand("DELETE FROM Users WHERE UserID = @ID", conn)
                cmd.Parameters.AddWithValue("@ID", userId)
                cmd.ExecuteNonQuery()
                Return True
            End Using
        Catch ex As Exception
            MessageBox.Show("Error deleting user: " & ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

End Class
