Imports System.Data.SqlClient

Public Class AddCompany
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
        Dim SqlCompany = "
        SELECT *
        FROM [Manpower_Mecha2].[dbo].[Company_Master]
        "
        StandardFunction.fillDataTableToDataGrid(GrdCompany, SqlCompany, "")
    End Sub

    Private Sub GrdCompany_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GrdCompany.RowDeleting
        Dim row = GrdCompany.Rows(e.RowIndex)
        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim sqlDel = "
        DELETE FROM [Manpower_Mecha2].[dbo].[Company_Master]
        WHERE [Company_name] = '" & row.Cells(2).Text & "';"

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(sqlDel, con)
            command.ExecuteNonQuery()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('เกิดข้อผิดพลาด');", True)
        Finally
            con.Close()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('OK');", True)
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        End Try

    End Sub

    Private Sub GrdCompany_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdCompany.SelectedIndexChanged
        Dim row = GrdCompany.SelectedRow
        TxtCompany.Text = row.Cells(2).Text
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        Dim SqlAdd = "
        INSERT INTO [Manpower_Mecha2].[dbo].[Company_Master]
                   ([Company_name])
             VALUES ('" & TxtCompany.Text & "')"

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