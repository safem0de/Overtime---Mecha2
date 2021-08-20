Imports Microsoft.Reporting.WebForms

Public Class PrintEAR
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            ReportViewerEAR.LocalReport.DataSources.Clear()
            Dim dsList As New DataTable

            ReportViewerEAR.ProcessingMode = ProcessingMode.Local
            ReportViewerEAR.LocalReport.ReportPath = Server.MapPath("~/EmployeeAction.rdlc")
            Dim datasource As New ReportDataSource("DataChangeShift", dsList)
            ReportViewerEAR.LocalReport.DataSources.Add(datasource)
        End If

    End Sub

End Class