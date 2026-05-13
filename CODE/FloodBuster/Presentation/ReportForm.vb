Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.IO

Public Class ReportForm
    ' --- 1. THE BRAIN (ReportDocument) ---
    Dim rpt As New FloodReport()

    Private Sub ReportForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigureReportConnection()
    End Sub

    ''' <summary>
    ''' Sets up the Database connection, refreshes data, and binds to the viewer.
    ''' </summary>
    Private Sub ConfigureReportConnection()
        Try
            ' 2. Database Connection Setup
            Dim connectionInfo As New ConnectionInfo()
            connectionInfo.ServerName = "MAVI\SQLEXPRESS"
            connectionInfo.DatabaseName = "FloodBusterDB"
            connectionInfo.IntegratedSecurity = True

            ' Apply login info to every table in the report
            For Each table As Table In rpt.Database.Tables
                Dim tableLogOnInfo As TableLogOnInfo = table.LogOnInfo
                tableLogOnInfo.ConnectionInfo = connectionInfo
                table.ApplyLogOnInfo(tableLogOnInfo)
            Next

            ' --- 3. REFRESH LOGIC ---
            ' This ensures that if you just added a new alert, it shows up now.
            rpt.Refresh()
            ' We clear any old filters so it shows the full official list
            rpt.RecordSelectionFormula = ""

            ' --- 4. THE FACE (CrystalReportViewer) ---
            CrystalReportViewer1.ReportSource = rpt
            CrystalReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show("Database Connection Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Exports the full official report to PDF.
    ''' </summary>
    Private Sub btnExportPDF_Click(sender As Object, e As EventArgs) Handles btnExportPDF.Click
        Try
            Dim saveDialog As New SaveFileDialog()
            saveDialog.Filter = "PDF Files (*.pdf)|*.pdf"
            saveDialog.Title = "Save Official Flood Report"
            saveDialog.FileName = "Official_Flood_Report_" & DateTime.Now.ToString("yyyyMMdd")

            If saveDialog.ShowDialog() = DialogResult.OK Then
                rpt.ExportToDisk(ExportFormatType.PortableDocFormat, saveDialog.FileName)
                MessageBox.Show("Official PDF exported successfully!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Export failed: " & ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' --- 5. CLEANUP ---
        rpt.Close()
        rpt.Dispose()

        DashboardForm.Show()
        Me.Close()
    End Sub
End Class
