Imports System.Data
Imports System.Data.SqlClient

Public Class EvacuationRepository

    ' Get list for Dropdown
    Public Function GetAllBarangays() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                ' Must select both ID and Name to avoid ComboBox errors
                Dim adapter As New SqlDataAdapter(
                    "SELECT BarangayID, BarangayName FROM Barangays ORDER BY BarangayName", conn)
                adapter.Fill(dt)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading barangays: " & ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function

    ' Get Recommendation Data
    Public Function GetPathRecommendations(userBarangay As String) As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                Dim sql As String =
                    "SELECT ec.CenterName AS [Evacuation Center], " &
                    "b.BarangayName AS [Location], " &
                    "bc.DistanceMeters AS [Distance], " &
                    "bc.RecommendedRoute AS [Safe Path], " &
                    "(ec.MaxCapacity - ec.CurrentOccupancy) AS [Available Slots] " &
                    "FROM EvacuationCenters ec " &
                    "JOIN Barangays b ON ec.BarangayID = b.BarangayID " &
                    "JOIN BarangayConnections bc ON ec.BarangayID = bc.ToBarangayID " &
                    "JOIN Barangays u ON bc.FromBarangayID = u.BarangayID " &
                    "WHERE u.BarangayName = @UserLoc"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@UserLoc", userBarangay)
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(dt)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading recommendations: " & ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function

End Class
