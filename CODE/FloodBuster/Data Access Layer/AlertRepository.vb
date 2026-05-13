Imports System.Data
Imports System.Data.SqlClient

Public Class AlertRepository

    ' READ - Get active alerts with Barangay Names
    Public Function GetActive() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                Dim sql As String = "SELECT A.AlertID, B.BarangayName, A.AlertMessage, A.AlertLevel, A.DateIssued " &
                                    "FROM FloodAlerts A " &
                                    "INNER JOIN Barangays B ON A.BarangayID = B.BarangayID " &
                                    "WHERE A.IsCleared = 0"
                Dim adapter As New SqlDataAdapter(sql, conn)
                adapter.Fill(dt)
            End Using
        Catch ex As Exception
            MessageBox.Show("Repository Error: " & ex.Message)
        End Try
        Return dt
    End Function

    ' CREATE - Issue a new alert
    Public Function Add(barangayId As Integer, message As String, level As String) As Boolean
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                Dim sql As String = "INSERT INTO FloodAlerts (BarangayID, AlertMessage, AlertLevel, IsCleared) " &
                                    "VALUES (@bid, @msg, @lvl, 0)"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@bid", barangayId)
                    cmd.Parameters.AddWithValue("@msg", message)
                    cmd.Parameters.AddWithValue("@lvl", level)
                    Return cmd.ExecuteNonQuery() > 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error issuing alert: " & ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' UPDATE - Clear all alerts (Admin only)
    Public Function ClearAll() As Boolean
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                Dim cmd As New SqlCommand(
                    "UPDATE FloodAlerts SET IsCleared = 1 WHERE IsCleared = 0", conn)
                cmd.ExecuteNonQuery()
                Return True
            End Using
        Catch ex As Exception
            MessageBox.Show("Error clearing alerts: " & ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' DELETE - Soft delete a specific alert by ID (Admin only)
    Public Function DeleteById(alertId As Integer) As Boolean
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                conn.Open()
                Dim sql As String = "UPDATE FloodAlerts SET IsCleared = 1 WHERE AlertID = @id"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", alertId)
                    Return cmd.ExecuteNonQuery() > 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error deleting alert: " & ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

End Class
