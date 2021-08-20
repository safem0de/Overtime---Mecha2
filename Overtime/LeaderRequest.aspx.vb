Imports System.Data.SqlClient
Imports System.Globalization

Public Class LeaderRequest
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
            LoadPage()
            TxtWorkDate.AutoPostBack = True
            DrpProcess.AutoPostBack = True
            DrpRequestfor.AutoPostBack = True
            DrpPreset.AutoPostBack = True
            CheckBoxPreset.AutoPostBack = True
            ChkboxDel.AutoPostBack = True
        End If
    End Sub
    Sub BindColumn(GV As GridView, dt As DataTable, Optional SsName As String = Nothing)
        If Not SsName = Nothing Then
            Session(SsName) = dt
        End If
        GV.DataSource = dt
        GV.DataBind()
    End Sub

    Function AddColumnConfirm() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("Date")
        dt.Columns.Add("EmpNo")
        dt.Columns.Add("Name")
        dt.Columns.Add("Surname")
        dt.Columns.Add("Shift")
        dt.Columns.Add("Hours")
        dt.Columns.Add("Accum Hours")
        dt.Columns.Add("Reason")
        dt.Rows.Add()
        Return dt
    End Function

    Function AddColumnSelect() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("EmpNo")
        dt.Columns.Add("Name")
        dt.Columns.Add("Surname")
        dt.Columns.Add("Shift")
        dt.Columns.Add("Accum Hours")
        dt.Rows.Add()
        Return dt
    End Function

    Function AddColumn() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("Date")
        dt.Columns.Add("EmpNo")
        dt.Columns.Add("Name")
        dt.Columns.Add("Surname")
        dt.Columns.Add("Shift")
        dt.Columns.Add("Section")
        dt.Columns.Add("Process")
        dt.Columns.Add("Hours")
        dt.Columns.Add("Accum Hours")
        dt.Columns.Add("Reason")
        dt.Rows.Add()
        Return dt
    End Function

    Function AddColumns(ArrCol As ArrayList) As DataTable
        Dim dt As New DataTable
        For Each x In ArrCol
            dt.Columns.Add(x)
        Next
        dt.Rows.Add()
        Return dt
    End Function


    Sub LoadPage()
        Dim GetWeek = "Select DATEPART(WEEK,GETDATE()) As [W]"
        Dim W = StandardFunction.getSQLDataString(GetWeek)
        If CInt(W) Mod 2 = 1 Then
            LblMNStatus.Text = "MN1 = N, MN2 = M"
        Else
            LblMNStatus.Text = "MN1 = M, MN2 = N"
        End If

        Dim SqlProcess = "SELECT [Process_Name] FROM [Manpower_Mecha2].[dbo].[Process_Master]"
        StandardFunction.setDropdownlist(DrpProcess, SqlProcess, "")
        StandardFunction.setDropdownlist(DrpRequestfor, SqlProcess, "")

        Dim SqlShift = "SELECT * FROM [Manpower_Mecha2].[dbo].[Shift_Master]"
        StandardFunction.setDropdownlist(DrpShift, SqlShift, "")

        Dim Preset = "
            SELECT [Preset]
            FROM [Manpower_Mecha2].[dbo].[Request_OT]
            WHERE Not [Preset] = ''
            AND [Request_By] = '" & Session("User") & "'"
        StandardFunction.setDropdownlist(DrpPreset, Preset, "")
        Dim dt = StandardFunction.GetDataTable(Preset)

        If dt.Rows.Count = 0 Or ChkboxDel.Checked = False Then
            BtnDelPreset.Enabled = False
        End If

        BindColumn(GrdEmpSelect, AddColumnSelect, "EmpSelect")
        BindColumn(GrdEmpConfirm, AddColumnConfirm, "EmpConfirm")
        Session("Storage") = AddColumn()
    End Sub

    Private Sub DrpProcess_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DrpProcess.SelectedIndexChanged

        Dim Alert As New StringBuilder("Pls. Select")
        Dim check As Boolean = True

        Dim OTDate
        Dim OTDateValue
        Dim NowDate
        Dim NowDateValue

        If TxtWorkDate.Text = "" Then
            Alert.Append(" วันที่,")
            check = False
        Else
            OTDate = CDate(TxtWorkDate.Text)
            OTDateValue = OTDate.ToOADate()
            NowDate = CDate(DateTime.Now.ToShortDateString)
            NowDateValue = NowDate.ToOADate()
            If Session("Position") = "LEADER" Then 'Clerk and Staff key ย้อนหลังได้
                Dim x = DateTime.FromOADate(OTDateValue).ToString("yyyy")
                If (OTDateValue < NowDateValue) And (Not x = 2000) Then
                    Alert.Append(" วันที่ย้อนหลัง,")
                    check = False
                End If
                'MsgBox(DateTime.FromOADate(OTDateValue).ToString("yyyy"))
                'MsgBox(NowDateValue)
            End If
        End If

        If DrpProcess.SelectedIndex = 0 Then
            Alert.Append(" กระบวนการ,")
            LoadPage()
            check = False
        End If

        If check = False Then
            Clearform()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert.ToString.Substring(0, Alert.Length - 1) & "');", True)
            Exit Sub
        End If

        Dim SqlGrid As String = "
            DECLARE @D Date, @Pr Varchar(50), @Emp varchar(5);
            SET @D = '" & TxtWorkDate.Text & "';
            SET @Pr = '" & DrpProcess.SelectedValue.Replace("'", "''") & "';

            Set datefirst 1;

            With TableA as
            (
            SELECT	[EmpNo]
		            ,[Name]
		            ,[Surname]
			        ,[Shift]
			        ,[Section]
			        ,[Process]
		            FROM [Manpower_Mecha2].[dbo].[Emp_Master]
		            WHERE [Process] = @Pr
                    AND [Status] = 'ACTIVE'
                    AND NOT [Name] = 'ADMIN'
            ),
            TableB as
            (
            SELECT [EmpNo]
	                ,DATEPART(Week, [Date]) AS [Week]
                    ,SUM([Hours]) as [Accum]
            FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
            WHERE DATEPART(Week, [Date]) = DATEPART(Week, @D)
	        AND [Date] = Convert(Date , @D)
            GROUP BY  [EmpNo]
                    ,[Name]
                    ,[Surname]
	                ,DATEPART(Week, [Date])
            ),
	        TableC as
            (
            SELECT [EmpNo]
	                ,DATEPART(Week, [Date]) AS [Week]
                    ,SUM([Hours]) as [Accum]
            FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
            WHERE DATEPART(Week, [Date]) = DATEPART(Week, @D)
	        AND [Date] < Convert(Date , @D)
            GROUP BY  [EmpNo]
                    ,[Name]
                    ,[Surname]
	                ,DATEPART(Week, [Date])
            ),
	        TableD as
            (
            SELECT [EmpNo]
	                ,DATEPART(Week, [Date]) AS [Week]
                    ,SUM([Hours]) as [Accum]
            FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
            WHERE DATEPART(Week, [Date]) = DATEPART(Week, @D)
	        AND [Date] > Convert(Date , @D)
            GROUP BY  [EmpNo]
                    ,[Name]
                    ,[Surname]
	                ,DATEPART(Week, [Date])
            )

            SELECT t1.[EmpNo]
                    ,[Name]
                    ,[Surname]
			        --,[Section]
			        ,[Shift]
	                --,ISNULL(t2.[Accum],0) as [Curr Accum Hours]
			        ,ISNULL(t3.[Accum],0) as [Accum Hours]
			        --,ISNULL(t4.[Accum],0) as [Next Accum Hours]
            FROM TableA as t1
            LEFT JOIN tableB as t2
            ON t1.[EmpNo] = t2.[EmpNo]
	        LEFT JOIN tableC as t3
            ON t1.[EmpNo] = t3.[EmpNo]
	        LEFT JOIN tableD as t4
            ON t1.[EmpNo] = t4.[EmpNo];
        "
        TextBoxTest.Text += SqlGrid
        Dim dts As DataTable = StandardFunction.GetDataTable(SqlGrid)
        Session("EmpSelect") = dts

        If dts.Rows.Count > 0 Then
            StandardFunction.fillDataTableToDataGrid(GrdEmpSelect, SqlGrid, "")
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('No Data');", True)
        End If
    End Sub

    Private Sub GrdEmpSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdEmpSelect.SelectedIndexChanged
        Dim row As GridViewRow = GrdEmpSelect.SelectedRow
        Dim SqlSection As String = "
        SELECT [Section_Name]
        FROM [Manpower_Mecha2].[dbo].[Process_Master] as t1
        Left Join [Manpower_Mecha2].[dbo].[Section_Master] as t2
        on t1.[Section_Id] = t2.[Section_Id]
        WHERE [Process_Name] = '" & DrpProcess.Text & "'
        "

        Dim Section As String = StandardFunction.getSQLDataString(SqlSection)

        Dim Alert As String = "Pls. Select"
        Dim check As Boolean = True

        Dim OTDate
        Dim OTDateValue
        Dim NowDate
        Dim NowDateValue

        Dim Sqlchk = "
        DECLARE @D Date, @Emp Varchar(5);
        SET @D = '" & TxtWorkDate.Text & "';
        SET @Emp = '" & row.Cells(1).Text & "';

        SELECT Convert(Date,[Date]) As [Date]
                          ,t1.[EmpNo]
                          ,[Name]
                          ,[Surname]
                          ,[Shift]
                          ,[Hours]
                          ,[Status]
        FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1
        Where t1.[EmpNo] = @Emp
        And Convert(Date,[Date]) = @D
        "

        Dim dtChk = StandardFunction.GetDataTable(Sqlchk)

        If TxtWorkDate.Text = "" Then
            Alert += " วันที่,"
            check = False
        Else
            OTDate = CDate(TxtWorkDate.Text)
            OTDateValue = OTDate.ToOADate()
            NowDate = CDate(DateTime.Now.ToShortDateString)
            NowDateValue = NowDate.ToOADate()
            If Session("Position") = "LEADER" Then
                Dim x = DateTime.FromOADate(OTDateValue).ToString("yyyy")
                If (OTDateValue < NowDateValue) And (Not x = 2000) Then
                    Alert += " วันที่ย้อนหลัง,"
                    check = False
                End If
            End If
        End If

        If DrpHours.SelectedIndex = 0 Then
            Alert += " เลือกจำนวนชั่วโมง,"
            check = False
        End If

        If dtChk.Rows.Count > 0 Then
            Alert += " รีเควสซ้ำ : วันที่ " & TxtWorkDate.Text & ","
            check = False
        End If

        Dim Hr As Double
        Dim Acc_Hr As Double

        Sqlchk = "
        DECLARE @D Date, @Emp Varchar(5);
        SET @D = '" & TxtWorkDate.Text & "';
        SET @Emp = '" & row.Cells(1).Text & "';

        Set datefirst 1;
        
                SELECT	[EmpNo]
		        ,SUM([Hours]) as [Accum_Hours]
        FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
        WHERE DATEPART(Week,[Date]) = DATEPART(Week,@D)
		AND NOT [Date] = @D
        AND [EmpNo] = @Emp
        GROUP BY [EmpNo]
        "

        dtChk = StandardFunction.GetDataTable(Sqlchk)

        For Each i As DataRow In dtChk.Rows
            If Not DrpHours.SelectedIndex = 0 Then
                Hr = DrpHours.SelectedValue
                Acc_Hr = CDbl(i.Item("Accum_Hours"))
            End If

            If Session("Position") = "LEADER" Then
                If Hr + Acc_Hr > 36 Then
                    Alert += " จำนวนชั่วโมง/สัปดาห์ เกิน 36 ชั่วโมง,"
                    check = False
                End If
            End If

        Next

        If DrpShift.SelectedIndex = 0 Then
            Alert += " กรุณาเลือกกะ,"
            check = False
        Else
            If DrpShift.SelectedValue = "CBA" Then
                DrpShift.SelectedIndex = 0
                Alert += " กรุณาเลือกกะ AAAA, BBBB หรือ CCCC,"
                check = False
            End If
        End If

        If DrpReason.Text = "" Then
            Alert += " กรุณาเลือกเหตุผล,"
            check = False
        End If

        Alert = Alert.Substring(0, Alert.Length - 1)

        If check = False Then
            GrdEmpSelect.SelectedIndex = -1
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert & "');", True)
            Exit Sub
        End If

        If Not (row.Cells(1).Text.Replace("&nbsp;", "")) = "" Then
            Dim sendText As New ArrayList
            sendText.Add(TxtWorkDate.Text)
            sendText.Add(row.Cells(1).Text)
            sendText.Add(row.Cells(2).Text)
            sendText.Add(row.Cells(3).Text)
            sendText.Add(DrpShift.SelectedValue)
            sendText.Add(Section)
            sendText.Add(DrpProcess.SelectedValue)
            sendText.Add(DrpHours.SelectedValue)
            sendText.Add(CDbl(row.Cells(5).Text))
            sendText.Add(DrpReason.Text)

            Session("Add") = sendText
        Else
            GrdEmpSelect.SelectedIndex = -1
            Exit Sub
        End If

        Dim txt As New StringBuilder()
        txt.Append("<h5 style='color:blue'>" & row.Cells(2).Text & " " & row.Cells(3).Text & "</h5>")
        txt.Append("&emsp;รหัสพนักงาน : <b>" & row.Cells(1).Text & "</b> กะ <b>" & DrpShift.SelectedValue & "</b>")
        txt.Append("<br />")
        txt.Append("&emsp;กระบวนการ : <b>" & DrpProcess.SelectedValue & "</b>")
        txt.Append("<br />")
        txt.Append("&emsp;วันที่รีเควสโอ-ที : <b>" & TxtWorkDate.Text & "</b> เวลา <b>" & DrpHours.SelectedValue & "</b> ชั่วโมง")
        txt.Append("<br />")
        txt.Append("&emsp;จำนวนชั่วโมงโอ-ที(สะสม) : <b>" & CDbl(Acc_Hr) & "</b> รวม <b>" & CDbl(Hr + Acc_Hr) & "</b>")
        txt.Append("<br />")
        txt.Append("&emsp;เหตุผล : <b>เพื่อ" & DrpReason.SelectedValue & "</b>")
        Label.Text = txt.ToString()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "$('#exampleModal').modal();", True)

        GrdEmpSelect.SelectedIndex = -1
    End Sub

    Sub Clearform()
        TxtWorkDate.Text = Nothing
        DrpProcess.SelectedIndex = 0
        DrpPreset.SelectedIndex = 0
        DrpReason.SelectedIndex = 0
        DrpHours.SelectedIndex = 0
        DrpShift.SelectedIndex = 0
        TxtRemark.Text = Nothing
        TxtPreset.Text = Nothing
        TxtPreset.Enabled = False
        CheckBoxPreset.Checked = False

        BindColumn(GrdEmpSelect, AddColumnSelect, "EmpSelect")
        BindColumn(GrdEmpConfirm, AddColumnConfirm, "EmpConfirm")
        Session("Storage") = AddColumn()
    End Sub

    Protected Sub BtnDelPreset_Click(sender As Object, e As EventArgs) Handles BtnDelPreset.Click

        If DrpPreset.SelectedIndex = 0 Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('กรุณาระบุ Preset ที่ต้องการลบ');", True)
            Exit Sub
        End If

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim sqlDel = "UPDATE [Manpower_Mecha2].[dbo].[Request_OT]
                      SET [Preset] = ''
                      WHERE [Preset] = '" & DrpPreset.SelectedValue & "';"
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
        End Try

        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click

        Dim dtStorage As DataTable = Session("Storage")
        Dim dtEmployee As DataTable = Session("EmpSelect")
        Dim dtConfirm As DataTable = Session("EmpConfirm")
        Dim arr As ArrayList = Session("Add")

        For i As Integer = dtConfirm.Rows.Count - 1 To 0 Step -1
            If dtConfirm.Rows(i).Item("EmpNo").ToString = arr(1) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('รายชื่อพนักงานซ้ำ');", True)
                Exit Sub
            End If
        Next

        dtStorage.Rows.Add(arr(0), arr(1), arr(2), arr(3), arr(4), arr(5), arr(6), arr(7), arr(8), arr(9))
        dtConfirm.Rows.Add(arr(0), arr(1), arr(2), arr(3), arr(4), arr(7), arr(8), arr(9))

        For i As Integer = dtStorage.Rows.Count - 1 To 0 Step -1
            Dim row As DataRow = dtStorage.Rows(i)
            If row.Item(0) Is Nothing Then
                dtStorage.Rows.Remove(row)
            ElseIf row.Item(0).ToString = "" Then
                dtStorage.Rows.Remove(row)
            End If
        Next

        For i As Integer = dtConfirm.Rows.Count - 1 To 0 Step -1
            Dim row As DataRow = dtConfirm.Rows(i)
            If row.Item(0) Is Nothing Then
                dtConfirm.Rows.Remove(row)
            ElseIf row.Item(0).ToString = "" Then
                dtConfirm.Rows.Remove(row)
            End If
        Next

        'กดเลือกแล้ว ลบ จากตารางรายชื่อที่เลือก
        For i As Integer = dtEmployee.Rows.Count - 1 To 0 Step -1
            Dim row As DataRow = dtEmployee.Rows(i)
            If dtEmployee.Rows(i).Item("EmpNo").ToString = arr(1) Then
                dtEmployee.Rows.Remove(row)
            End If
        Next

        If dtEmployee.Rows.Count = 0 Then
            BindColumn(GrdEmpSelect, AddColumnSelect, "EmpSelect")
        Else
            BindColumn(GrdEmpSelect, dtEmployee, "EmpSelect")
        End If

        BindColumn(GrdEmpConfirm, dtConfirm, "EmpConfirm")
        BindColumn(GrdStorage, dtStorage, "Storage")
        GrdEmpSelect.SelectedIndex = -1
    End Sub

    Private Sub GrdEmpConfirm_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdEmpConfirm.SelectedIndexChanged

        Dim rowz = GrdEmpConfirm.SelectedRow
        Dim dtStorage As DataTable = Session("Storage")
        Dim dtEmployee As DataTable = Session("EmpSelect")
        Dim dtConfirm As DataTable = Session("EmpConfirm")

        For i As Integer = dtConfirm.Rows.Count - 1 To 0 Step -1
            Dim row As DataRow = dtConfirm.Rows(i)
            If dtConfirm.Rows(i).Item("EmpNo").ToString = rowz.Cells(2).Text Then
                dtConfirm.Rows.Remove(row)
            End If
        Next

        For i As Integer = dtStorage.Rows.Count - 1 To 0 Step -1
            Dim row As DataRow = dtStorage.Rows(i)
            If dtStorage.Rows(i).Item("EmpNo").ToString = rowz.Cells(2).Text Then
                If dtStorage.Rows(i).Item("Process") = DrpProcess.SelectedValue Then

                    Dim SqlGetShift = "SELECT [Shift]
                      FROM [Manpower_Mecha2].[dbo].[Emp_Master]
                      Where [EmpNo] = '" & dtStorage.Rows(i).Item("EmpNo").ToString & "'"
                    Dim GetShift = StandardFunction.getSQLDataString(SqlGetShift)

                    dtEmployee.Rows.Add(dtStorage.Rows(i).Item("EmpNo"),
                                        dtStorage.Rows(i).Item("Name"),
                                        dtStorage.Rows(i).Item("SurName"),
                                        GetShift,
                                        CDbl(dtStorage.Rows(i).Item("Accum Hours")))

                End If
                dtStorage.Rows.Remove(row)
            End If
        Next

        If dtConfirm.Rows.Count = 0 Then
            BindColumn(GrdEmpConfirm, AddColumnConfirm, "EmpConfirm")
        Else
            BindColumn(GrdEmpConfirm, dtConfirm, "EmpConfirm")
        End If

        BindColumn(GrdEmpSelect, dtEmployee, "EmpSelect")
        'BindColumn(GrdStorage, dtStorage, "Storage")
        GrdEmpConfirm.SelectedIndex = -1
    End Sub

    Protected Sub CheckBoxPreset_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxPreset.CheckedChanged
        If CheckBoxPreset.Checked = True Then
            TxtPreset.Enabled = True
        Else
            TxtPreset.Enabled = False
        End If
    End Sub

    Protected Sub BtnConfirm_Click(sender As Object, e As EventArgs) Handles BtnConfirm.Click

        Dim StrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim Alert As New StringBuilder("Pls. Select")
        Dim check As Boolean = True
        Dim dt As DataTable = Session("Storage")

        For i As Integer = dt.Rows.Count - 1 To 0 Step -1
            Dim row As DataRow = dt.Rows(i)
            If row.Item(0) Is Nothing Or row.Item(0).ToString = "" Then
                Alert.Append(" กรุณาเลือกรายชื่อ,")
                check = False
            End If
        Next

        If CheckBoxPreset.Checked = True And TxtPreset.Text.Trim = Nothing Then
            Alert.Append(" กรุณาระบุ ชื่อ Preset,")
            check = False

        Else

            Dim SqlCheckPreset = "
                SELECT Top(1) 'False' as [Preset]
                  FROM [Manpower_Mecha2].[dbo].[Request_OT]
                  WHERE [Preset] = '" & TxtPreset.Text & "'
                  AND NOT [Preset] = ''
                "
            'TxtRemark.Text += SqlCheckPreset
            Dim SqlPreset = StandardFunction.getSQLDataString(SqlCheckPreset)
            If Not CBool(String.IsNullOrEmpty(SqlPreset)) Then
                Alert.Append(" ชื่อ Preset(ซ้ำ) ถูกใช้งานแล้ว,")
                check = False
            End If

        End If

        If DrpRequestfor.SelectedIndex = 0 Then
            check = False
            Alert.Append(" เลือก Process ที่รีเควส")
        End If

        If check = False Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert.ToString.Substring(0, Alert.Length - 1) & "');", True)
            Exit Sub
        End If

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim SqlRequest = "
        INSERT INTO [Manpower_Mecha2].[dbo].[Request_OT]
           ([Date_Req]
           ,[Request_By]
           ,[Reason]
           ,[Preset]
           ,[Process])
            VALUES
           (Convert(datetime,'" & StrDate & "')
           ,'" & Session("User") & "'
           ,'" & TxtRemark.Text.Replace("'", "") & "'
           ,'" & TxtPreset.Text.Replace("'", "") & "'
           ,'" & DrpRequestfor.SelectedValue.Replace("'", "''") & "')"

        Try
            con.ConnectionString = StandardFunction.connectionString 'คำสั่งเชื่อม SQL จาก IP ไหน,Password อะไร'
            con.Open()
            command = New SqlCommand(SqlRequest, con)
            command.ExecuteNonQuery() 'คำสั่งเปิดใช้งาน การเชื่อมต่อ Sql'

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ผิด');", True)
        Finally
            con.Close()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('OK');", True)
        End Try

        Dim SqlUpdate As String = "SELECT Top(1) [Request_Id]
                     FROM [Manpower_Mecha2].[dbo].[Request_OT]
                     WHERE [Request_By] = '" & Session("User") & "'
                   ORDER BY [Date_Req] DESC"

        Dim x = StandardFunction.getSQLDataString(SqlUpdate)

        Dim StrbsqlVal As New StringBuilder("INSERT INTO [Manpower_Mecha2].[dbo].[Request_OT_Detail]
           ([Req_Id]
           ,[Date]
           ,[EmpNo]
           ,[Name]
           ,[Surname]
           ,[Shift]
           ,[Section]
           ,[Process]
           ,[Hours]
           ,[Accum_Hours]
           ,[Status]
           ,[Reason])
            VALUES")

        For i = 0 To dt.Rows.Count - 1
            StrbsqlVal.Append("(" & x & ",
                    '" & dt.Rows(i).Item("Date").ToString & "',
                    '" & dt.Rows(i).Item("EmpNo").ToString & "',
                    '" & dt.Rows(i).Item("Name").ToString & "',
                    '" & dt.Rows(i).Item("Surname").ToString & "',
                    '" & dt.Rows(i).Item("Shift").ToString & "',
                    '" & dt.Rows(i).Item("Section").ToString.Replace("'", "''") & "',
                    '" & dt.Rows(i).Item("Process").ToString.Replace("'", "''") & "',
                    " & dt.Rows(i).Item("Hours").ToString & ",
                    " & dt.Rows(i).Item("Accum Hours").ToString & ",
                    'Confirmed',
                    '" & dt.Rows(i).Item("Reason").ToString & "'),")
        Next
        Dim StrbConvert = StrbsqlVal.ToString
        StrbConvert = StrbConvert.Substring(0, StrbConvert.Length - 1)

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(StrbConvert, con)
            command.ExecuteNonQuery()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ผิด');", True)
        Finally
            con.Close()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('OK');", True)
        End Try

        For i = 0 To dt.Rows.Count - 1
            'MsgBox(dt.Rows(i).Item("EmpNo").ToString)
            Dim SqlSelectNext = "
            DECLARE @D Date, @Emp Varchar(5);
            SET @D = '" & dt.Rows(i).Item("Date").ToString & "';
            SET @Emp = '" & dt.Rows(i).Item("EmpNo").ToString & "';

            Set datefirst 1;

                SELECT [Req_detail_Id]
                      ,[Date]
                      ,[Hours]
                      ,[Accum_Hours]
                  FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                  Where [EmpNo] = @Emp
                  And DatePart(Week, [Date]) = DatePart(Week, @D)
                  And [Date] > @D
            "
            Dim dtx = StandardFunction.GetDataTable(SqlSelectNext)

            If dtx.Rows.Count > 0 Then
                For Each z As DataRow In dtx.Rows
                    Dim SqlUpdateNext = "
                UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                   SET [Accum_Hours] = " & CDbl(z.Item("Accum_Hours")) + CDbl(z.Item("Hours")) & "
                 WHERE [Req_detail_Id] = " & z.Item("Req_detail_Id") & "
                "
                    Try
                        ' พยายามลองทำคำสั่งในนี้
                        con.ConnectionString = StandardFunction.connectionString
                        con.Open()
                        command = New SqlCommand(SqlUpdateNext, con)
                        command.ExecuteNonQuery()
                    Catch ex As Exception
                        ' Error/fail แล้วทำตรงนี้
                        Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลผิดพลาด');", True)
                        MsgBox(ex.Message)
                    Finally
                        'ไม่ว่าจะ Error หรือ OK ก็ต้องเข้า loop นี้
                        Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                        con.Close()
                    End Try
                Next
            End If
        Next

        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        Clearform()
    End Sub

    Protected Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        Clearform()
    End Sub

    Private Sub DrpPreset_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DrpPreset.SelectedIndexChanged

        Dim Alert As New StringBuilder("Pls. Select")
        Dim check As Boolean = True

        Dim dts As New DataTable

        Dim OTDate As Date
        Dim OTDateValue
        Dim NowDate As Date
        Dim NowDateValue

        If TxtWorkDate.Text = "" Then
            Alert.Append(" วันที่,")
            check = False
        Else
            OTDate = CDate(TxtWorkDate.Text)
            OTDateValue = OTDate.ToOADate()
            NowDate = CDate(DateTime.Now.ToShortDateString)
            NowDateValue = NowDate.ToOADate()
            'ไม่ให้เอา preset เดิมมาใช้
            If OTDateValue < NowDateValue Then
                Alert.Append(" วันที่ย้อนหลัง,")
                check = False
            End If
        End If

        If DrpShift.SelectedIndex = 0 Then

            If DrpShift.SelectedValue = "MN" Then
                DrpShift.SelectedIndex = 0
                Alert.Append(" กรุณาเลือกกะ MMMM หรือ NNNN,")
                check = False
            ElseIf DrpShift.SelectedValue = "CBA" Then
                DrpShift.SelectedIndex = 0
                Alert.Append(" กรุณาเลือกกะ AAAA, BBBB หรือ CCCC,")
                check = False
            End If
        End If

        If DrpPreset.SelectedIndex = 0 Then
            Alert.Append(" Preset,")
            LoadPage()
            check = False
        End If

        If DrpHours.SelectedIndex = 0 Then
            Alert.Append(" เลือกจำนวนชั่วโมง,")
            check = False
        End If

        If check = False Then
            If ChkboxDel.Checked Then
                DrpPreset.AutoPostBack = False
                Exit Sub
            Else
                Clearform()
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert.ToString.Substring(0, Alert.Length - 1) & "');", True)
                Exit Sub
            End If
        End If

        Dim SqlGetReq_Id As String = "
            SELECT Top(1) [Request_Id]
              FROM [Manpower_Mecha2].[dbo].[Request_OT]
              WHERE [Preset] = '" & DrpPreset.SelectedValue & "'
              AND [Request_By] = '" & Session("User") & "'
              Order by [Date_Req] desc
        "
        Dim Req_Id = StandardFunction.getSQLDataString(SqlGetReq_Id)

        If Not IsNothing(Req_Id) Then
            Dim SqlGrid As String = "
                SELECT Convert(varchar(10),[Date],23) as [Date]
                      ,[EmpNo]
                      ,[Name]
                      ,[Surname]
                      ,[Shift]
                      ,[Section]
                      ,[Process]
                      ,[Hours]
                      ,[Accum_Hours] as [Accum Hours]
                      --,[Status]ไม่ต้องใส่เพราะในปุ่ม Confirm จะใส่ให้อีกครั้ง
                      ,[Reason]
                  FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                  Where [Req_Id] = " & Req_Id & "
            "

            dts = StandardFunction.GetDataTable(SqlGrid)

            If dts.Rows.Count > 0 Then
                For Each x As DataRow In dts.Rows
                    Dim xSql = "
                       DECLARE @D Date, @Pr Varchar(50), @Emp varchar(5);
                       SET @D = '" & TxtWorkDate.Text & "';
                       SET @Emp = '" & x.Item("EmpNo") & "';

                       Set datefirst 1;

                    With TableA as
                    (
                    SELECT	[EmpNo]
                      ,[Name]
                      ,[Surname]
                      ,[Shift]
                      ,[Section]
                      ,[Process]
                      FROM [Manpower_Mecha2].[dbo].[Emp_Master]
                      WHERE NOT [Name] = 'ADMIN'
                      AND [Status] = 'ACTIVE'
                    ),
                    TableB as
                    (
                    SELECT [EmpNo]
                         ,DATEPART(Week, [Date]) AS [Week]
                            ,SUM([Hours]) as [Accum]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                    WHERE DATEPART(Week, [Date]) = DATEPART(Week, @D)
                    AND [Date] = Convert(Date , @D)
                    GROUP BY  [EmpNo]
                            ,[Name]
                            ,[Surname]
                         ,DATEPART(Week, [Date])
                    ),
                    TableC as
                    (
                    SELECT [EmpNo]
                         ,DATEPART(Week, [Date]) AS [Week]
                            ,SUM([Hours]) as [Accum]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                    WHERE DATEPART(Week, [Date]) = DATEPART(Week, @D)
                    AND [Date] < Convert(Date , @D)
                    GROUP BY  [EmpNo]
                            ,[Name]
                            ,[Surname]
                         ,DATEPART(Week, [Date])
                    ),
                    TableD as
                    (
                    SELECT [EmpNo]
                         ,DATEPART(Week, [Date]) AS [Week]
                            ,SUM([Hours]) as [Accum]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                    WHERE DATEPART(Week, [Date]) = DATEPART(Week, @D)
                    AND [Date] > Convert(Date , @D)
                    GROUP BY  [EmpNo]
                            ,[Name]
                            ,[Surname]
                         ,DATEPART(Week, [Date])
                    )

                    SELECT t1.[EmpNo]
                            --,[Name]
                            --,[Surname]
                      --,[Section]
                      --,[Shift]
                         ,ISNULL(t2.[Accum],0) as [Curr Accum Hours]
                      ,ISNULL(t3.[Accum],0) as [Prev Accum Hours]
                      ,ISNULL(t4.[Accum],0) as [Next Accum Hours]
                    FROM TableA as t1
                    LEFT JOIN tableB as t2
                    ON t1.[EmpNo] = t2.[EmpNo]
                    LEFT JOIN tableC as t3
                    ON t1.[EmpNo] = t3.[EmpNo]
                    LEFT JOIN tableD as t4
                    ON t1.[EmpNo] = t4.[EmpNo]
                    WHERE t1.[EmpNo] = @Emp;
                    "

                    Dim dty = StandardFunction.GetDataTable(xSql)

                    For i As Integer = dty.Rows.Count - 1 To 0 Step -1
                        Dim row As DataRow = dty.Rows(i)
                        If dty.Rows(i).Item("Curr Accum Hours").ToString > 0 Then
                            x.Delete()
                        Else
                            If dty.Rows(i).Item("Prev Accum Hours").ToString > 0 Then
                                x.Item("Accum Hours") = dty.Rows(i).Item("Prev Accum Hours")
                                x.Item("Date") = CDate(OTDate).ToString("yyyy-MM-dd")
                                x.Item("Shift") = DrpShift.SelectedValue
                                x.Item("Hours") = DrpHours.SelectedValue
                                x.Item("Reason") = DrpReason.SelectedValue
                            Else
                                x.Item("Date") = CDate(OTDate).ToString("yyyy-MM-dd")
                                x.Item("Shift") = DrpShift.SelectedValue
                                x.Item("Hours") = DrpHours.SelectedValue
                                x.Item("Reason") = DrpReason.SelectedValue
                            End If
                        End If
                    Next
                Next
                dts.AcceptChanges()
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('No Data');", True)
            End If
        End If

        Dim dtt = dts.Copy
        dtt.Columns.Remove("Section")
        dtt.Columns.Remove("Process")
        'dtt.Columns.Remove("Status")

        If dtt.Rows.Count = 0 Then
            BindColumn(GrdEmpConfirm, AddColumnConfirm, "EmpConfirm")
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('Request By Preset ซ้ำ\nหรือ รายชื่อทั้งหมดถูก รีเควสแล้ว');", True)
        Else
            BindColumn(GrdEmpConfirm, dtt, "EmpConfirm")
        End If

        BindColumn(GrdStorage, dts, "Storage")
    End Sub

    Private Sub TxtWorkDate_TextChanged(sender As Object, e As EventArgs) Handles TxtWorkDate.TextChanged
        'TxtWorkDate.Text = Nothing
        DrpProcess.SelectedIndex = 0
        DrpPreset.SelectedIndex = 0
        DrpReason.SelectedIndex = 0
        DrpHours.SelectedIndex = 0
        DrpShift.SelectedIndex = 0
        TxtRemark.Text = Nothing
        TxtPreset.Text = Nothing
        TxtPreset.Enabled = False
        CheckBoxPreset.Checked = False

        BindColumn(GrdEmpSelect, AddColumnSelect, "EmpSelect")
        BindColumn(GrdEmpConfirm, AddColumnConfirm, "EmpConfirm")
        Session("Storage") = AddColumn()
    End Sub

    Protected Sub ChkboxDel_CheckedChanged(sender As Object, e As EventArgs) Handles ChkboxDel.CheckedChanged
        If ChkboxDel.Checked Then
            BtnDelPreset.Enabled = True
        Else
            BtnDelPreset.Enabled = False
        End If
    End Sub
End Class