Public Class BarangayForm
    ' --- Properties and Variables ---
    Public Property IsAdminMode As Boolean = False
    Private _svc As New FloodStatusService()    ' BLL service replaces direct repo
    Private barangayData As DataTable           ' Holds the data in memory for fast searching
    Private showOnlyFlooded As Boolean = False  ' Tracks the filter state

    ' --- Form Initialization ---
    Private Sub BarangayForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.StartPosition = FormStartPosition.CenterScreen

        ' 1. Set up UI based on Admin or User mode
        ApplyViewSettings()

        ' 2. Load the initial data from the service
        RefreshBarangayList()
    End Sub

    ' --- Data Loading ---
    Private Sub RefreshBarangayList()
        ' Fetch fresh data through the service
        barangayData = _svc.GetAll()

        ' Display in the grid
        dgvBarangays.DataSource = barangayData

        ' Format the columns for a professional look
        If dgvBarangays.Columns.Contains("BarangayName") Then
            dgvBarangays.Columns("BarangayID").HeaderText = "ID"
            dgvBarangays.Columns("BarangayID").Width = 50
            dgvBarangays.Columns("BarangayName").HeaderText = "Barangay Name"
            dgvBarangays.Columns("BarangayName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvBarangays.Columns("IsFlooded").HeaderText = "Flooded?"
            dgvBarangays.Columns("LastUpdated").HeaderText = "Last Updated"
        End If
    End Sub

    ' --- Search & Filter Logic ---

    ' Real-time search as you type
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        PerformSearchAndFilter()
    End Sub

    ' Toggle button to show only flooded areas
    Private Sub btnFilterFlooded_Click(sender As Object, e As EventArgs) Handles btnFilterFlooded.Click
        showOnlyFlooded = Not showOnlyFlooded

        If showOnlyFlooded Then
            btnFilterFlooded.Text = "Showing: Flooded"
            btnFilterFlooded.BackColor = Color.LightCoral
        Else
            btnFilterFlooded.Text = "Show Only Flooded"
            btnFilterFlooded.BackColor = Color.FromKnownColor(KnownColor.Control)
        End If

        PerformSearchAndFilter()
    End Sub

    ' The core logic that handles both Search and Filter together
    Private Sub PerformSearchAndFilter()
        If barangayData IsNot Nothing Then
            Try
                Dim dv As New DataView(barangayData)

                Dim filterString As String = String.Format("BarangayName LIKE '%{0}%'", txtSearch.Text.Replace("'", "''"))

                If showOnlyFlooded Then
                    filterString &= " AND IsFlooded = 1"
                End If

                dv.RowFilter = filterString
                dgvBarangays.DataSource = dv
            Catch ex As Exception
                MessageBox.Show("Filter Error: " & ex.Message)
            End Try
        End If
    End Sub

    ' --- Admin Action Buttons ---

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Validation and add through FloodStatusService
        If _svc.AddBarangay(txtBarangayName.Text) Then
            txtBarangayName.Clear()
            RefreshBarangayList()
        End If
    End Sub

    Private Sub btnMarkFlooded_Click(sender As Object, e As EventArgs) Handles btnMarkFlooded.Click
        If dgvBarangays.CurrentRow IsNot Nothing Then
            Dim id As Integer = Convert.ToInt32(dgvBarangays.CurrentRow.Cells("BarangayID").Value)
            Dim name As String = dgvBarangays.CurrentRow.Cells("BarangayName").Value.ToString()

            ' MarkFlooded now also auto-issues a CRITICAL alert via the service
            If _svc.MarkFlooded(id, name) Then
                MessageBox.Show(name & " marked as flooded.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                RefreshBarangayList()
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If dgvBarangays.CurrentRow IsNot Nothing Then
            Dim id As Integer = Convert.ToInt32(dgvBarangays.CurrentRow.Cells("BarangayID").Value)
            If MessageBox.Show("Are you sure you want to delete this?", "Confirm",
                               MessageBoxButtons.YesNo) = DialogResult.Yes Then
                If _svc.DeleteBarangay(id) Then
                    RefreshBarangayList()
                End If
            End If
        End If
    End Sub

    ' --- UI Settings & Navigation ---
    Private Sub ApplyViewSettings()
        ' Admin Controls
        lblMode.Visible = IsAdminMode
        lblBarangayName.Visible = IsAdminMode
        txtBarangayName.Visible = IsAdminMode
        btnAdd.Visible = IsAdminMode
        btnMarkFlooded.Visible = IsAdminMode
        btnDelete.Visible = IsAdminMode

        ' Common/User Controls
        lblmode2.Visible = Not IsAdminMode
        dgvBarangays.Visible = True
        btnBack.Visible = True

        If IsAdminMode Then
            lblMode.Text = "Flood Management"
            Me.Text = "FloodBuster - Administrator"
            dgvBarangays.ReadOnly = False
        Else
            lblmode2.Text = "Current Flood Status"
            Me.Text = "FloodBuster - Public View"
            dgvBarangays.ReadOnly = True
        End If
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        DashboardForm.Show()
        Me.Close()
    End Sub

End Class
