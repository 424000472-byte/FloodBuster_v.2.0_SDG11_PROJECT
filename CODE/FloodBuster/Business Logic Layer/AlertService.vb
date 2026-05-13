Public Class AlertService

    Private _alertRepo As New AlertRepository()

    ' Issue a new alert — called manually from AlertForm OR automatically from FloodStatusService
    Public Function IssueAlert(barangayId As Integer, message As String, level As String) As Boolean
        If String.IsNullOrWhiteSpace(message) Then
            MessageBox.Show("Alert message cannot be empty.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return _alertRepo.Add(barangayId, message, level)
    End Function

    ' Get all active (uncleared) alerts for display
    Public Function GetActiveAlerts() As DataTable
        Return _alertRepo.GetActive()
    End Function

    ' Clear all alerts — Admin only (enforced in the UI)
    Public Function ClearAll() As Boolean
        Return _alertRepo.ClearAll()
    End Function

    ' Delete a specific alert by ID — Admin only (enforced in the UI)
    Public Function DeleteAlert(alertId As Integer) As Boolean
        Return _alertRepo.DeleteById(alertId)
    End Function

End Class
