Public Class AlertForm
    Private _alertSvc As New AlertService()
    Private _floodSvc As New FloodStatusService()

    ' Set this from DashboardForm before calling .Show()
    Public Property IsAdminMode As Boolean = False

    ' 1. INITIAL LOAD
    Private Sub AlertForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.StartPosition = FormStartPosition.CenterScreen
        LoadBarangays()
        SetupLevelComboBox()
        RefreshGrid()
        UpdateFloodStatus()

        ' Hide or show admin controls based on role
        PnlStatus.Visible = IsAdminMode
        btnClearAlerts.Visible = IsAdminMode
        btnDeleteAlert.Visible = IsAdminMode
        lblBarangay.Visible = IsAdminMode
        cmbBarangay.Visible = IsAdminMode
        lblLevel.Visible = IsAdminMode
        cmbLevel.Visible = IsAdminMode
        lblMessage.Visible = IsAdminMode
        txtAlertMessage.Visible = IsAdminMode
        btnIssue.Visible = IsAdminMode
    End Sub

    ' Fill the Barangay Dropdown from the service
    Private Sub LoadBarangays()
        Dim dt As DataTable = _floodSvc.GetAll()
        cmbBarangay.DataSource = dt
        cmbBarangay.DisplayMember = "BarangayName"
        cmbBarangay.ValueMember = "BarangayID"
    End Sub

    ' Fill the Alert Level Dropdown manually
    Private Sub SetupLevelComboBox()
        cmbLevel.Items.Clear()
        cmbLevel.Items.AddRange(New String() {"LOW", "MODERATE", "CRITICAL"})
        cmbLevel.SelectedIndex = 0
    End Sub

    ' 2. REFRESH DATA & STATUS
    Private Sub RefreshGrid()
        Try
            Dim dt As DataTable = _alertSvc.GetActiveAlerts()
            dgvAlerts.DataSource = Nothing
            dgvAlerts.Rows.Clear()
            dgvAlerts.Columns.Clear()
            dgvAlerts.DataSource = dt

            If dgvAlerts.Columns.Count > 0 Then
                If dgvAlerts.Columns.Contains("AlertID") Then
                    dgvAlerts.Columns("AlertID").Visible = False
                End If
                dgvAlerts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Else
                Console.WriteLine("Database returned no alerts.")
            End If

            UpdateFloodStatus()
        Catch ex As Exception
            MessageBox.Show("Grid Refresh Error: " & ex.Message)
        End Try
    End Sub

    ' Check for flooded areas and update the status label
    Private Sub UpdateFloodStatus()
        Dim dt As DataTable = _floodSvc.GetAll()
        Dim floodedCount As Integer = dt.Select("IsFlooded = True OR IsFlooded = 1").Length
        If floodedCount > 0 Then
            lblStatusSummary.Text = $"⚠️ STATUS: CRITICAL ({floodedCount} Areas Flooded)"
            lblStatusSummary.ForeColor = Color.Red
        Else
            lblStatusSummary.Text = "✅ STATUS: ALL AREAS SAFE"
            lblStatusSummary.ForeColor = Color.DarkGreen
        End If
    End Sub

    ' 3. REFRESH BUTTON
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        RefreshGrid()
    End Sub

    ' 4. ISSUE NEW ALERT
    Private Sub btnIssue_Click(sender As Object, e As EventArgs) Handles btnIssue.Click
        Dim barangayId As Integer = Convert.ToInt32(cmbBarangay.SelectedValue)
        Dim message As String = txtAlertMessage.Text
        Dim level As String = cmbLevel.SelectedItem.ToString()
        If _alertSvc.IssueAlert(barangayId, message, level) Then
            MessageBox.Show("Alert Issued Successfully!", "FloodBuster")
            txtAlertMessage.Clear()
            RefreshGrid()
        End If
    End Sub

    ' 5. CLEAR ALL ALERTS
    Private Sub btnClearAll_Click(sender As Object, e As EventArgs) Handles btnClearAlerts.Click
        Dim confirm As DialogResult = MessageBox.Show(
            "Are you sure you want to clear all active alerts?",
            "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If confirm = DialogResult.Yes Then
            If _alertSvc.ClearAll() Then
                MessageBox.Show("All alerts have been cleared.")
                RefreshGrid()
            End If
        End If
    End Sub

    ' 6. DELETE SPECIFIC ALERT (Admin only)
    Private Sub btnDeleteAlerts_Click(sender As Object, e As EventArgs) Handles btnDeleteAlert.Click
        ' Guard: must have a row selected
        If dgvAlerts.CurrentRow Is Nothing Then
            MessageBox.Show("Please select an alert to delete.", "No Selection",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim alertId As Integer = Convert.ToInt32(dgvAlerts.CurrentRow.Cells("AlertID").Value)
        Dim barangayName As String = dgvAlerts.CurrentRow.Cells("BarangayName").Value.ToString()

        Dim confirm As DialogResult = MessageBox.Show(
            $"Delete the alert for ""{barangayName}""?" & Environment.NewLine & "This action cannot be undone.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning)

        If confirm = DialogResult.Yes Then
            If _alertSvc.DeleteAlert(alertId) Then
                MessageBox.Show("Alert deleted successfully.", "FloodBuster",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                RefreshGrid()
            End If
        End If
    End Sub

    ' 7. NAVIGATION
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        DashboardForm.Show()
        Me.Close()
    End Sub

    Private Sub lblBarangay_Click(sender As Object, e As EventArgs) Handles lblBarangay.Click
    End Sub

    Private Sub cmbBarangay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBarangay.SelectedIndexChanged
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
    End Sub

End Class
