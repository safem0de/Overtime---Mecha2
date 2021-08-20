Imports System.Data.SqlClient

Public Class EmployeeActionRequest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadPage()
            TxtSearch.AutoPostBack = True
            RadioBtnShiftProcess.AutoPostBack = True
        End If
        Search()
    End Sub

    Sub BindColumn(GV As GridView, dt As DataTable, Optional SsName As String = Nothing)
        If Not SsName = Nothing Then
            Session(SsName) = dt
        End If
        GV.DataSource = dt
        GV.DataBind()
    End Sub

    Function AddColumns(ArrCol As ArrayList) As DataTable
        Dim dt As New DataTable
        For Each x In ArrCol
            dt.Columns.Add(x)
        Next
        dt.Rows.Add()
        Return dt
    End Function

    Sub LoadPage()
        Dim SqlShift = "SELECT * FROM [Manpower_Mecha2].[dbo].[Shift_Master]"
        StandardFunction.setDropdownlist(DrpShiftOld, SqlShift, "")
        StandardFunction.setDropdownlist(DrpShiftNew, SqlShift, "")
        Dim SqlProcess = "SELECT [Process_Name] FROM [Manpower_Mecha2].[dbo].[Process_Master]"
        StandardFunction.setDropdownlist(DrpProcessOld, SqlProcess, "")
        StandardFunction.setDropdownlist(DrpProcessNew, SqlProcess, "")

        If IsNothing(Session("EARList")) Then
            Dim arr As New ArrayList
            arr.Add("No.")
            arr.Add("Action Date")
            arr.Add("EmpNo")
            arr.Add("Name")
            arr.Add("Surname")
            arr.Add("Shift")
            arr.Add("Process")
            BindColumn(GrdAllChange, AddColumns(arr), "EARList")
        End If

        CheckRdBtn()
    End Sub
    Sub Clearform()
        TxtSearch.Text = Nothing
        GrdSearch.DataSource = Nothing
        GrdSearch.DataBind()
    End Sub
    Sub Search()
        Dim SqlSearch = Nothing
        If TxtSearch.Text = Nothing Then
        Else
            SqlSearch = "
                DECLARE @Search Varchar(50);
                SET @Search = '" & TxtSearch.Text.Replace("'", "''").ToUpper & "%';

                SELECT	[EmpNo]
                        ,[Name]
                        ,[Surname]
                        ,[Shift]
                        ,[Process]
                    FROM [Manpower_Mecha2].[dbo].[Emp_Master]
                WHERE(
	                [EmpNo] like @Search
                OR	
	                [Name] like @Search
                OR	
	                [Surname] like @Search
                OR	
	                [Process] like @Search
                )
                AND
                    [Status] = 'ACTIVE'
                ORDER BY
	                [EmpNo] asc
            "
            StandardFunction.fillDataTableToDataGrid(GrdSearch, SqlSearch)
        End If
    End Sub

    Private Sub RadioBtnShiftProcess_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioBtnShiftProcess.SelectedIndexChanged
        CheckRdBtn()
    End Sub

    Sub CheckRdBtn()
        If RadioBtnShiftProcess.SelectedValue = 1 Then
            DrpShiftOld.Enabled = True
            DrpShiftNew.Enabled = True
            DrpProcessOld.Enabled = False
            DrpProcessNew.Enabled = False
        ElseIf RadioBtnShiftProcess.SelectedValue = 2 Then
            DrpShiftOld.Enabled = False
            DrpShiftNew.Enabled = False
            DrpProcessOld.Enabled = True
            DrpProcessNew.Enabled = True
        ElseIf RadioBtnShiftProcess.SelectedValue = 3 Then
            DrpShiftOld.Enabled = True
            DrpShiftNew.Enabled = True
            DrpProcessOld.Enabled = True
            DrpProcessNew.Enabled = True
        End If
        DrpShiftOld.SelectedIndex = 0
        DrpShiftNew.SelectedIndex = 0
        DrpProcessOld.SelectedIndex = 0
        DrpProcessNew.SelectedIndex = 0
    End Sub

    Private Sub GrdSearch_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GrdSearch.PageIndexChanging
        GrdSearch.PageIndex = e.NewPageIndex
        GrdSearch.DataBind()
    End Sub

    Private Sub GrdSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdSearch.SelectedIndexChanged
        Dim Check As Boolean = True
        Dim Alert As New StringBuilder("กรุณาระบุ\n")
        Dim row As GridViewRow = GrdSearch.SelectedRow

        On Error Resume Next
        DrpShiftOld.SelectedValue = row.Cells(4).Text

        If RadioBtnShiftProcess.SelectedValue = 1 Then
            If DrpShiftOld.SelectedValue.Equals(DrpShiftNew.SelectedValue) And Not DrpShiftOld.SelectedIndex = 0 Then
                Check = False
                Alert.Append("- กะการทำงาน ซ้ำ\n")
            ElseIf DrpShiftOld.SelectedIndex = 0 Or DrpShiftNew.SelectedIndex = 0 Then
                Check = False
                Alert.Append("- เลือกกะการทำงาน\n")
            End If
        ElseIf RadioBtnShiftProcess.SelectedValue = 2 Then
            If DrpProcessOld.SelectedValue.Equals(DrpProcessNew.SelectedValue) And Not DrpProcessOld.SelectedIndex = 0 Then
                Check = False
                Alert.Append("- กระบวนการ ซ้ำ\n")
            ElseIf DrpProcessOld.SelectedIndex = 0 Or DrpProcessNew.SelectedIndex = 0 Then
                Check = False
                Alert.Append("- เลือกกระบวนการ\n")
            End If
        ElseIf RadioBtnShiftProcess.SelectedValue = 3 Then
            If DrpShiftOld.SelectedValue.Equals(DrpShiftNew.SelectedValue) And Not DrpShiftOld.SelectedIndex = 0 Then
                Check = False
                Alert.Append("- กะการทำงาน ซ้ำ\n")
            ElseIf DrpShiftOld.SelectedIndex = 0 Or DrpShiftNew.SelectedIndex = 0 Then
                Check = False
                Alert.Append("- เลือกกะการทำงาน\n")
            End If

            If DrpProcessOld.SelectedValue.Equals(DrpProcessNew.SelectedValue) And Not DrpProcessOld.SelectedIndex = 0 Then
                Check = False
                Alert.Append("- กระบวนการ ซ้ำ\n")
            ElseIf DrpProcessOld.SelectedIndex = 0 Or DrpProcessNew.SelectedIndex = 0 Then
                Check = False
                Alert.Append("- เลือกกระบวนการ\n")
            End If
        End If

        If Check = False Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert.ToString & "');", True)
            Exit Sub
        Else
            Dim dt As DataTable = Session("EARList")
            For i As Integer = dt.Rows.Count - 1 To 0 Step -1
                If dt.Rows(i).Item("EmpNo").ToString = row.Cells(1).Text Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('รายชื่อพนักงานซ้ำ');", True)
                    Exit Sub
                End If
            Next

            Dim Shift_Val As String = Nothing
            Dim Process_Val As String = Nothing
            If RadioBtnShiftProcess.SelectedValue = 1 Then
                Process_Val = "-"
                Shift_Val = DrpShiftNew.SelectedValue
            ElseIf RadioBtnShiftProcess.SelectedValue = 2 Then
                Process_Val = DrpProcessNew.SelectedValue
                Shift_Val = "-"
            ElseIf RadioBtnShiftProcess.SelectedValue = 3 Then
                Shift_Val = DrpShiftNew.SelectedValue
                Process_Val = DrpProcessNew.SelectedValue
            End If

            For i As Integer = dt.Rows.Count - 1 To 0 Step -1
                Dim rowx As DataRow = dt.Rows(i)
                If rowx.Item(0) Is Nothing Then
                    dt.Rows.Remove(rowx)
                ElseIf rowx.Item(0).ToString = "" Then
                    dt.Rows.Remove(rowx)
                End If
            Next

            dt.Rows.Add(dt.Rows.Count + 1, TxtActionDate.Text, row.Cells(1).Text, row.Cells(2).Text, row.Cells(3).Text, Shift_Val, Process_Val)

            BindColumn(GrdAllChange, dt, "EARList")
        End If
    End Sub

    Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs) Handles TxtSearch.TextChanged
        If Len(TxtSearch.Text) = 0 Then
            Clearform()
        End If
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        Clearform()
    End Sub

    Protected Sub BtnConfirm_Click(sender As Object, e As EventArgs) Handles BtnConfirm.Click

        Dim Check As Boolean = True
        Dim Alert As New StringBuilder

        Dim dt As DataTable = Session("EARList")

        If Check = False Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert.ToString & "');", True)
            Exit Sub
        Else
            Dim StrDate = DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss")
            Dim con As New SqlConnection
            Dim command As SqlCommand

            Dim SqlEAR = "
                INSERT INTO [Manpower_Mecha2].[dbo].[Emp_Action]
                       ([Req_Date]
                       ,[Req_By])
                 VALUES
                       (Convert(DateTime, '" & StrDate & "', 21)
                       ,'" & Session("User") & "')
            "

            Try
                con.ConnectionString = StandardFunction.connectionString
                con.Open()
                command = New SqlCommand(SqlEAR, con)
                command.ExecuteNonQuery()
            Catch ex As Exception
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
            Finally
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                con.Close()
            End Try

            Dim SqlGetEARId = "
                SELECT
                    [Req_EAR_Id]
                  FROM [Manpower_Mecha2].[dbo].[Emp_Action]
                  WHERE
                  [Req_Date] = Convert(DateTime, '" & StrDate & "', 21)
                 AND
                  [Req_By] = '" & Session("User") & "'
            "
            Dim x = StandardFunction.getSQLDataString(SqlGetEARId)

            Dim SqlEARDetails As New StringBuilder("
                INSERT INTO [Manpower_Mecha2].[dbo].[Emp_Action_Detail]
                       ([Req_EAR_Id]
                       ,[Action_Date]
                       ,[EmpNo]
                       ,[Name]
                       ,[Surname]
                       ,[Shift]
                       ,[Process])
                 VALUES
            ")

            For Each i As DataRow In dt.Rows
                SqlEARDetails.Append("(")
                SqlEARDetails.Append(CInt(x))
                SqlEARDetails.Append(",'")
                SqlEARDetails.Append(i.Item("Action Date"))
                SqlEARDetails.Append("','")
                SqlEARDetails.Append(i.Item("EmpNo"))
                SqlEARDetails.Append("','")
                SqlEARDetails.Append(i.Item("Name"))
                SqlEARDetails.Append("','")
                SqlEARDetails.Append(i.Item("Surname"))
                SqlEARDetails.Append("','")
                SqlEARDetails.Append(i.Item("Shift"))
                SqlEARDetails.Append("','")
                SqlEARDetails.Append(i.Item("Process"))
                SqlEARDetails.Append("'),")
            Next

            TxtTest.Text = SqlEARDetails.ToString.Substring(0, SqlEARDetails.ToString.Length - 1)

            Try
                con.ConnectionString = StandardFunction.connectionString
                con.Open()
                command = New SqlCommand(SqlEARDetails.ToString.Substring(0, SqlEARDetails.ToString.Length - 1), con)
                command.ExecuteNonQuery()
            Catch ex As Exception
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
            Finally
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                con.Close()
            End Try
        End If

        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        LoadPage()
    End Sub

    Private Sub GrdAllChange_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GrdAllChange.RowDeleting
        Dim deldt As DataTable = Session("EARList")

        If e.RowIndex >= 0 And GrdAllChange.Rows.Count <> 1 Then
            deldt.Rows.RemoveAt(e.RowIndex)

            For i = e.RowIndex To deldt.Rows.Count - 1
                deldt.Rows(i).Item("No.") = CInt(deldt.Rows(i).Item("No.")) - 1
            Next

            BindColumn(GrdAllChange, deldt, "EARList")
        Else
            Dim arr As New ArrayList
            arr.Add("No.")
            arr.Add("Action Date")
            arr.Add("EmpNo")
            arr.Add("Name")
            arr.Add("Surname")
            arr.Add("Shift")
            arr.Add("Process")
            BindColumn(GrdAllChange, AddColumns(arr), "EARList")
        End If

    End Sub
End Class