Imports System.Data.SqlClient

Public Class AddSection
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("CanAccess") Then
            Response.Redirect("~/Login.aspx")
        End If

        If Not (Session("Position") = "STAFF" _
                Or Session("Position") = "LEADER" _
                Or Session("Position") = "CLERK") Then
            Response.Redirect("~/Default.aspx")
        End If

        If Not IsPostBack Then

        End If
        Loadpage()
    End Sub

    Sub Loadpage()
        Dim SqlSection = "
        SELECT [Section_Name] FROM [Manpower_Mecha2].[dbo].[Section_Master]
        "
        StandardFunction.fillDataTableToDataGrid(GrdSection, SqlSection, "")
    End Sub

    Private Sub GrdSection_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GrdSection.RowDeleting

        Dim row = GrdSection.Rows(e.RowIndex)
        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim sqlDel = "
        DELETE FROM [Manpower_Mecha2].[dbo].[Section_Master]
        WHERE [Section_name] = '" & row.Cells(2).Text & "';"

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

    Private Sub GrdSection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdSection.SelectedIndexChanged
        Dim row = GrdSection.SelectedRow
        TxtSection.Text = row.Cells(2).Text
        Session("EditSection") = TxtSection.Text
    End Sub

    Sub Clearform()
        TxtSection.Text = Nothing
        GrdSection.SelectedIndex = -1
    End Sub

    Private Sub BtnCancle_Click(sender As Object, e As EventArgs) Handles BtnCancle.Click
        Clearform()
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        Dim SqlEdit = "
        UPDATE [Manpower_Mecha2].[dbo].[Section_Master]
           SET [Section_Name] = '" & TxtSection.Text & "'
         WHERE [Section_Name] = '" & Session("EditSection") & "'"

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlEdit, con)
            command.ExecuteNonQuery()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('เกิดข้อผิดพลาด');", True)
        Finally
            con.Close()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('OK');", True)
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        End Try
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        Dim SqlAdd = "
        INSERT INTO [Manpower_Mecha2].[dbo].[Section_Master]
                   ([Section_Name])
             VALUES ('" & TxtSection.Text & "')"

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

    Private Sub GrdSection_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GrdSection.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim item As String = e.Row.Cells(2).Text
            For Each button As Button In e.Row.Cells(1).Controls.OfType(Of Button)()
                If button.CommandName = "Delete" Then
                    button.Attributes("onclick") = "if(!confirm('ยืนยัน การลบ " + item + "?')){ return false; };"
                End If
            Next
        End If
    End Sub
End Class