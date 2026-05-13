Public Class UserService

    Private _userRepo As New UserRepository()

    ' Validate login — returns the role string ("Admin", "StandardUser") or "" if failed
    Public Function Login(username As String, password As String) As String
        If String.IsNullOrWhiteSpace(username) OrElse String.IsNullOrWhiteSpace(password) Then
            MessageBox.Show("Please enter both a username and password.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return ""
        End If
        Return _userRepo.ValidateLogin(username, password)
    End Function

End Class


