Public Class DashboardForm
    ' Tracks if the user logged in as Admin or regular User
    Public isAdmin As Boolean

    ' --- FORM EVENTS ---

    Private Sub DashboardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If isAdmin Then
            lblWelcome.Text = "Welcome to the Admin DashBoard!"
            lblsubtitle.Text = "Admin Controls"
        Else
            lblWelcome.Text = "Welcome to the DashBoard!"
            lblsubtitle.Text = "User Options"
        End If

        ConfigureButtonVisibility()
        CenterEverything()
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub

    Private Sub DashboardForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        CenterEverything()
    End Sub

    ' --- LOGIC & LAYOUT ---

    Private Sub ConfigureButtonVisibility()
        ' Admin-only buttons
        btnMarkFlooded.Visible = isAdmin
        btnManageAlerts.Visible = isAdmin
        btnGenerateReport.Visible = isAdmin
        btnResetFlood.Visible = isAdmin

        ' User-only buttons
        btnViewFloodStatus.Visible = Not isAdmin
        btnEvacuationPath.Visible = Not isAdmin
        btnViewAlerts.Visible = Not isAdmin
    End Sub

    Public Sub CenterEverything()
        flpButtons.AutoSize = True
        flpButtons.MaximumSize = New Size(700, 0)

        pnl1.Left = (Me.ClientSize.Width - pnl1.Width) / 2
        pnl1.Top = (Me.ClientSize.Height - pnl1.Height) / 2

        lblWelcome.Left = (pnl1.Width - lblWelcome.Width) / 2
        lblWelcome.Top = 30

        lblsubtitle.Left = (pnl1.Width - lblsubtitle.Width) / 2
        lblsubtitle.Top = lblWelcome.Bottom + 5

        flpButtons.Left = (pnl1.Width - flpButtons.Width) / 2
        flpButtons.Top = lblsubtitle.Bottom + 35

        btnLogout.Left = pnl1.Width - btnLogout.Width - 20
        btnLogout.Top = pnl1.Height - btnLogout.Height - 20
    End Sub

    ' --- BUTTON NAVIGATION CLICK EVENTS ---

    Private Sub btnViewFloodStatus_Click(sender As Object, e As EventArgs) Handles btnViewFloodStatus.Click
        Dim frm As New BarangayForm
        frm.IsAdminMode = False
        frm.Show()
        Me.Hide()
    End Sub

    Private Sub btnEvacuationPath_Click(sender As Object, e As EventArgs) Handles btnEvacuationPath.Click
        EvacuationForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnViewAlerts_Click(sender As Object, e As EventArgs) Handles btnViewAlerts.Click
        ' IsAdminMode = False hides pnlStatus and btnClearAlerts inside AlertForm_Load
        AlertForm.IsAdminMode = False
        AlertForm.label.Text = "View Flood Alerts"
        AlertForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnMarkFlooded_Click(sender As Object, e As EventArgs) Handles btnMarkFlooded.Click
        Dim frm As New BarangayForm
        frm.IsAdminMode = True
        frm.Show()
        Me.Hide()
    End Sub

    Private Sub btnManageAlerts_Click(sender As Object, e As EventArgs) Handles btnManageAlerts.Click
        ' IsAdminMode = True shows pnlStatus and btnClearAlerts inside AlertForm_Load
        AlertForm.IsAdminMode = True
        AlertForm.label.Text = "Manage System Alerts"
        AlertForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click
        ReportForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnResetFlood_Click(sender As Object, e As EventArgs) Handles btnResetFlood.Click
        Dim result = MessageBox.Show("Reset all flood statuses to normal?", "Confirm Reset",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result = DialogResult.Yes Then
            Dim svc As New FloodStatusService()
            If svc.ResetAll() Then
                MessageBox.Show("All statuses have been reset.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        LoginForm.Show()
        Me.Close()
    End Sub

    Private Sub pnl1_Paint(sender As Object, e As PaintEventArgs) Handles pnl1.Paint

    End Sub

End Class
