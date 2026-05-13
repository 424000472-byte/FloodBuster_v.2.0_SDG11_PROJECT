Imports System.Data
Imports System.Data.SqlClient

Public Class BarangayRepository

    ' READ - Get simple list of barangays and their flood status
    Public Function GetAll() As DataTable
        Dim dt As New DataTable()
        Using conn As SqlConnection = DatabaseConnection.GetConnection()
            conn.Open()
            Dim sql As String = "SELECT BarangayID, BarangayName, IsFlooded, LastUpdated FROM Barangays ORDER BY BarangayName"
            Dim adapter As New SqlDataAdapter(sql, conn)
            adapter.Fill(dt)
        End Using
        Return dt
    End Function

    ' CREATE - Add new barangay
    Public Function Add(name As String) As Boolean
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                Dim cmd As New SqlCommand(
                    "INSERT INTO Barangays (BarangayName, IsFlooded) VALUES (@Name, 0)", conn)
                cmd.Parameters.AddWithValue("@Name", name)
                cmd.ExecuteNonQuery()
                Return True
            End Using
        Catch ex As Exception
            MessageBox.Show("Error adding barangay: " & ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' UPDATE - Mark barangay as flooded
    Public Function MarkFlooded(barangayId As Integer) As Boolean
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                Dim cmd As New SqlCommand(
                    "UPDATE Barangays SET IsFlooded = 1, LastUpdated = GETDATE() WHERE BarangayID = @ID", conn)
                cmd.Parameters.AddWithValue("@ID", barangayId)
                cmd.ExecuteNonQuery()
                Return True
            End Using
        Catch ex As Exception
            MessageBox.Show("Error marking flooded: " & ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' UPDATE - Reset all flood statuses to Safe
    Public Function ResetAll() As Boolean
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                Dim cmd As New SqlCommand(
                    "UPDATE Barangays SET IsFlooded = 0, LastUpdated = GETDATE()", conn)
                cmd.ExecuteNonQuery()
                Return True
            End Using
        Catch ex As Exception
            MessageBox.Show("Error resetting flood status: " & ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' DELETE - Remove a barangay
    ' Must delete linked FloodAlerts first before deleting the barangay
    Public Function Delete(barangayId As Integer) As Boolean
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()

                ' Step 1: Delete all alerts linked to this barangay first
                Dim deleteAlerts As New SqlCommand(
                    "DELETE FROM FloodAlerts WHERE BarangayID = @ID", conn)
                deleteAlerts.Parameters.AddWithValue("@ID", barangayId)
                deleteAlerts.ExecuteNonQuery()

                ' Step 2: Now safe to delete the barangay itself
                Dim deleteBarangay As New SqlCommand(
                    "DELETE FROM Barangays WHERE BarangayID = @ID", conn)
                deleteBarangay.Parameters.AddWithValue("@ID", barangayId)
                deleteBarangay.ExecuteNonQuery()

                Return True
            End Using
        Catch ex As Exception
            MessageBox.Show("Error deleting barangay: " & ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' GET ONLY CRITICAL AREAS
    Public Function GetCriticalAreas() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                Dim sql As String = "SELECT BarangayName, LastUpdated FROM Barangays WHERE IsFlooded = 1"
                Dim adapter As New SqlDataAdapter(sql, conn)
                adapter.Fill(dt)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching critical areas: " & ex.Message)
        End Try
        Return dt
    End Function

End Class
