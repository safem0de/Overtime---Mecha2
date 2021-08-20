Imports System.Data.SqlClient

Public Class AddPosition
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("CanAccess") Then
            Response.Redirect("~/Login.aspx")
        End If

        If Not (Session("Position") = "STAFF" _
                Or Session("Position") = "CLERK") Then
            Response.Redirect("~/Default.aspx")
        End If

        If Not IsPostBack Then

        End If
        Loadpage()
    End Sub
    Sub Loadpage()
        Dim SqlPosition = "
        SELECT *
        FROM [Manpower_Mecha2].[dbo].[Position_Master]
        "
        StandardFunction.fillDataTableToDataGrid(GrdPosition, SqlPosition, "")
    End Sub

    Private Sub GrdPosition_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GrdPosition.RowDeleting

    End Sub

    Private Sub GrdPosition_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdPosition.SelectedIndexChanged
        Dim row = GrdPosition.SelectedRow
        TxtPosition.Text = row.Cells(2).Text
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        Dim SqlAdd = "
        INSERT INTO [Manpower_Mecha2].[dbo].[Position_Master]
                   ([Position])
             VALUES ('" & TxtPosition.Text & "')"

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlAdd, con)
            command.ExecuteNonQuery()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('เกิดข้อผิดพลาด');", True)
        Finally
            con.Close()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('OK');", True)
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        End Try
    End Sub
End Class