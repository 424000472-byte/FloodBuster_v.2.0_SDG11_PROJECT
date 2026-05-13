Public Class LoginForm

    ' 1. CENTERING LOGIC
    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CenterPanel()
    End Sub

    Private Sub LoginForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        CenterPanel()
    End Sub

    Private Sub CenterPanel()
        Panel1.Left = (Me.ClientSize.Width - Panel1.Width) / 2
        Panel1.Top = (Me.ClientSize.Height - Panel1.Height) / 2
    End Sub

    ' 2. LOGIN BUTTON LOGIC
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' Validation is now handled inside UserService
        Dim userSvc As New UserService()
        Dim role As String = userSvc.Login(txtUsername.Text, txtPassword.Text)

        If role <> "" Then
            Dim isAdmin As Boolean = (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))

            ' Pass the role to DashboardForm — it handles its own button visibility
            DashboardForm.isAdmin = isAdmin

            MessageBox.Show("Login Successful! Welcome, " & txtUsername.Text, "FloodBuster",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)

            DashboardForm.Show()
            Me.Hide()
        Else
            ' Failed login
            MessageBox.Show("Invalid username or password recorded in our database.", "Login Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtPassword.Clear()
            txtPassword.Focus()
        End If
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles logo.Click

    End Sub
End Class
