Public Class EvacuationService

    Private _evacRepo As New EvacuationRepository()

    ' Get all barangays for the dropdown
    Public Function GetAllBarangays() As DataTable
        Return _evacRepo.GetAllBarangays()
    End Function

    ' Get evacuation path recommendations for a selected barangay
    Public Function GetRecommendations(barangayName As String) As DataTable
        If String.IsNullOrWhiteSpace(barangayName) Then
            MessageBox.Show("Please select a barangay.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return New DataTable()
        End If
        Return _evacRepo.GetPathRecommendations(barangayName)
    End Function

End Class
