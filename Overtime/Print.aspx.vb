Imports Microsoft.Reporting.WebForms

Public Class Print
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
            RadioButtonSelect.AutoPostBack = True
            TxtWorkDate.AutoPostBack = True
            DrpRequester.AutoPostBack = True
            DrpRequestTime.AutoPostBack = True

            DrpProcess.AutoPostBack = True
            DrpShift.AutoPostBack = True

            DrpCompany.AutoPostBack = True

            DrpProcessIndiv.AutoPostBack = True
            DrpName.AutoPostBack = True
        End If
    End Sub

    Sub LoadPage()

    End Sub

    Private Sub BtnShow_Click(sender As Object, e As EventArgs) Handles BtnShow.Click
        ReportViewerPrint.LocalReport.DataSources.Clear()
        Dim SqlPrint As String = Nothing
        Dim SqlPrintProcess As String = Nothing
        Dim dsList As DataTable
        Dim dsProcess As DataTable

        Dim Check = True
        Dim Alert As New StringBuilder("กรุณาเลือก\n")

        If TxtWorkDate.Text = Nothing Then
            Check = False
            Alert.Append("- วันทำงาน\n")
        End If

        If DrpName.SelectedIndex = 0 Then
            Check = False
            Alert.Append("- เลือกชื่อพนักงาน\n")
        End If

        If Not Check Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert.ToString & "');", True)
            Exit Sub
        Else
            If RadioButtonSelect.SelectedValue = 1 And DrpShift.SelectedIndex = 0 Then
                SqlPrint = "
              DECLARE @D Date, @Pr Varchar(50), @Company varchar(20);
                  SET @D = '" & TxtWorkDate.Text & "';
                  SET @Pr = '" & DrpProcess.SelectedValue.Replace("'", "''") & "';
                  SET @Company = '" & DrpCompany.SelectedValue & "';
              SELECT
                Convert(Varchar(10),[Date],103) as [Date]
                                  ,t1.[EmpNo]
                                  ,t1.[Name]
                                  ,t1.[Surname]
                                  ,t1.[Shift]
                                  ,[Hours]
                                  ,[Accum_Hours]
                                  ,t1.[Reason]
                  FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                ON t1.[Req_Id] = t2.[Request_Id]

                LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
                ON t1.[EmpNo] = t3.[EmpNo]

                WHERE
	                t1.[Date] = @D
                AND
	                t2.[Process] = @Pr
                AND
	                t3.[Company] = @Company
                ORDER BY
                    t1.[EmpNo] asc
            "
                TxtTest.Text += SqlPrint
                SqlPrintProcess = "SELECT '" & DrpProcess.SelectedValue.Replace("'", "''") & "' as [Process]"
            ElseIf RadioButtonSelect.SelectedValue = 1 And Not DrpShift.SelectedIndex = 0 Then
                SqlPrint = "
              DECLARE @D Date, @Pr Varchar(50), @Shift varchar(10), @Company varchar(20);
                  SET @D = '" & TxtWorkDate.Text & "';
                  SET @Pr = '" & DrpProcess.SelectedValue.Replace("'", "''") & "';
                  SET @Shift = '" & DrpShift.SelectedValue & "';
                  SET @Company = '" & DrpCompany.SelectedValue & "';
              SELECT
                Convert(Varchar(10),[Date],103) as [Date]
                                  ,t1.[EmpNo]
                                  ,t1.[Name]
                                  ,t1.[Surname]
                                  ,t1.[Shift]
                                  ,[Hours]
                                  ,[Accum_Hours]
                                  ,t1.[Reason]
                  FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                ON t1.[Req_Id] = t2.[Request_Id]

                LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
                ON t1.[EmpNo] = t3.[EmpNo]                

                WHERE
	                t1.[Date] = @D
                AND
	                t1.[Shift] = @Shift
                AND
	                t2.[Process] = @Pr
                AND
	                t3.[Company] = @Company
                ORDER BY
                    t1.[EmpNo] asc
            "
                TxtTest.Text += SqlPrint
                SqlPrintProcess = "SELECT '" & DrpProcess.SelectedValue.Replace("'", "''") & "' as [Process]"
            ElseIf RadioButtonSelect.SelectedValue = 2 And DrpRequestTime.SelectedIndex = 0 Then
                SqlPrint = "
                DECLARE @D Date, @Emp Varchar(5), @Company varchar(20);
                SET @D = '" & TxtWorkDate.Text & "';
                SET @Emp = '" & DrpRequester.SelectedValue & "';
                SET @Company = '" & DrpCompany.SelectedValue & "';
                SELECT 
	                Convert(Varchar(10),[Date],103) as [Date]
                                                  ,t1.[EmpNo]
                                                  ,t1.[Name]
                                                  ,t1.[Surname]
                                                  ,t1.[Shift]
                                                  ,[Hours]
                                                  ,[Accum_Hours]
                                                  ,t1.[Reason]
                FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                ON t1.[Req_Id] = t2.[Request_Id]
                
                LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
                ON t1.[EmpNo] = t3.[EmpNo]
                
                WHERE
	                t1.[Date] = @D
                AND
	                t1.[Status] = 'Confirmed'
                AND
	                t2.[Request_By] = @Emp
                AND
	                Not t2.[Request_By] Is Null
                AND
	                t3.[Company] = @Company
                ORDER BY
                    t1.[EmpNo] asc
            "
                TxtTest.Text += SqlPrint
                SqlPrintProcess = "
                    SELECT 
                    [Process]
                      FROM [Manpower_Mecha2].[dbo].[Emp_Master]
                    WHERE [EmpNo] = '" & DrpRequester.SelectedValue & "'
                "
            ElseIf RadioButtonSelect.SelectedValue = 2 And Not DrpRequestTime.SelectedIndex = 0 Then
                SqlPrint = "
                DECLARE @D Date, @Emp Varchar(5), @T Datetime, @Company varchar(20);
                SET @D = '" & TxtWorkDate.Text & "';
                SET @Emp = '" & DrpRequester.SelectedValue & "';
                SET @T = '" & DrpRequestTime.SelectedValue & "';
                SET @Company = '" & DrpCompany.SelectedValue & "';
                SELECT 
	                Convert(Varchar(10),[Date],103) as [Date]
                                                  ,t1.[EmpNo]
                                                  ,t1.[Name]
                                                  ,t1.[Surname]
                                                  ,t1.[Shift]
                                                  ,[Hours]
                                                  ,[Accum_Hours]
                                                  ,t1.[Reason]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                ON t1.[Req_Id] = t2.[Request_Id]

                LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
                ON t1.[EmpNo] = t3.[EmpNo]                

                WHERE
	                t1.[Date] = @D
                AND
	                t1.[Status] = 'Confirmed'
                AND
	                t2.[Request_By] = @Emp
                AND
	                t2.[Date_Req] = @T
                AND
	                Not t2.[Request_By] Is Null
                AND
	                t3.[Company] = @Company
                ORDER BY
                    t1.[EmpNo] asc
                "
                TxtTest.Text += SqlPrint
                SqlPrintProcess = "
                    SELECT [Process]
                      FROM [Manpower_Mecha2].[dbo].[Request_OT]
                      WHERE 
	                    [Date_Req] = '" & DrpRequestTime.SelectedValue & "'
                      AND
	                    [Request_By] = '" & DrpRequester.SelectedValue & "'
                "
            ElseIf RadioButtonSelect.SelectedValue = 3 And Not DrpName.SelectedIndex = 0 Then
                SqlPrint = "
                 DECLARE @D Date, @Pr Varchar(50), @Company varchar(20), @EName varchar(50);
                  SET @D = '" & TxtWorkDate.Text & "';
                  SET @Pr = '" & DrpProcessIndiv.SelectedValue.Replace("'", "''") & "';
                  SET @Company = '" & DrpCompany.SelectedValue & "';
                  SET @EName = '" & DrpName.SelectedValue & "';
              SELECT
                Convert(Varchar(10),[Date],103) as [Date]
                                  ,t1.[EmpNo]
                                  ,t1.[Name]
                                  ,t1.[Surname]
                                  ,t1.[Shift]
                                  ,[Hours]
                                  ,[Accum_Hours]
                                  ,t1.[Reason]
                  FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                ON t1.[Req_Id] = t2.[Request_Id]

                LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
                ON t1.[EmpNo] = t3.[EmpNo]

                WHERE
	                t1.[Date] = @D
                AND
	                t2.[Process] = @Pr
                AND
	                t3.[Company] = @Company
                AND
					t1.[Name] = @EName
                ORDER BY
                    t1.[EmpNo] asc
                "
                TxtTest.Text += SqlPrint
                SqlPrintProcess = "SELECT '" & DrpProcessIndiv.SelectedValue.Replace("'", "''") & "' as [Process]"
            End If

            dsList = StandardFunction.GetDataTable(SqlPrint)
            dsProcess = StandardFunction.GetDataTable(SqlPrintProcess)
            'TxtTest.Text += SqlPrint
            'TxtTest.Text += (dsList.Rows.Count).ToString
            If dsList.Rows.Count > 0 Then
                ReportViewerPrint.ProcessingMode = ProcessingMode.Local
                ReportViewerPrint.LocalReport.ReportPath = Server.MapPath("~/ReportOvertime.rdlc")
                Dim datasource As New ReportDataSource("DataSetList", dsList)
                Dim datasource2 As New ReportDataSource("DataSetProcess", dsProcess)
                ReportViewerPrint.LocalReport.DataSources.Clear()
                ReportViewerPrint.LocalReport.DataSources.Add(datasource)
                ReportViewerPrint.LocalReport.DataSources.Add(datasource2)
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ไม่พบข้อมูล');", True)
                Exit Sub
            End If

        End If

    End Sub

    Sub Clearform()
        ReportViewerPrint.LocalReport.DataSources.Clear()
        DrpProcess.Visible = False
        DrpShift.Visible = False
        DrpRequester.Visible = False
        DrpRequestTime.Visible = False
        DrpProcessIndiv.Visible = False
        DrpName.Visible = False
        DrpProcess.Dispose()
        DrpShift.Dispose()
        DrpRequester.Dispose()
        DrpRequestTime.Dispose()
        DrpCompany.Dispose()
        DrpProcessIndiv.Dispose()
        DrpName.Dispose()
        RadioButtonSelect.ClearSelection()
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        TxtWorkDate.Text = Nothing
        Clearform()
    End Sub

    Private Sub RadioButtonSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioButtonSelect.SelectedIndexChanged

        DrpProcess.Dispose()
        DrpShift.Dispose()
        DrpRequester.Dispose()
        DrpRequestTime.Dispose()
        DrpProcessIndiv.Dispose()
        DrpName.Dispose()
        ReportViewerPrint.LocalReport.DataSources.Clear()

        Dim Check = True
        Dim Alert As New StringBuilder("กรุณาเลือก\n")

        If TxtWorkDate.Text = Nothing Then
            Check = False
            Alert.Append("- วันทำงาน\n")
        End If

        If Not Check Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert.ToString & "');", True)
            Exit Sub
        Else
            If RadioButtonSelect.SelectedValue = 1 Then
                DrpProcess.Visible = True
                DrpShift.Visible = True
                DrpRequester.Visible = False
                DrpRequestTime.Visible = False
                DrpProcessIndiv.Visible = False
                DrpName.Visible = False
                Dim SqlDrp1 = "
                    DECLARE @D Date, @Company varchar(20);
	                SET @D = '" & TxtWorkDate.Text & "';
                    SET @Company = '" & DrpCompany.SelectedValue & "';

                    SELECT t2.[Process]
                      FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                    LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                    ON t1.[Req_Id] = t2.[Request_Id]

                    LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
                    ON t1.[EmpNo] = t3.[EmpNo]    

                    WHERE
	                    t1.[Date] = @D
                    AND
	                t1.[Status] = 'Confirmed'

                    AND
                    Not t2.[Process] Is Null
                    
                    AND
	                t3.[Company] = @Company
    
                    GROUP BY
	                    t2.[Process]
                    "
                TxtTest.Text += SqlDrp1
                StandardFunction.setDropdownlist(DrpProcess, SqlDrp1)

                DrpShift.Dispose()
                Dim SqlDrp2 = "
                DECLARE @D Date, @Pr varchar(50), @Company varchar(20);
                SET @D = '" & TxtWorkDate.Text & "';
                SET @Pr = '" & DrpProcess.SelectedValue.Replace("'", "''") & "';
                SET @Company = '" & DrpCompany.SelectedValue & "';
                SELECT t1.[Shift]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                ON t1.[Req_Id] = t2.[Request_Id]

                LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
                ON t1.[EmpNo] = t3.[EmpNo]

                WHERE
	                t1.[Date] = @D
                AND
	                t2.[Process] = @Pr
                AND
	                t1.[Status] = 'Confirmed'
                AND
	                Not t2.[Process] Is Null
                AND
	                t3.[Company] = @Company
                GROUP BY
	                t1.[Shift]
            "
                StandardFunction.setDropdownlist(DrpShift, SqlDrp2, "All")

            ElseIf RadioButtonSelect.SelectedValue = 2 Then
                DrpProcess.Visible = False
                DrpShift.Visible = False
                DrpRequester.Visible = True
                DrpRequestTime.Visible = True
                DrpProcessIndiv.Visible = False
                DrpName.Visible = False

                Dim SqlDrpReq = "
                    DECLARE @D Date, @Company varchar(20);
                    SET @D = '" & TxtWorkDate.Text & "';
                    SET @Company = '" & DrpCompany.SelectedValue & "';

                    SELECT t2.[Request_By]
                        FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                    LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                    ON t1.[Req_Id] = t2.[Request_Id]

                    LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
                    ON t1.[EmpNo] = t3.[EmpNo]

                    WHERE
	                    t1.[Date] = @D
                    AND
                    t1.[Status] = 'Confirmed'

                    AND
                    Not t2.[Request_By] Is Null

                    AND
	                t3.[Company] = @Company

                    GROUP BY
                    t2.[Request_By]
                "
                StandardFunction.setDropdownlist(DrpRequester, SqlDrpReq)

                DrpRequestTime.Dispose()
                Dim SqlDrp = "
                DECLARE @D Date, @Emp Varchar(5), @Company varchar(20);
                SET @D = '" & TxtWorkDate.Text & "';
                SET @Emp = '" & DrpRequester.SelectedValue & "';
                SET @Company = '" & DrpCompany.SelectedValue & "';

                SELECT Convert(varchar,t2.[Date_Req],20) as [Date]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                ON t1.[Req_Id] = t2.[Request_Id]

                LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
                ON t1.[EmpNo] = t3.[EmpNo]

                WHERE
	                t1.[Date] = @D
                AND
	                t1.[Status] = 'Confirmed'
                AND
	                t2.[Request_By] = @Emp
                AND
	                Not t2.[Request_By] Is Null
                AND
	                t3.[Company] = @Company
                GROUP BY
	                t2.[Date_Req]
            "
                StandardFunction.setDropdownlist(DrpRequestTime, SqlDrp, "All")
            ElseIf RadioButtonSelect.SelectedValue = 3 Then
                DrpProcess.Visible = False
                DrpShift.Visible = False
                DrpRequester.Visible = False
                DrpRequestTime.Visible = False
                DrpProcessIndiv.Visible = True
                DrpName.Visible = True

                Dim SqlDrp1 = "
                    DECLARE @D Date, @Company varchar(20);
	                SET @D = '" & TxtWorkDate.Text & "';
                    SET @Company = '" & DrpCompany.SelectedValue & "';

                    SELECT t2.[Process]
                      FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                    LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                    ON t1.[Req_Id] = t2.[Request_Id]

                    LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
                    ON t1.[EmpNo] = t3.[EmpNo]    

                    WHERE
	                    t1.[Date] = @D
                    AND
	                t1.[Status] = 'Confirmed'

                    AND
                    Not t2.[Process] Is Null
                    
                    AND
	                t3.[Company] = @Company
    
                    GROUP BY
	                    t2.[Process]
                    "
                TxtTest.Text += SqlDrp1
                StandardFunction.setDropdownlist(DrpProcessIndiv, SqlDrp1)

                DrpName.Dispose()
                'Code บรรทัดเป็นต้นไปจะเหมือน DrpName SelectedIndex...
                Dim SqlDrp2 = "
                DECLARE @D Date, @Pr varchar(50),@Company varchar(20);
                SET @D = '" & TxtWorkDate.Text & "';
                SET @Pr = '" & DrpProcessIndiv.SelectedValue.Replace("'", "''") & "';
				SET @Company = '" & DrpCompany.SelectedValue & "';

                SELECT t1.[Name]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                ON t1.[Req_Id] = t2.[Request_Id]

				LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
                ON t1.[EmpNo] = t3.[EmpNo] 

                WHERE
	                t1.[Date] = @D
                AND
	                t2.[Process] = @Pr
                AND
	                t1.[Status] = 'Confirmed'
                AND
	                Not t2.[Process] Is Null
				AND
	                t3.[Company] = @Company
                GROUP BY
	                t1.[Name]
        "
                StandardFunction.setDropdownlist(DrpName, SqlDrp2, "")
            End If
        End If

    End Sub

    Private Sub TxtWorkDate_TextChanged(sender As Object, e As EventArgs) Handles TxtWorkDate.TextChanged
        Clearform()
        If IsDate(TxtWorkDate.Text) Then
            Dim SqlGetCompany = "
            DECLARE @D Date;
                SET @D = '" & TxtWorkDate.Text & "';

            SELECT t3.[Company]
            FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

            LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
            ON t1.[EmpNo] = t3.[EmpNo]

            WHERE
	            t1.[Date] = @D
            AND
	            t1.[Status] = 'Confirmed'
            GROUP BY
	            t3.[Company]
            "
            Dim dt = StandardFunction.GetDataTable(SqlGetCompany)
            If dt.Rows.Count > 0 Then
                StandardFunction.setDropdownlist(DrpCompany, SqlGetCompany)
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ไม่พบข้อมูล');", True)
                Exit Sub
            End If

        End If
    End Sub

    Private Sub DrpProcess_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DrpProcess.SelectedIndexChanged
        DrpShift.Dispose()
        'If Not DrpProcess.SelectedIndex = 0 Then
        Dim SqlDrp = "
                DECLARE @D Date, @Pr varchar(50);
                SET @D = '" & TxtWorkDate.Text & "';
                SET @Pr = '" & DrpProcess.SelectedValue.Replace("'", "''") & "';

                SELECT t1.[Shift]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                ON t1.[Req_Id] = t2.[Request_Id]

                WHERE
	                t1.[Date] = @D
                AND
	                t2.[Process] = @Pr
                AND
	                t1.[Status] = 'Confirmed'
                AND
	                Not t2.[Process] Is Null
                GROUP BY
	                t1.[Shift]
            "
        StandardFunction.setDropdownlist(DrpShift, SqlDrp, "All")
        'End If
    End Sub

    Private Sub DrpRequester_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DrpRequester.SelectedIndexChanged
        DrpRequestTime.Dispose() 'ถ้าจะให้ดีควรใส่ Company ด้วย
        Dim SqlDrp = "
                DECLARE @D Date, @Emp Varchar(5);
                SET @D = '" & TxtWorkDate.Text & "';
                SET @Emp = '" & DrpRequester.SelectedValue & "';

                SELECT Convert(varchar,t2.[Date_Req],20) as [Date]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                ON t1.[Req_Id] = t2.[Request_Id]

                WHERE
	                t1.[Date] = @D
                AND
	                t1.[Status] = 'Confirmed'
                AND
	                t2.[Request_By] = @Emp
                AND
	                Not t2.[Request_By] Is Null
                GROUP BY
	                t2.[Date_Req]
            "
        StandardFunction.setDropdownlist(DrpRequestTime, SqlDrp, "All")
    End Sub

    Private Sub DrpProcessIndiv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DrpProcessIndiv.SelectedIndexChanged
        DrpName.Dispose()
        Dim SqlDrp = "
                DECLARE @D Date, @Pr varchar(50),@Company varchar(20);
                SET @D = '" & TxtWorkDate.Text & "';
                SET @Pr = '" & DrpProcessIndiv.SelectedValue.Replace("'", "''") & "';
				SET @Company = '" & DrpCompany.SelectedValue & "';

                SELECT t1.[Name]
                    FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                ON t1.[Req_Id] = t2.[Request_Id]

				LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
                ON t1.[EmpNo] = t3.[EmpNo] 

                WHERE
	                t1.[Date] = @D
                AND
	                t2.[Process] = @Pr
                AND
	                t1.[Status] = 'Confirmed'
                AND
	                Not t2.[Process] Is Null
				AND
	                t3.[Company] = @Company
                GROUP BY
	                t1.[Name]
        "
        StandardFunction.setDropdownlist(DrpName, SqlDrp, "")
    End Sub
End Class