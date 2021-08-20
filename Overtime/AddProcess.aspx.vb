Imports System.Data.SqlClient

Public Class AddProcess
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
            LoadPage()
        End If

    End Sub
    Sub LoadPage()
        Dim SqlProcess = "
        SELECT [Process_Name]
              ,[Process_Code]
	          ,t2.[Section_Name]
          FROM [Manpower_Mecha2].[dbo].[Process_Master] as t1
          LEFT JOIN [Manpower_Mecha2].[dbo].[Section_Master] as t2
          ON t1.Section_Id = t2.Section_Id
        "
        StandardFunction.fillDataTableToDataGrid(GrdProcess, SqlProcess, "")

        Dim SqlSection = "
        SELECT [Section_Name]
        FROM [Manpower_Mecha2].[dbo].[Section_Master]"

        StandardFunction.setDropdownlist(DrpSection, SqlSection, "")
    End Sub

    Private Sub GrdProcess_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GrdProcess.RowDeleting
        Dim row = GrdProcess.Rows(e.RowIndex)
        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim sqlDel = "
        DELETE FROM [Manpower_Mecha2].[dbo].[Process_Master]
      WHERE [Process_Name] = '" & row.Cells(2).Text.Replace("&amp;", "&").Replace("'", "''") & "';"

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

    Private Sub GrdProcess_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdProcess.SelectedIndexChanged
        Dim row = GrdProcess.SelectedRow
        TxtProcessName.Text = row.Cells(2).Text.Replace("&amp;", "&")
        TxtProcessCode.Text = row.Cells(3).Text
        DrpSection.SelectedValue = row.Cells(4).Text
    End Sub

    Protected Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        Dim Check = True
        Dim Alert As New StringBuilder("กรุณาเลือก\n")

        If TxtProcessName.Text = Nothing Then
            Check = False
            Alert.Append("- ชื่อกระบวนการ\n")
        End If

        If TxtProcessCode.Text = Nothing And Len(TxtProcessCode.Text) < 11 Then
            Check = False
            Alert.Append("- รหัสกระบวนการ\n")
        End If

        If DrpSection.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- ส่วนงาน\n")
        End If

        If Check = False Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert.ToString & "');", True)
            Exit Sub
        Else
            Dim SqlGetSectionId = "
                SELECT [Section_Id]
                  FROM [Manpower_Mecha2].[dbo].[Section_Master]
                WHERE [Section_Name] = '" & DrpSection.SelectedValue & "'
            "
            Dim x = StandardFunction.getSQLDataString(SqlGetSectionId)

            Dim SqlEdit = "
                UPDATE [Manpower_Mecha2].[dbo].[Process_Master]
                   SET [Process_Code] = '" & TxtProcessCode.Text & "'
                      ,[Section_Id] = " & CInt(x) & "
                 WHERE [Process_Name] = '" & TxtProcessName.Text & "'
            "
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
        End If

    End Sub

    Protected Sub BtnCancle_Click(sender As Object, e As EventArgs) Handles BtnCancle.Click
        Clearform()
    End Sub

    Sub Clearform()
        TxtProcessName.Text = ""
        TxtProcessCode.Text = ""
        DrpSection.SelectedIndex = 0
        GrdProcess.SelectedIndex = -1
    End Sub

    Protected Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click

        Dim Check = True
        Dim Alert As New StringBuilder("กรุณาเลือก\n")

        If TxtProcessName.Text = Nothing Then
            Check = False
            Alert.Append("- ชื่อกระบวนการ\n")
        End If

        If TxtProcessCode.Text = Nothing And Len(TxtProcessCode.Text) < 11 Then
            Check = False
            Alert.Append("- รหัสกระบวนการ\n")
        End If

        If DrpSection.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- ส่วนงาน\n")
        End If

        If Check = False Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert.ToString & "');", True)
            Exit Sub
        Else
            Dim SqlGetSectionId = "
                SELECT [Section_Id]
                  FROM [Manpower_Mecha2].[dbo].[Section_Master]
                WHERE [Section_Name] = '" & DrpSection.SelectedValue & "'
            "
            Dim x = StandardFunction.getSQLDataString(SqlGetSectionId)

            Dim SqlAdd = "
                INSERT INTO [Manpower_Mecha2].[dbo].[Process_Master]
               ([Process_Name]
               ,[Process_Code]
               ,[Section_Id])
                VALUES
               ('" & TxtProcessName.Text.Replace("'", "''") & "'
               ,'" & TxtProcessCode.Text & "'
               ," & CInt(x) & ")
            "

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
        End If

    End Sub

    Private Sub GrdProcess_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GrdProcess.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim item As String = e.Row.Cells(2).Text.Replace("&amp;", "&")
            For Each button As Button In e.Row.Cells(1).Controls.OfType(Of Button)()
                If button.CommandName = "Delete" Then
                    button.Attributes("onclick") = "if(!confirm('ยืนยัน การลบ " + item + "?')){ return false; };"
                End If
            Next
        End If
    End Sub
End Class