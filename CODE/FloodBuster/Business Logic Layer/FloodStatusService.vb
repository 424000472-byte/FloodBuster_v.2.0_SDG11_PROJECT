Public Class FloodStatusService

    Private _brgyRepo As New BarangayRepository()
    Private _alertService As New AlertService()

    ' Get all barangays (used by BarangayForm grid)
    Public Function GetAll() As DataTable
        Return _brgyRepo.GetAll()
    End Function

    ' Add a new barangay — validates name is not empty
    Public Function AddBarangay(name As String) As Boolean
        If String.IsNullOrWhiteSpace(name) Then
            MessageBox.Show("Barangay name cannot be empty.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return _brgyRepo.Add(name)
    End Function

    ' Mark a barangay as flooded AND auto-generate a CRITICAL alert
    Public Function MarkFlooded(barangayId As Integer, barangayName As String) As Boolean
        If Not _brgyRepo.MarkFlooded(barangayId) Then Return False

        ' Auto-generate a CRITICAL alert when a barangay is marked flooded
        Dim message As String = barangayName & " is FLOODED. Please evacuate immediately if necessary."
        _alertService.IssueAlert(barangayId, message, "CRITICAL")

        Return True
    End Function

    ' Reset all barangays to Safe
    Public Function ResetAll() As Boolean
        Return _brgyRepo.ResetAll()
    End Function

    ' Delete a barangay
    Public Function DeleteBarangay(barangayId As Integer) As Boolean
        Return _brgyRepo.Delete(barangayId)
    End Function

End Class
