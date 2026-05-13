Public Class EvacuationForm
    Private _svc As New EvacuationService()

    Private Sub EvacuationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateBarangayDropdown()
    End Sub

    Private Sub PopulateBarangayDropdown()
        Dim dt As DataTable = _svc.GetAllBarangays()
        If dt.Rows.Count > 0 Then
            cboBarangay.DataSource = dt
            cboBarangay.DisplayMember = "BarangayName"
            cboBarangay.ValueMember = "BarangayID"
        End If
    End Sub

    Private Sub btnGetRecommendation_Click(sender As Object, e As EventArgs) Handles btnGetRecommendation.Click
        Dim selectedLoc As String = cboBarangay.Text
        Dim dt As DataTable = _svc.GetRecommendations(selectedLoc)

        dgvResults.DataSource = dt

        If dt.Rows.Count > 0 Then
            lblStatus.Text = "Recommended: " & dt.Rows(0)("Evacuation Center").ToString()
            dgvResults.Rows(0).DefaultCellStyle.BackColor = Color.LightGreen
        Else
            lblStatus.Text = "No path found for this location."
        End If
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        DashboardForm.Show()
        Me.Close()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
