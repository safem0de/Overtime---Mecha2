Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class Manpower
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("CanAccess") Then
            Response.Redirect("~/Login.aspx")
        End If

        If Not (Session("Position") = "STAFF" _
                Or Session("Position") = "LEADER" _
                Or Session("Position") = "CLERK") Then
            BtnAdd.Enabled = False
            BtnEdit.Enabled = False
        End If

        If Not Page.IsPostBack Then
            LoadPage()
        End If

        LoadTable()
    End Sub
    Sub LoadTable()
        Dim SqlGrdEmployee = "
        SELECT [EmpNo]
	        ,[Name]
	        ,[Surname]
	        ,[Status]
	        ,[Shift]
	        ,[Gender]
	        ,[Company]
	        ,[Section]
	        ,[Process]
	        ,[Position]
        FROM [Manpower_Mecha2].[dbo].[Emp_Master]
        WHERE NOT [Name] = 'ADMIN'
             AND [EmpNo] like '" + TxtEmpNo.Text + "%'
	         AND [Name] like '" + TxtName.Text + "%'
	         AND [Surname] like '" + TxtSurName.Text + "%'
	         AND [Status] like '" + DrpStatus.SelectedValue + "%'
	         AND [Shift] like '" + DrpShift.SelectedValue + "%'
	         AND [Gender] like '" + DrpGender.SelectedValue + "%'
	         AND [Company] like '" + DrpCompany.SelectedValue + "%'
	         AND [Section] like '" + DrpSection.SelectedValue + "%'
	         AND [Process] like '" + DrpProcess.SelectedValue.Replace("'", "''") + "%'
	         AND [Position] like '" + DrpPosition.SelectedValue + "%'
        "
        StandardFunction.fillDataTableToDataGrid(GrdEmployee, SqlGrdEmployee, "")
        Dim x = StandardFunction.GetDataTable(SqlGrdEmployee)
        Session("Check") = x
    End Sub
    Sub LoadPage()
        Dim SqlCompany = "SELECT * FROM [Manpower_Mecha2].[dbo].[Company_Master]"
        StandardFunction.setDropdownlist(DrpCompany, SqlCompany, "")
        Dim SqlSection = "SELECT [Section_Name] FROM [Manpower_Mecha2].[dbo].[Section_Master]"
        StandardFunction.setDropdownlist(DrpSection, SqlSection, "")
        Dim SqlProcess = "SELECT [Process_Name] FROM [Manpower_Mecha2].[dbo].[Process_Master]"
        StandardFunction.setDropdownlist(DrpProcess, SqlProcess, "")
        Dim SqlShift = "SELECT * FROM [Manpower_Mecha2].[dbo].[Shift_Master]"
        StandardFunction.setDropdownlist(DrpShift, SqlShift, "")
        Dim SqlPosition = "SELECT * FROM [Manpower_Mecha2].[dbo].[Position_Master]"
        StandardFunction.setDropdownlist(DrpPosition, SqlPosition, "")
    End Sub

    Sub Clearform()
        GrdEmployee.SelectedIndex = -1
        TxtEmpNo.Text = ""
        TxtName.Text = ""
        TxtSurName.Text = ""
        DrpStatus.SelectedIndex = 0
        DrpShift.SelectedIndex = 0
        DrpGender.SelectedIndex = 0
        DrpCompany.SelectedIndex = 0
        DrpSection.SelectedIndex = 0
        DrpProcess.SelectedIndex = 0
        DrpPosition.SelectedIndex = 0
        LoadTable()
    End Sub

    Private Sub GrdEmployee_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdEmployee.SelectedIndexChanged
        On Error Resume Next
        Dim arr As New ArrayList
        Dim row As GridViewRow = GrdEmployee.SelectedRow
        TxtEmpNo.Text = row.Cells(1).Text
        TxtName.Text = row.Cells(2).Text
        TxtSurName.Text = row.Cells(3).Text
        DrpStatus.Text = row.Cells(4).Text
        DrpShift.SelectedValue = row.Cells(5).Text.ToUpper
        DrpGender.SelectedValue = row.Cells(6).Text.ToUpper
        DrpCompany.SelectedValue = row.Cells(7).Text.ToUpper
        DrpSection.SelectedValue = row.Cells(8).Text.ToUpper
        DrpProcess.SelectedValue = row.Cells(9).Text.ToUpper.Replace("'", "''")
        DrpPosition.SelectedValue = row.Cells(10).Text.ToUpper
    End Sub

    Private Sub GrdEmployee_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GrdEmployee.PageIndexChanging
        GrdEmployee.PageIndex = e.NewPageIndex
        GrdEmployee.DataBind()
    End Sub

    Protected Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        Clearform()
    End Sub

    Protected Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        Dim x = Session("Check")
        If x.Rows.Count > 0 Then
        Else
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "window-script", "alert('ไม่พบรายการที่ท่านค้นหา');", True)
            Clearform()
        End If
    End Sub

    Protected Sub BtnDownloadformat_Click(sender As Object, e As EventArgs) Handles BtnDownloadformat.Click
        Dim SqlDownloadFormat = "
        SELECT [EmpNo]
              ,[Name]
              ,[Surname]
              ,[Status]
              ,[Shift]
              ,[Gender]
              ,[Mobile]
              ,[Company]
              ,[Section]
              ,[Process]
              ,[Position]
              ,[Start_date]
              ,[End_date]
          FROM [Manpower_Mecha2].[dbo].[Emp_Master]
          WHERE [EmpNo] = ''
        "
        Dim SectionProcess = "
        SELECT [Process_Name]
              ,[Process_Code]
	          ,t2.[Section_Name]
          FROM [Manpower_Mecha2].[dbo].[Process_Master] as t1
          LEFT JOIN [Manpower_Mecha2].[dbo].[Section_Master] as t2
          ON t1.[Section_Id] = t2.[Section_Id]
        "

        Dim Position = "
        SELECT [Position] FROM [Manpower_Mecha2].[dbo].[Position_Master]
        "

        Dim Gender = "
        DECLARE @myTableVariable TABLE (Sex varchar(1),Gender varchar(20))
        insert into @myTableVariable values('F','FEMALE (หญิง)'),('M','MALE (ชาย)')
        select * from @myTableVariable
        "

        Dim arr As New ArrayList
        arr.Add({SqlDownloadFormat, "Format"})
        arr.Add({SectionProcess, "Section_Process"})
        arr.Add({Position, "Position"})
        arr.Add({Gender, "Gender"})
        StandardFunction.ExportExcelMultiSheet(Me, arr, "Format Download")
    End Sub

    Protected Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click

        Dim Check = True
        Dim Alert As New StringBuilder("กรุณาระบุ\n")

        If TxtEmpNo.Text = Nothing Then
            Check = False
            Alert.Append("- รหัสพนักงาน (5 หลัก)\n")
        End If

        If TxtName.Text = Nothing Then
            Check = False
            Alert.Append("- ชื่อพนักงาน\n")
        End If

        If TxtSurName.Text = Nothing Then
            Check = False
            Alert.Append("- นามสกุลพนักงาน\n")
        End If

        If DrpGender.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- เพศพนักงาน\n")
        End If

        If DrpStatus.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- สถานะการทำงาน\n")
        End If

        If DrpSection.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- ส่วนงาน\n")
        End If

        If DrpProcess.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- ส่วนงาน\n")
        End If

        If DrpProcess.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- กระบวนการ\n")
        End If

        If DrpShift.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- กะการทำงาน\n")
        End If

        If DrpCompany.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- แผนก หรือ บริษัท\n")
        End If

        If DrpPosition.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- ตำแหน่ง หรือ หน้าที่\n")
        End If

        If Check = False Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert.ToString & "');", True)
            Exit Sub
        End If

        Dim SqlEdit As String = Nothing

        If DrpStatus.SelectedValue = "IN-ACTIVE" Then
            SqlEdit = "
               UPDATE [Manpower_Mecha2].[dbo].[Emp_Master]
                   SET [Name] = '" & TxtName.Text & "'
                      ,[Surname] = '" & TxtSurName.Text & "'
                      ,[Status] = 'IN-ACTIVE'
                      ,[Shift] = '" & DrpShift.SelectedValue & "'
                      ,[Gender] = '" & DrpGender.SelectedValue & "'
                      ,[Company] = '" & DrpCompany.SelectedValue & "'
                      ,[Section] = '" & DrpSection.SelectedValue & "'
                      ,[Process] = '" & DrpProcess.SelectedValue.Replace("'", "''") & "'
                      ,[Position] = '" & DrpPosition.SelectedValue & "'
                      ,[End_date] = Convert(varchar(10),GETDATE(),103)
                 WHERE [EmpNo] = '" & TxtEmpNo.Text & "'
           "
        ElseIf DrpStatus.SelectedValue = "ACTIVE" Then
            SqlEdit = "
               UPDATE [Manpower_Mecha2].[dbo].[Emp_Master]
                   SET [Name] = '" & TxtName.Text & "'
                      ,[Surname] = '" & TxtSurName.Text & "'
                      ,[Status] = 'ACTIVE'
                      ,[Shift] = '" & DrpShift.SelectedValue & "'
                      ,[Gender] = '" & DrpGender.SelectedValue & "'
                      ,[Company] = '" & DrpCompany.SelectedValue & "'
                      ,[Section] = '" & DrpSection.SelectedValue & "'
                      ,[Process] = '" & DrpProcess.SelectedValue.Replace("'", "''") & "'
                      ,[Position] = '" & DrpPosition.SelectedValue & "'
                 WHERE [EmpNo] = '" & TxtEmpNo.Text & "'
           "
        End If

        'TxtTest.Text += SqlEdit

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlEdit, con)
            command.ExecuteNonQuery()
        Catch ex As Exception
            TxtTest.Text += ex.Message
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & ex.Message & ");", True)
        Finally
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('แก้ไขรายชื่อสำเร็จ (Edit Completed)');", True)
            con.Close()
        End Try
        Clearform()
    End Sub

    Protected Sub BtnUpload_Click(sender As Object, e As EventArgs) Handles BtnUpload.Click
        Dim dtU As DataTable = StandardFunction.NewUploadXls(Me, GrdEmployee, DrpSheet, "Manpower_Mecha2", "Emp_Master")
        Dim Alert As New StringBuilder
        Dim InsertCount As Integer = 0
        Dim InsertStr As New StringBuilder(
        "INSERT INTO [Manpower_Mecha2].[dbo].[Emp_Master]
           ([EmpNo]
           ,[Name]
           ,[Surname]
           ,[Status]
           ,[Shift]
           ,[Gender]
           ,[Mobile]
           ,[Company]
           ,[Section]
           ,[Process]
           ,[Position]
           ,[Start_date])
        VALUES ")
        Dim UpdateStr As New StringBuilder

        If IsNothing(dtU) Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('Format ผิด');", True)
        Else
            For i = 0 To dtU.Rows.Count - 1
                Dim SQLSelect As String = "
                SELECT 'False'
                  FROM [Manpower_Mecha2].[dbo].[Emp_Master]
                  WHERE [EmpNo] ='" & dtU.Rows(i).Item("EmpNo") & "'
                "
                Dim SelectStr = StandardFunction.getSQLDataString(SQLSelect)
                If Not CBool(String.IsNullOrEmpty(SelectStr)) Then
                    'MsgBox("Update : " & dtU.Rows(i).Item("EmpNo"))

                    Dim Update_read As String = "
                        UPDATE [Manpower_Mecha2].[dbo].[Emp_Master]
                           SET [Name] = '" & dtU.Rows(i).Item("Name").ToString() & "'
                              ,[Surname] = '" & dtU.Rows(i).Item("Surname").ToString() & "'
                              ,[Status] = '" & dtU.Rows(i).Item("Status").ToString().ToUpper & "'
                              ,[Shift] = '" & dtU.Rows(i).Item("Shift").ToString().ToUpper & "'
                              ,[Gender] = '" & dtU.Rows(i).Item("Gender").ToString() & "'
                              ,[Mobile] = '" & dtU.Rows(i).Item("Mobile").ToString() & "'
                              ,[Company] = '" & dtU.Rows(i).Item("Company").ToString().Replace(Chr(34), """").Replace("'", "''").Trim().ToUpper & "'
                              ,[Section] = '" & dtU.Rows(i).Item("Section").ToString().Replace(Chr(34), """").Replace("'", "''").Trim().ToUpper & "'
                              ,[Process] = '" & dtU.Rows(i).Item("Process").ToString().Replace(Chr(34), """").Replace("'", "''").Trim().ToUpper & "'
                              ,[Position] = '" & dtU.Rows(i).Item("Position").ToString().ToUpper & "'
                              ,[Start_date] = Convert(varchar(10),'" & dtU.Rows(i).Item("Start_date").ToString() & "',23)
                         WHERE [EmpNo] = '" & dtU.Rows(i).Item("EmpNo").ToString() & "'
                    "

                    UpdateStr.Append(Update_read)

                Else
                    If Not IsNothing(dtU.Rows(i).Item("EmpNo").ToString()) Then
                        'MsgBox("Insert : " & dtU.Rows(i).Item("EmpNo"))
                        InsertCount += 1
                        InsertStr.Append(
                              "('" & dtU.Rows(i).Item("EmpNo").ToString() & "'
                           ,'" & dtU.Rows(i).Item("Name").ToString() & "'
                           ,'" & dtU.Rows(i).Item("Surname").ToString() & "'
                           ,'" & dtU.Rows(i).Item("Status").ToString() & "'
                           ,'" & dtU.Rows(i).Item("Shift").ToString().ToUpper & "'
                           ,'" & dtU.Rows(i).Item("Gender").ToString() & "'
                           ,'" & dtU.Rows(i).Item("Mobile").ToString() & "'
                           ,'" & dtU.Rows(i).Item("Company").ToString().Replace(Chr(34), """").Replace("'", "''").Trim().ToUpper & "'
                           ,'" & dtU.Rows(i).Item("Section").ToString().Replace(Chr(34), """").Replace("'", "''").Trim().ToUpper & "'
                           ,'" & dtU.Rows(i).Item("Process").ToString().Replace(Chr(34), """").Replace("'", "''").Trim().ToUpper & "'
                           ,'" & dtU.Rows(i).Item("Position").ToString().ToUpper & "'
                           , Convert(varchar(10),'" & dtU.Rows(i).Item("Start_date").ToString() & "',23)),")
                    End If
                End If
            Next

            TxtTest.Text += UpdateStr.ToString
            TxtTest.Text += InsertStr.ToString.Substring(0, InsertStr.Length - 1)

            Dim con As New SqlConnection
            Dim command As SqlCommand


            If Not String.IsNullOrEmpty(UpdateStr.ToString) Then
                Try
                    con.ConnectionString = StandardFunction.connectionString
                    con.Open()
                    command = New SqlCommand(UpdateStr.ToString, con)
                    command.ExecuteNonQuery()
                Catch ex As Exception
                    TxtTest.Text += ex.Message
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('Update ผิด'" & ex.Message & ");", True)
                Finally
                    con.Close()
                End Try
            End If

            If InsertCount > 0 Then
                Try
                    con.ConnectionString = StandardFunction.connectionString
                    con.Open()
                    command = New SqlCommand(InsertStr.ToString.Substring(0, InsertStr.Length - 1), con)
                    command.ExecuteNonQuery()
                Catch ex As Exception
                    TxtTest.Text += ex.Message
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('Insert ผิด'" & ex.Message & ");", True)
                Finally
                    con.Close()
                End Try
            End If
        End If
    End Sub

    Protected Sub BtnSelect_Click(sender As Object, e As EventArgs) Handles BtnSelect.Click
        StandardFunction.getSheetExcel(Me, FileUpload, DrpSheet)
    End Sub

    Protected Sub BtnDownloadAll_Click(sender As Object, e As EventArgs) Handles BtnDownloadAll.Click

        Dim SqlDownload = "
        SELECT [EmpNo]
              ,[Name]
              ,[Surname]
              ,[Status]
              ,[Shift]
              ,[Gender]
              ,[Mobile]
              ,[Company]
              ,[Section]
              ,[Process]
              ,[Position]
              ,[Start_date]
              ,[End_date]
          FROM [Manpower_Mecha2].[dbo].[Emp_Master]
          WHERE NOT [EmpNo] = 'ADMIN'
        "
        StandardFunction.ExportExcel(Me, SqlDownload, "AllEmployee Download")
    End Sub

    Protected Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click

        Dim Check = True
        Dim Alert As New StringBuilder("กรุณาระบุ\n")

        If TxtEmpNo.Text = Nothing Then
            Check = False
            Alert.Append("- รหัสพนักงาน (5 หลัก)\n")
        End If

        If TxtName.Text = Nothing Then
            Check = False
            Alert.Append("- ชื่อพนักงาน\n")
        End If

        If TxtSurName.Text = Nothing Then
            Check = False
            Alert.Append("- นามสกุลพนักงาน\n")
        End If

        If DrpGender.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- เพศพนักงาน\n")
        End If

        If DrpStatus.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- สถานะการทำงาน\n")
        End If

        If DrpSection.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- ส่วนงาน\n")
        End If

        If DrpProcess.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- ส่วนงาน\n")
        End If

        If DrpProcess.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- กระบวนการ\n")
        End If

        If DrpShift.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- กะการทำงาน\n")
        End If

        If DrpCompany.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- แผนก หรือ บริษัท\n")
        End If

        If DrpPosition.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- ตำแหน่ง หรือ หน้าที่\n")
        End If

        Dim ChkEmpNo = "
        SELECT 'False'
          FROM [Manpower_Mecha2].[dbo].[Emp_Master]
          WHERE [EmpNo] = '" & TxtEmpNo.Text & "'
        "
        Dim x = StandardFunction.getSQLDataString(ChkEmpNo)
        If Not String.IsNullOrEmpty(x) Then
            Check = False
            Alert.Append("- รหัสพนักงาน(ซ้ำ)\n")
        End If

        If Check = False Then
            'MsgBox("ผิด" & Alert.ToString)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert.ToString & "');", True)
            Exit Sub
        End If

        Dim SqlAddNew = "
               INSERT INTO [Manpower_Mecha2].[dbo].[Emp_Master]
              ([EmpNo]
              ,[Name]
              ,[Surname]
              ,[Status]
              ,[Shift]
              ,[Gender]
              ,[Company]
              ,[Section]
              ,[Process]
              ,[Position]
              ,[Start_date])
        VALUES
              ('" & TxtEmpNo.Text.ToUpper & "'
              ,'" & TxtName.Text & "'
              ,'" & TxtSurName.Text & "'
              ,'" & DrpStatus.SelectedValue & "'
              ,'" & DrpShift.SelectedValue & "'
              ,'" & DrpGender.SelectedValue & "'
              ,'" & DrpCompany.SelectedValue & "'
              ,'" & DrpSection.SelectedValue & "'
              ,'" & DrpProcess.SelectedValue.Replace("'", "''") & "'
              ,'" & DrpPosition.SelectedValue & "'
              ,CONVERT(varchar(10),GETDATE(),23))
           "

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlAddNew, con)
            command.ExecuteNonQuery()
        Catch ex As Exception
            TxtTest.Text += ex.Message
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & ex.Message & ");", True)
        Finally
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('เพิ่มรายชื่อสำเร็จ (Added)');", True)
            con.Close()
        End Try
    End Sub
End Class