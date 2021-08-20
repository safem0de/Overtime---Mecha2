Imports System.Data.SqlClient

Public Class LeaderConfirm
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

        If Session("Position") = "CLERK" Then
            BtnConfUnConfDate.Enabled = False
            BtnUnConfSave.Enabled = False
            BtnUnConfDel.Enabled = False
        End If

        If Not IsPostBack Then
            TxtUnConfDate.AutoPostBack = True
            TxtConfDate.AutoPostBack = True
        End If

        LoadPage()
    End Sub

    Sub LoadPage()

        Dim SqlWaitConf = "
            select convert(varchar(10),t2.[Date],23) as [Date]
                              ,t2.[EmpNo]
                              ,t2.[Name]
                              ,t2.[Surname]
                              ,t2.[Shift]
                              ,t2.[Section]
                              ,t2.[Process]
                              ,t2.[Hours]
                              ,t2.[Accum_Hours]
                              ,t2.[Status]
                              ,t2.[Reason]
                        from [Manpower_Mecha2].[dbo].[Emp_Master] as t1

                        Left join [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t2
                        on t1.[Section] = t2.[Section] or t1.[Process] = t2.[Process]

                        Left Join [Manpower_Mecha2].[dbo].[Authorize_Process] as t3
                        On t1.[EmpNo] = t3.[EmpNo]

                        where t1.[EmpNo] = '" & Session("User") & "'
                        and t2.[Date] = convert(Date,'" & TxtUnConfDate.Text & "')
                        and t2.[Req_id] = 0
                        and t2.[Status] = 'Wait Confirm'
                        and t2.[Process] = t3.[Process];
            "

        Dim SqlConfirm = Nothing

        If TxtConfDate.Text = Nothing Then

            If Session("Position") = "STAFF" Or Session("Position") = "CLERK" Then
                SqlConfirm = "
                Select convert(varchar(10),t2.[Date],23) as [Date]
                              ,t2.[EmpNo]
                              ,t2.[Name]
                              ,t2.[Surname]
                              ,t2.[Shift]
                              ,t2.[Section]
                              ,t2.[Process]
                              ,t2.[Hours]
                              ,t2.[Accum_Hours]
                              ,t2.[Status]
                              ,t2.[Reason]
						      ,t3.[Request_By] as [Confirmed_By]
                        from [Manpower_Mecha2].[dbo].[Emp_Master] as t1

                        Left join [Manpower_Mecha2].[dbo].[Request_OT_Detail] t2
                        on t1.[Section]=t2.[Section] or t1.[Process] = t2.[Process]

					    Left join [Manpower_Mecha2].[dbo].[Request_OT] t3
                        on t2.[Req_Id] = t3.[Request_Id]

                        where
                            t1.[EmpNo] = '" & Session("User") & "'
                        And
                            Not t2.[Status] = 'Wait Confirm'
                        And
						    DATEDIFF(DAY,GETDATE(),[Date]) >= -2
					    Order by
						    t2.[Date],t2.[EmpNo] asc
                    "
            ElseIf Session("Position") = "LEADER" Then
                SqlConfirm = "
                SELECT
                Convert(varchar(10),[Date],23) as [Date]
                        ,[EmpNo]
                        ,[Name]
                        ,[Surname]
                        ,[Shift]
                        ,[Section]
                        ,t1.[Process]
                        ,[Hours]
                        ,[Accum_Hours]
                        ,[Status]
                        ,t1.[Reason]
		                ,t2.[Request_By] as [Confirmed_By]
                FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1
                LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                ON t1.[Req_Id] = t2.[Request_Id]
                WHERE 
                t2.[Request_By] = '" & Session("User") & "'
                AND
                NOT [Status] = 'Wait Confirm'
                AND
                DATEDIFF(DAY,GETDATE(),[Date]) >= -2
                ORDER BY
                [Date] desc
            "
            End If

        Else

            If Session("Position") = "STAFF" Or Session("Position") = "CLERK" Then
                SqlConfirm = "select convert(varchar(10),t2.[Date],23) as [Date]
                          ,t2.[EmpNo]
                          ,t2.[Name]
                          ,t2.[Surname]
                          ,t2.[Shift]
                          ,t2.[Section]
                          ,t2.[Process]
                          ,t2.[Hours]
                          ,t2.[Accum_Hours]
                          ,t2.[Status]
                          ,t2.[Reason]
						  ,t3.[Request_By] as [Confirmed_By]
                    from [Manpower_Mecha2].[dbo].[Emp_Master] as t1
                    Left join [Manpower_Mecha2].[dbo].[Request_OT_Detail] t2
                    on t1.[Section]=t2.[Section] or t1.[Process] = t2.[Process]
					Left join [Manpower_Mecha2].[dbo].[Request_OT] t3
                    on t2.[Req_Id] = t3.[Request_Id]
                    where
                        t1.[EmpNo] = '" & Session("User") & "'
                    And
                        t2.[Date] = convert(Date,'" & TxtConfDate.Text & "')
                    And 
                        Not t2.[Status] = 'Wait Confirm'
                    "
            ElseIf Session("Position") = "LEADER" Then
                SqlConfirm = "
                SELECT
                Convert(varchar(10),[Date],23) as [Date]
                                          ,[EmpNo]
                                          ,[Name]
                                          ,[Surname]
                                          ,[Shift]
                                          ,[Section]
                                          ,t1.[Process]
                                          ,[Hours]
                                          ,[Accum_Hours]
                                          ,[Status]
                                          ,t1.[Reason]
						                  ,t2.[Request_By] as [Confirmed_By]
                  FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1
                  LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                  ON t1.[Req_Id] = t2.[Request_Id]
                  WHERE 
                    t2.[Request_By] = '" & Session("User") & "'
                  AND
                    [Date] = convert(Date,'" & TxtConfDate.Text & "')
                  AND
                    NOT [Status] = 'Wait Confirm'
                  AND
                    DATEDIFF(DAY,[Date],GETDATE()) <= 3
                  ORDER BY
                    [Date] desc
                "
            End If
        End If

        StandardFunction.fillDataTableToDataGrid(GrdWaitConf, SqlWaitConf, "")
        StandardFunction.fillDataTableToDataGrid(GrdConfirm, SqlConfirm, "")

        If GrdWaitConf.Rows.Count = 0 Then
            BtnConfUnConfDate.Enabled = False
        Else
            BtnConfUnConfDate.Enabled = True
        End If

    End Sub
    Sub BindColumn(GV As GridView, dt As DataTable, Optional SsName As String = Nothing)
        If Not SsName = Nothing Then
            Session(SsName) = dt
        End If
        GV.DataSource = dt
        GV.DataBind()
    End Sub

    Private Sub GrdWaitConf_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdWaitConf.SelectedIndexChanged

        Dim row As GridViewRow = GrdWaitConf.SelectedRow
        Dim sendText As New ArrayList

        For i = 0 To row.Cells.Count - 1
            If Not row.Cells(i).Text = Nothing Then
                sendText.Add(row.Cells(i).Text)
            End If
        Next

        Session("Edit_Del_UnConf") = sendText

        Dim txt As New StringBuilder()
        txt.Append("<h5 style='color:blue'>" & row.Cells(3).Text & " " & row.Cells(4).Text & "</h5>")
        txt.Append("&emsp;รหัสพนักงาน : <b>" & row.Cells(2).Text & "</b> กะ <b>" & row.Cells(5).Text & "</b>")
        txt.Append("<br />")
        txt.Append("&emsp;กระบวนการ : <b>" & row.Cells(7).Text & "</b>")
        txt.Append("<br />")
        txt.Append("&emsp;วันที่รีเควสโอ-ที : <b>" & row.Cells(1).Text & "</b> ชั่วโมง")
        txt.Append("<br />")
        txt.Append("&emsp;จำนวนชั่วโมงโอ-ที(สะสม) : <b>" & row.Cells(9).Text & "</b>")
        txt.Append("<br />")
        txt.Append("&emsp;เหตุผล : <b>เพื่อ" & row.Cells(11).Text & "</b>")
        txt.Append("<br />")
        Label.Text = txt.ToString()
        TxtEditHour.Text = row.Cells(8).Text
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "$('#exampleModal').modal();", True)
    End Sub

    Private Sub TxtUnConfDate_TextChanged(sender As Object, e As EventArgs) Handles TxtUnConfDate.TextChanged

        GrdWaitConf.DataSource = Nothing
        GrdWaitConf.DataBind()

        LoadPage()

    End Sub

    Private Sub GrdConfirm_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdConfirm.SelectedIndexChanged

        Dim SqlShift = "SELECT * FROM [Manpower_Mecha2].[dbo].[Shift_Master]"
        StandardFunction.setDropdownlist(DrpShift, SqlShift, "")
        DrpShift.AutoPostBack = False

        Dim row As GridViewRow = GrdConfirm.SelectedRow
        Dim sendText As New ArrayList

        For i = 0 To row.Cells.Count - 1
            If Not row.Cells(i).Text = Nothing Then
                sendText.Add(row.Cells(i).Text)
            End If
        Next

        Session("Edit_Del_Conf") = sendText

        Dim txt As New StringBuilder()
        txt.Append("<h5 style='color:blue'>" & row.Cells(3).Text & " " & row.Cells(4).Text & "</h5>")
        txt.Append("&emsp;รหัสพนักงาน : <b>" & row.Cells(2).Text & "</b>")
        txt.Append("<br />")
        txt.Append("&emsp;กระบวนการ : <b>" & row.Cells(7).Text & "</b>")
        txt.Append("<br />")
        txt.Append("&emsp;วันที่รีเควสโอ-ที : <b>" & row.Cells(1).Text & "</b> สะสม <b>" & row.Cells(9).Text & "</b> ชั่วโมง")
        txt.Append("<br />")

        LblInfo.Text = txt.ToString
        DrpShift.SelectedValue = row.Cells(5).Text
        DrpHours.SelectedValue = row.Cells(8).Text
        DrpReason.SelectedValue = row.Cells(11).Text
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "$('#exampleModal2').modal();", True)
    End Sub

    Private Sub BtnConfUnConfDate_Click(sender As Object, e As EventArgs) Handles BtnConfUnConfDate.Click

        Dim StrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim SqlInsert = "
        INSERT INTO [Manpower_Mecha2].[dbo].[Request_OT]
               ([Date_Req]
               ,[Request_By]
               ,[Reason])
         VALUES
               (Convert(datetime,'" & StrDate & "')
               ,'" & Session("User") & "'
               ,'Confirmed')
        "

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlInsert, con)
            command.ExecuteNonQuery()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('เกิดข้อผิดพลาด');", True)
        Finally
            con.Close()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('OK');", True)
        End Try

        Dim SqlReq_Id = "SELECT [Request_Id]
          FROM [Manpower_Mecha2].[dbo].[Request_OT]
          Where [Date_Req] = Convert(datetime,'" & StrDate & "')
          And [Request_By] = '" & Session("User") & "'
          And [Reason] = 'Confirmed'"

        Dim Req_Id = StandardFunction.getSQLDataString(SqlReq_Id)
        Dim SqlWaitConf = "select convert(varchar(10),t2.[Date],23) as [Date]
                          ,t2.[EmpNo]
                    from [Manpower_Mecha2].[dbo].[Emp_Master] as t1
                    Left join [Manpower_Mecha2].[dbo].[Request_OT_Detail] t2
                    on t1.[Section]=t2.[Section] or t1.[Process]=t2.[Process]
                    where t1.[EmpNo] = '" & Session("User") & "'
                    and t2.[Date] = convert(Date,'" & TxtUnConfDate.Text & "')
                    and t2.[Req_id] = 0
                    and t2.[Status] = 'Wait Confirm';"
        Dim dt = StandardFunction.GetDataTable(SqlWaitConf)

        'TextTest.Text += SqlWaitConf
        If dt.Rows.Count > 0 Then
            For Each x As DataRow In dt.Rows
                Dim SqlUpdate = "
            UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
               SET [Req_Id] = " & CInt(Req_Id) & "
                  ,[Status] = 'Confirmed'
             WHERE [Date] = '" & CDate(x.Item("Date")).ToString("yyyy-MM-dd") & "'
             AND [EmpNo] = '" & x.Item("EmpNo") & "'
            "
                'TextTest.Text += SqlUpdate
                Try
                    con.ConnectionString = StandardFunction.connectionString
                    con.Open()
                    command = New SqlCommand(SqlUpdate, con)
                    command.ExecuteNonQuery()
                Catch ex As Exception
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('เกิดข้อผิดพลาด : '" & ex.Message & ");", True)
                Finally
                    con.Close()
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ยืนยันรายชื่อ เรียบร้อย');", True)
                End Try
            Next
        End If
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
    End Sub

    Private Sub BtnUnConfDel_Click(sender As Object, e As EventArgs) Handles BtnUnConfDel.Click
        Dim arr = Session("Edit_Del_UnConf")

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim SqlUpdateCurr = "
        UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
           SET [Hours] = 0
              ,[Accum_Hours] = 0
              ,[Status] = 'Denied'
         WHERE [Date] = '" & CDate(arr(0)).ToString("yyyy-MM-dd") & "'
         AND [EmpNo] = '" & arr(1) & "'       
        "

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlUpdateCurr, con)
            command.ExecuteNonQuery()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
        Finally
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
            con.Close()
        End Try

        Dim SqlSelectNext = "
        DECLARE @D Date, @Emp Varchar(5);
        SET @D = '" & CDate(arr(0)).ToString("yyyy-MM-dd") & "'
        SET @Emp = '" & arr(1) & "'

        Set datefirst 1;

        SELECT [Req_detail_Id]
              ,[Date]
	          ,[EmpNo]
              ,[Hours]
	          ,(
	          SELECT [Manpower_Mecha2].[dbo].[GETACCUM] ([Date],@Emp)
	          ) as [Accum_Hours]
          FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]

        WHERE
	        DATEPART(Week, [Date]) = DATEPART(Week, @D)
        AND
	        [EmpNo] = @Emp
        "

        Dim dt = StandardFunction.GetDataTable(SqlSelectNext)

        If dt.Rows.Count > 0 Then
            For Each x As DataRow In dt.Rows
                Dim SqlUpdateNext = "
                UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                   SET [Accum_Hours] = " & CDbl(x.Item("Accum_Hours")) & "
                 WHERE [Req_detail_Id] = " & x.Item("Req_detail_Id") & "
                "
                Try
                    con.ConnectionString = StandardFunction.connectionString
                    con.Open()
                    command = New SqlCommand(SqlUpdateNext, con)
                    command.ExecuteNonQuery()
                Catch ex As Exception
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
                Finally
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                    con.Close()
                End Try
            Next
        End If


        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
    End Sub

    Private Sub BtnConfDel_Click(sender As Object, e As EventArgs) Handles BtnConfDel.Click
        Dim arr = Session("Edit_Del_Conf")

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim SqlDeleteCurr = "
        DELETE FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                      WHERE [Req_detail_Id] = 
                (SELECT [Req_detail_Id]
                  FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                  Where [Date] = '" & CDate(arr(0)).ToString("yyyy-MM-dd") & "'
                  And [EmpNo] = '" & arr(1) & "')
        "

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlDeleteCurr, con)
            command.ExecuteNonQuery()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลผิดพลาด');", True)
        Finally
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
            con.Close()
        End Try

        Dim SqlSelectNext = "
        DECLARE @D Date, @Emp Varchar(5);
        SET @D = '" & CDate(arr(0)).ToString("yyyy-MM-dd") & "'
        SET @Emp = '" & arr(1) & "'

        Set datefirst 1;

        SELECT [Req_detail_Id]
              ,[Date]
	          ,[EmpNo]
              ,[Hours]
	          ,(
	          SELECT [Manpower_Mecha2].[dbo].[GETACCUM] ([Date],@Emp)
	          ) as [Accum_Hours]
          FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]

        WHERE
	        DATEPART(Week, [Date]) = DATEPART(Week, @D)
        AND
	        [EmpNo] = @Emp
        "
        Dim dt = StandardFunction.GetDataTable(SqlSelectNext)

        If dt.Rows.Count > 0 Then
            For Each x As DataRow In dt.Rows
                Dim SqlUpdateNext = "
                UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                   SET [Accum_Hours] = " & CDbl(x.Item("Accum_Hours")) & "
                 WHERE [Req_detail_Id] = " & x.Item("Req_detail_Id") & "
                "
                Try
                    con.ConnectionString = StandardFunction.connectionString
                    con.Open()
                    command = New SqlCommand(SqlUpdateNext, con)
                    command.ExecuteNonQuery()
                Catch ex As Exception
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลผิดพลาด');", True)
                Finally
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                    con.Close()
                End Try
            Next
        End If

        LoadPage()
    End Sub

    Private Sub BtnUnConfSave_Click(sender As Object, e As EventArgs) Handles BtnUnConfSave.Click

        Dim arr = Session("Edit_Del_UnConf")

        Dim con As New SqlConnection
        Dim command As SqlCommand

        If Not CDbl(TxtEditHour.Text) = CDbl(arr(7)) Then

            Dim SqlUpdateCurr = "
            UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
               SET [Hours] = '" & CDbl(TxtEditHour.Text) & "'
             WHERE [Date] = '" & CDate(arr(0)).ToString("yyyy-MM-dd") & "'
             AND [EmpNo] = '" & arr(1) & "'       
            "

            Try
                con.ConnectionString = StandardFunction.connectionString
                con.Open()
                command = New SqlCommand(SqlUpdateCurr, con)
                command.ExecuteNonQuery()
            Catch ex As Exception
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
            Finally
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                con.Close()
            End Try

            Dim SqlSelectNext = "
            DECLARE @D Date, @Emp Varchar(5);
            SET @D = '" & CDate(arr(0)).ToString("yyyy-MM-dd") & "';
            SET @Emp = '" & arr(1) & "';

            Set datefirst 1;

            SELECT [Req_detail_Id]
                  ,[Date]
	              ,[EmpNo]
                  ,[Hours]
	              ,(
	              SELECT [Manpower_Mecha2].[dbo].[GETACCUM] ([Date],@Emp)
	              ) as [Accum_Hours]
              FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]

            WHERE
	            DATEPART(Week, [Date]) = DATEPART(Week, @D)
            AND
	            [EmpNo] = @Emp
            "

            Dim dt = StandardFunction.GetDataTable(SqlSelectNext)

            If dt.Rows.Count > 0 And dt.Columns.Count > 1 Then
                For Each x As DataRow In dt.Rows
                    Dim SqlUpdateNext = "
                UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                   SET [Accum_Hours] = " & CDbl(x.Item("Accum_Hours")) & "
                 WHERE [Req_detail_Id] = " & x.Item("Req_detail_Id") & "
                "
                    Try
                        con.ConnectionString = StandardFunction.connectionString
                        con.Open()
                        command = New SqlCommand(SqlUpdateNext, con)
                        command.ExecuteNonQuery()
                    Catch ex As Exception
                        Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
                    Finally
                        Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                        con.Close()
                    End Try
                Next
            End If
        End If

        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
    End Sub

    Private Sub BtnConfSave_Click(sender As Object, e As EventArgs) Handles BtnConfSave.Click

        'Dim arr = Session("Edit_Del_Conf")
        'MsgBox(arr(0))
        'MsgBox(arr(1))
        'MsgBox(arr(2))
        'MsgBox(arr(3))
        'MsgBox(arr(4))
        'MsgBox(arr(5))
        'MsgBox(arr(6))
        'MsgBox(arr(7))
        Dim Check As Boolean = True
        Dim Alert As New StringBuilder("กรุณาระบุ \n")

        If DrpShift.SelectedIndex = 0 Or DrpShift.SelectedValue = Nothing Then
            Check = False
            Alert.Append("- กะการทำงาน \n")
        End If

        If DrpHours.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- ชั่วโมงการทำงาน \n")
        End If

        If DrpReason.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- เหตุผล \n")
        End If


        If Check Then
            Dim arr = Session("Edit_Del_Conf")

            Dim con As New SqlConnection
            Dim command As SqlCommand

            If Not CDbl(DrpHours.SelectedValue) = CDbl(arr(7)) Then

                Dim SqlUpdateCurr = "
                UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                   SET [Hours] = '" & CDbl(DrpHours.SelectedValue) & "',
                        [Reason] = '" & DrpReason.SelectedValue & "',
                        [Shift] = '" & DrpShift.SelectedValue & "'
                 WHERE [Date] = '" & CDate(arr(0)).ToString("yyyy-MM-dd") & "'
                 AND [EmpNo] = '" & arr(1) & "'       
                "

                'TextTest.Text += SqlUpdateCurr
                Try
                    con.ConnectionString = StandardFunction.connectionString
                    con.Open()
                    command = New SqlCommand(SqlUpdateCurr, con)
                    command.ExecuteNonQuery()
                Catch ex As Exception
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
                Finally
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                    con.Close()
                End Try

                Dim SqlSelectNext As String = "
                DECLARE @D Date, @Emp Varchar(5);
                SET @D = '" & CDate(arr(0)).ToString("yyyy-MM-dd") & "';
                SET @Emp = '" & arr(1) & "';

                Set datefirst 1;

                SELECT [Req_detail_Id]
                      ,[Date]
	                  ,[EmpNo]
                      ,[Hours]
	                  ,(
	                  SELECT [Manpower_Mecha2].[dbo].[GETACCUM] ([Date],@Emp)
	                  ) as [Accum_Hours]
                  FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]

                WHERE
	                DATEPART(Week, [Date]) = DATEPART(Week, @D)
                AND
	                [EmpNo] = @Emp
                "
                'TextTest.Text += SqlSelectNext
                Dim dt = StandardFunction.GetDataTable(SqlSelectNext)

                If dt.Rows.Count > 0 And dt.Columns.Count > 1 Then
                    For Each x As DataRow In dt.Rows
                        Dim SqlUpdateNext = "
                UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                   SET [Accum_Hours] = " & CDbl(x.Item("Accum_Hours")) & "
                 WHERE [Req_detail_Id] = " & x.Item("Req_detail_Id") & "
                "
                        'TextTest.Text += SqlUpdateNext
                        Try
                            con.ConnectionString = StandardFunction.connectionString
                            con.Open()
                            command = New SqlCommand(SqlUpdateNext, con)
                            command.ExecuteNonQuery()
                        Catch ex As Exception
                            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
                        Finally
                            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                            con.Close()
                        End Try
                    Next
                End If

            ElseIf Not DrpShift.SelectedValue = arr(4) Then
                'MsgBox("test")
                Dim SqlEditShift = "
                    UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                       SET [Shift] = '" & DrpShift.SelectedValue & "'
                     WHERE [Date] = '" & CDate(arr(0)).ToString("yyyy-MM-dd") & "'
                     AND [EmpNo] = '" & arr(1) & "'
                "

                Try
                    con.ConnectionString = StandardFunction.connectionString
                    con.Open()
                    command = New SqlCommand(SqlEditShift, con)
                    command.ExecuteNonQuery()
                Catch ex As Exception
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
                Finally
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                    con.Close()
                End Try

            End If

            Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)

        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & Alert.ToString & "');", True)
        End If

    End Sub

    Private Sub TxtConfDate_TextChanged(sender As Object, e As EventArgs) Handles TxtConfDate.TextChanged

        GrdConfirm.DataSource = Nothing
        GrdConfirm.DataBind()

        LoadPage()
    End Sub

    Private Sub BtnUpdate_Click(sender As Object, e As EventArgs) Handles BtnUpdate.Click
        Dim arr = Session("Edit_Del_Conf")

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim SqlSelectNext As String = "
                DECLARE @D Date, @Emp Varchar(5);
                SET @D = '" & CDate(arr(0)).ToString("yyyy-MM-dd") & "';
                SET @Emp = '" & arr(1) & "';

                Set datefirst 1;

                SELECT [Req_detail_Id]
                      ,[Date]
	                  ,[EmpNo]
                      ,[Hours]
	                  ,(
	                  SELECT [Manpower_Mecha2].[dbo].[GETACCUM] ([Date],@Emp)
	                  ) as [Accum_Hours]
                  FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]

                WHERE
	                DATEPART(Week, [Date]) = DATEPART(Week, @D)
                AND
	                [EmpNo] = @Emp
                "
        'TextTest.Text += SqlSelectNext
        Dim dt = StandardFunction.GetDataTable(SqlSelectNext)

        If dt.Rows.Count > 0 And dt.Columns.Count > 1 Then
            Dim SqlUpdateNext As New StringBuilder()
            For Each x As DataRow In dt.Rows
                SqlUpdateNext.Append("
                UPDATE [Manpower_Mecha2].[dbo].[Request_OT_Detail]
                   SET [Accum_Hours] = " & CDbl(x.Item("Accum_Hours")) & "
                WHERE [Req_detail_Id] = " & CInt(x.Item("Req_detail_Id")) & "
                ")

            Next
            'TextTest.Text += SqlUpdateNext.ToString
            Try
                con.ConnectionString = StandardFunction.connectionString
                con.Open()
                command = New SqlCommand(SqlUpdateNext.ToString, con)
                command.ExecuteNonQuery()
            Catch ex As Exception
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
            Finally
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
                con.Close()
            End Try
        End If

        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)

    End Sub
End Class