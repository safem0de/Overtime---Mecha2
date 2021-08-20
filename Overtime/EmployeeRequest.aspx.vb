Imports System.Data.SqlClient

Public Class EmployeeRequest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("CanAccess") Then
            Response.Redirect("~/Login.aspx")
        End If

        If Not IsPostBack Then
            LoadPage()
        End If
        'DrpShift.AutoPostBack = False
        'DrpHours.AutoPostBack = False
        'DrpProcess.AutoPostBack = False
    End Sub

    Sub LoadPage()
        TxtEmpNo.Enabled = False
        TxtEmpNo.Text = Session("User")

        Dim SqlProcess = "SELECT [Process_Name] FROM [Manpower_Mecha2].[dbo].[Process_Master]"
        StandardFunction.setDropdownlist(DrpProcess, SqlProcess, "")

        Dim SqlShift = "SELECT * FROM [Manpower_Mecha2].[dbo].[Shift_Master]"
        StandardFunction.setDropdownlist(DrpShift, SqlShift, "")

        Dim SqlGridWaitConf = "
        SELECT Convert(varchar(10),[Date],23) as [WorkDate]
              ,[EmpNo]
              ,[Shift]
              ,[Section]
              ,[Process]
              ,[Hours]
              ,[Accum_Hours]
              ,[Status]
              ,[Reason]
          FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
          Where [EmpNo] = '" & Session("User") & "'
          And [Status] = 'Wait Confirm'
          --And [Date] >= GETDATE()
          Order by [Date] desc
          "
        StandardFunction.fillDataTableToDataGrid(GrdWaitConfirm, SqlGridWaitConf, "")

        Dim SqlGridConf = "
        SELECT Convert(varchar(10),[Date],23) as [WorkDate]
              ,[EmpNo]
              ,[Shift]
              ,[Section]
              ,[Process]
              ,[Hours]
              ,[Accum_Hours]
              ,[Status]
              ,[Reason]
          FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
          Where [EmpNo] = '" & Session("User") & "'
          And Not [Status] = 'Wait Confirm'
          Order by [Date] desc
          "
        StandardFunction.fillDataTableToDataGrid(GrdConfrimed, SqlGridConf, "")

        Dim SqlAuto As String
        If TxtEmpNo.Text = "" Then
            DrpProcess.SelectedIndex = 0
            DrpShift.SelectedIndex = 0
        ElseIf Len(TxtEmpNo.Text) >= 4 And Len(TxtEmpNo.Text) <= 5 Then
            SqlAuto = "
            SELECT [Name]
                  ,[Surname]
                  ,[Shift]
                  ,[Section]
                  ,[Process]
              FROM [Manpower_Mecha2].[dbo].[Emp_Master]
              Where [EmpNo] = '" & TxtEmpNo.Text & "'
            "
            Dim dt As DataTable = StandardFunction.GetDataTable(SqlAuto)
            If dt.Rows.Count = 1 Then
                For Each r As DataRow In dt.Rows
                    DrpProcess.Text = r.Item("Process")
                    DrpShift.Text = r.Item("Shift")
                Next
            Else
                Exit Sub
            End If
        End If
    End Sub

    Protected Sub BtnCancle_Click(sender As Object, e As EventArgs) Handles BtnCancle.Click
        Clearform()
    End Sub

    Sub Clearform()
        TxtEmpNo.Text = ""
        TxtWorkDate.Text = ""
        DrpShift.SelectedIndex = 0
        DrpHours.SelectedIndex = 0
        DrpProcess.SelectedIndex = 0
        DrpReason.SelectedIndex = 0
    End Sub
    Protected Sub BtnConfirm_Click(sender As Object, e As EventArgs) Handles BtnConfirm.Click

        Dim Check As Boolean = True
        Dim NeedToUpdate As Boolean = False

        Dim Alert As New StringBuilder("Pls. input ")
        Dim StrName As String = Nothing
        Dim StrSurName As String = Nothing
        Dim StrSection As String = Nothing
        Dim StrAccum As String = Nothing

        Dim SqlGetDetails = "
            DECLARE @D Date, @Pr Varchar(50), @Emp varchar(5);
            SET @D = '" & TxtWorkDate.Text & "';
            --SET @Pr = '" & DrpProcess.SelectedValue & "';
            SET @Emp = '" & TxtEmpNo.Text & "';

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
		            WHERE 
                    --[Process] = @Pr
                    --AND
                    NOT [Name] = 'ADMIN'
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
                    ,[Section]
			        ,[Shift]
	                ,ISNULL(t2.[Accum],0) as [Curr Accum Hours]
			        ,ISNULL(t3.[Accum],0) as [Prev Accum Hours]
			        ,ISNULL(t4.[Accum],0) as [Next Accum Hours]
            FROM TableA as t1
            LEFT JOIN TableB as t2
            ON t1.[EmpNo] = t2.[EmpNo]
	        LEFT JOIN TableC as t3
            ON t1.[EmpNo] = t3.[EmpNo]
	        LEFT JOIN TableD as t4
            ON t1.[EmpNo] = t4.[EmpNo]
	        Where t1.[EmpNo] = @Emp;
        "
        'TextTest.Text += SqlGetDetails
        'StandardFunction.fillDataTableToDataGrid(GrdTest, SqlGetDetails, "")

        Dim dt As DataTable = StandardFunction.GetDataTable(SqlGetDetails)
        If dt.Rows.Count > 0 And dt.Columns.Count > 1 Then
            For Each i As DataRow In dt.Rows

                If IsNothing(i.Item("Name")) Then
                    Check = False
                    Alert.Append("รหัสพนักงาน, ")
                End If

                If CDbl(i.Item("Curr Accum Hours").ToString) > 0 Then
                    Check = False
                    NeedToUpdate = False
                    Alert.Append("รีเควสซ้ำ : " & TxtWorkDate.Text & ", ")
                Else
                    NeedToUpdate = True
                End If

                If CDbl(i.Item("Next Accum Hours").ToString) = 0 Then
                    NeedToUpdate = False
                Else
                    NeedToUpdate = True
                End If

                StrName = i.Item("Name").ToString
                StrSurName = i.Item("Surname").ToString
                StrSection = i.Item("Section").ToString
                StrAccum = i.Item("Prev Accum Hours").ToString
            Next
        End If

        If DrpProcess.SelectedIndex = 0 Then
            Check = False
            Alert.Append("Process, ")
        End If

        If TxtWorkDate.Text = "" Then
            Check = False
            Alert.Append("Date, ")
        End If

        If DrpShift.SelectedIndex = 0 Then
            If DrpShift.SelectedValue = "MN" Then
                DrpShift.SelectedIndex = 0
                Alert.Append(" กรุณาเลือกกะ MMMM หรือ NNNN,")
                Check = False
            ElseIf DrpShift.SelectedValue = "CBA" Then
                DrpShift.SelectedIndex = 0
                Alert.Append(" กรุณาเลือกกะ AAAA, BBBB หรือ CCCC,")
                Check = False
            End If
        End If

        If DrpHours.SelectedIndex = 0 Then
            Check = False
            Alert.Append("Hours, ")
        End If

        If DrpReason.Text = "" Then
            Check = False
            Alert.Append("เหตุผล, ")
        End If

        If Check = False Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Window-script", "alert('" & Alert.ToString.Substring(0, Alert.Length - 2) & "');", True)
            Exit Sub
        End If

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim SqlAdd As String = "
        INSERT INTO [Manpower_Mecha2].[dbo].[Request_OT_Detail]
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
        VALUES
           (0
           ,'" & TxtWorkDate.Text & "'
           ,'" & TxtEmpNo.Text.ToUpper.Trim & "'
           ,'" & StrName & "'
           ,'" & StrSurName & "'
           ,'" & DrpShift.SelectedValue & "'
           ,'" & StrSection & "'
           ,'" & DrpProcess.SelectedValue & "'
           ," & DrpHours.SelectedValue & "
           ," & StrAccum & "
           ,'Wait Confirm'
           ,'" & DrpReason.SelectedValue & "')"

        'TextTest.Text += SqlAdd
        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlAdd, con)
            command.ExecuteNonQuery()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" + ex.Message + "');", True)
        Finally
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
            con.Close()
        End Try

        If NeedToUpdate Then
            Dim SqlSelectNext = "
            DECLARE @D Date, @Emp Varchar(5);
            SET @D = '" & CDate(TxtWorkDate.Text).ToString("yyyy-MM-dd") & "'
            SET @Emp = '" & Session("User") & "'

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
            'TextTest.Text += SqlSelectNext
            dt = StandardFunction.GetDataTable(SqlSelectNext)

            If dt.Rows.Count > 0 Then
                For Each x As DataRow In dt.Rows
                    Dim SqlUpdateNext = "
                    UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                       SET [Accum_Hours] = " & CDbl(x.Item("Accum_Hours")) + CDbl(DrpHours.SelectedValue) & "
                     WHERE [Req_detail_Id] = " & x.Item("Req_detail_Id") & "
                    "

                    Try
                        con.ConnectionString = StandardFunction.connectionString
                        con.Open()
                        command = New SqlCommand(SqlUpdateNext, con)
                        command.ExecuteNonQuery()
                    Catch ex As Exception
                        Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" + ex.Message + "');", True)
                    Finally
                        Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                        con.Close()
                    End Try

                Next
            End If
        End If

        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
    End Sub

    Private Sub BtnInfo_Click(sender As Object, e As EventArgs) Handles BtnInfo.Click
        Dim SqlInfo = "
        Declare @Emp varchar(5);
        Set @Emp = '" + Session("User") + "';

        Set datefirst 1;

        With TableA as
        (
	        Select [EmpNo]
	        ,[Name]
	        ,[Surname]
          FROM [Manpower_Mecha2].[dbo].[Emp_Master] as t1
          WHERE [EmpNo] = @Emp
        ),
        TableB as
        (
	        SELECT [EmpNo]
	            ,DATEPART(Week, [Date]) AS [Week]
                ,SUM([Hours]) as [Accum]
        FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
        WHERE DATEPART(Week, [Date]) = DATEPART(Week, GETDATE())
        AND [Status] = 'Confirmed'
        AND [EmpNo] = @Emp
        GROUP BY  [EmpNo]
                ,[Name]
                ,[Surname]
	            ,DATEPART(Week, [Date])
        ),
        TableC as
        (
	        SELECT [EmpNo]
	            ,DATEPART(MONTH, [Date]) AS [Month]
                ,SUM([Hours]) as [Accum]
        FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
        WHERE DATEPART(MONTH, [Date]) = DATEPART(MONTH, GETDATE())
        AND [Status] = 'Confirmed'
        AND [EmpNo] = @Emp
        GROUP BY  [EmpNo]
                ,[Name]
                ,[Surname]
	            ,DATEPART(MONTH, [Date])
        )

        Select [Name]
		        ,[Surname]
		        ,IsNull(t2.[Accum],0) as [Curr_Week_Hours]
		        ,IsNull(t3.[Accum],0) as [Curr_Month_Hours]
        From TableA as t1
        Left Join TableB as t2
        on t1.[EmpNo] = t2.[EmpNo]
        Left Join TableC as t3
        on t1.[EmpNo] = t3.[EmpNo]
        "
        Dim dt = StandardFunction.GetDataTable(SqlInfo)
        Dim StrName As String = Nothing
        Dim StrSurName As String = Nothing
        Dim StrCurrWeek As String = Nothing
        Dim StrCurrMonth As String = Nothing

        If dt.Rows.Count > 0 Then
            For Each x As DataRow In dt.Rows
                StrName = x.Item("Name")
                StrSurName = x.Item("Surname")
                StrCurrWeek = x.Item("Curr_Week_Hours")
                StrCurrMonth = x.Item("Curr_Month_Hours")
            Next
        End If

        LblName.Text = "ชื่อ-นามสกุล : <b>" + StrName + " " + StrSurName + "</b>"
        LblEmpNo.Text = "รหัสพนักงาน : <b>" & Session("User") & "</b>"
        lblOCurrWeek.Text = "จำนวนโอทีในสัปดาห์นี้ : <b>" + StrCurrWeek + "</b> ชั่วโมง"
        lblOCurrMonth.Text = "จำนวนโอทีในเดือนนี้ : <b>" + StrCurrMonth + "</b> ชั่วโมง"
        Dim SqlPersonal = "
            Declare @Emp varchar(5)
            Set @Emp = '" + Session("User") + "';

            Set datefirst 1;

            With
            TableA as
            (
            SELECT	DATENAME(Month,[Date]) as [Month]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                    WHERE DATEPART(Year,[Date]) = DATEPART(Year,GETDATE())
                    AND [EmpNo] = @Emp
                    GROUP BY DATENAME(Month,[Date])
            ),
            TableB as
            (
            SELECT	DATENAME(Month,[Date]) as [Month]
                    ,SUM([Hours]) as [Hours]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                    WHERE DATEPART(Year,[Date]) = DATEPART(Year,GETDATE())
                    AND [EmpNo] = @Emp
		            AND [Status] = 'Wait Confirm'
                    GROUP BY DATENAME(Month,[Date])
            ),
            TableC as
            (
            SELECT	DATENAME(Month,[Date]) as [Month]
                    ,SUM([Hours]) as [Hours]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                    WHERE DATEPART(Year,[Date]) = DATEPART(Year,GETDATE())
                    AND [EmpNo] = @Emp
		            AND [Status] = 'Confirmed'
                    GROUP BY DATENAME(Month,[Date])
            )

            Select t1.[Month]
		            ,IsNull(t2.[Hours],0) as [Wait Confirm]
		            ,IsNull(t3.[Hours],0) as [Confirmed]
            From TableA as t1
            Left Join TableB as t2
            on t1.[Month] = t2.[Month]
            Left Join TableC as t3
            on t1.[Month] = t3.[Month];"
        StandardFunction.fillDataTableToDataGrid(GrdPersonal, SqlPersonal, "")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "$('#exampleModal').modal();", True)
    End Sub

    Private Sub GrdWaitConfirm_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GrdWaitConfirm.RowDeleting

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim row = GrdWaitConfirm.Rows(e.RowIndex)
        Dim SqlSelectNext = "
        DECLARE @D Date, @Emp Varchar(5);
        SET @D = '" & CDate(row.Cells(1).Text).ToString("yyyy-MM-dd") & "'
        SET @Emp = '" & Session("User") & "'

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
        'TextTest.Text += SqlSelectNext
        Dim dt = StandardFunction.GetDataTable(SqlSelectNext)

        If dt.Rows.Count > 0 Then
            For Each x As DataRow In dt.Rows
                Dim SqlUpdateNext = "
                UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                   SET [Accum_Hours] = " & CDbl(x.Item("Accum_Hours")) - CDbl(row.Cells(6).Text) & "
                 WHERE [Req_detail_Id] = " & x.Item("Req_detail_Id") & "
                "

                Try
                    con.ConnectionString = StandardFunction.connectionString
                    con.Open()
                    command = New SqlCommand(SqlUpdateNext, con)
                    command.ExecuteNonQuery()
                Catch ex As Exception
                    ' Error/fail แล้วทำตรงนี้
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
                    'MsgBox("Update Next " & ex.Message)
                Finally
                    'ไม่ว่าจะ Error หรือ OK ก็ต้องเข้า loop นี้
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                    con.Close()
                End Try
            Next
        End If

        Dim SqlDeleteCurr = "
                DELETE FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                      WHERE [Req_detail_Id] = 
                (SELECT [Req_detail_Id]
                  FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                  Where [Date] = '" & CDate(row.Cells(1).Text).ToString("yyyy-MM-dd") & "'
                  And [EmpNo] = '" & Session("User") & "'
                  And [Hours] > 0)
                "
        'TextTest.Text += SqlDeleteCurr
        Try
            ' พยายามลองทำคำสั่งในนี้
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlDeleteCurr, con)
            command.ExecuteNonQuery()
        Catch ex As Exception
            ' Error/fail แล้วทำตรงนี้
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
            'MsgBox("Delete Curr " & ex.Message)
        Finally
            'ไม่ว่าจะ Error หรือ OK ก็ต้องเข้า loop นี้
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
            con.Close()
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        End Try

    End Sub

    Private Sub GrdWaitConfirm_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GrdWaitConfirm.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim item As String = e.Row.Cells(1).Text
            For Each button As Button In e.Row.Cells(0).Controls.OfType(Of Button)()
                If button.CommandName = "Delete" Then
                    button.Attributes("onclick") = "if(!confirm('ยกเลิกการ Request Overtime วันที่ " + item + "?')){ return false; };"
                End If
            Next
        End If
    End Sub
End Class