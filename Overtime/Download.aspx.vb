Public Class Download
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Session("Position") = "STAFF" _
                Or Session("Position") = "LEADER" _
                Or Session("Position") = "CLERK") Then
            Response.Redirect("~/Default.aspx")
        End If
    End Sub

    Private Sub BtnDownload_Click(sender As Object, e As EventArgs) Handles BtnDownload.Click
        Dim Check As Boolean = True
        Dim Alert As New StringBuilder("กรุณาเลือก")
        Dim Sql1 As String
        Dim Sql2 As String

        If TxtWorkDate.Text = "" Then
            Check = False
            Alert.Append(" วันที่,")
        End If

        If DrpGroup1.Text = "" Then
            Check = False
            Alert.Append(" กลุ่ม1,")
        End If

        If DrpGroup2.Text = "" Then
            Check = False
            Alert.Append(" กลุ่ม2,")
        End If

        If DrpGroup3.Text = "" Then
            Check = False
            Alert.Append(" กลุ่ม3,")
        End If

        If DrpMN1.SelectedIndex = 0 Then
            Check = False
            Alert.Append(" MN1,")
        End If

        If DrpMN2.SelectedIndex = 0 Then
            Check = False
            Alert.Append(" MN2,")
        End If

        If DrpGroup1.SelectedValue.Equals(DrpGroup2.SelectedValue) Or DrpGroup1.SelectedValue.Equals(DrpGroup3.SelectedValue) Or DrpGroup2.SelectedValue.Equals(DrpGroup3.SelectedValue) Then
            If DrpGroup1.SelectedIndex > 0 Or DrpGroup2.SelectedIndex > 0 Or DrpGroup3.SelectedIndex > 0 Then
                Check = False
                Alert.Append(" กลุ่ม(GROUP) ซ้ำ,")
            End If
        End If

        If DrpMN1.SelectedValue.Equals(DrpMN2.SelectedValue) Then
            If DrpMN1.SelectedIndex > 0 Or DrpMN2.SelectedIndex > 0 Then
                Check = False
                Alert.Append(" กลุ่ม(MN) ซ้ำ,")
            End If
        End If

        If Check = False Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & Alert.ToString.Substring(0, Alert.Length - 1) & "');", True)
            Exit Sub
        Else
            Sql1 = "
            DECLARE @Date varchar(10);
            SET @Date = '" & TxtWorkDate.Text & "';
            With TableA as
            (
            SELECT t1.[EmpNo]
            ,t1.[Shift]
            ,Convert(varchar,Year([Date])) as [RQ.Year]
            ,Convert(varchar,Format([Date],'MM')) as [RQ.Month]
            ,Convert(varchar,Format([Date],'dd')) as [RQ.Day]
            ,CASE
            WHEN t1.[Shift] = 'GROUP1' THEN '" & DrpGroup1.Text & "'
            WHEN t1.[Shift] = 'GROUP2' THEN '" & DrpGroup2.Text & "'
            WHEN t1.[Shift] = 'GROUP3' THEN '" & DrpGroup3.Text & "'
            WHEN t1.[Shift] = 'MN1' THEN '" & DrpMN1.Text & "'
            WHEN t1.[Shift] = 'MN2' THEN '" & DrpMN2.Text & "'
            WHEN t1.[Shift] = 'DDDD' THEN 'D'
            WHEN t1.[Shift] = 'AAAA' THEN 'A'
            WHEN t1.[Shift] = 'MMMM' THEN 'M'
            WHEN t1.[Shift] = 'N' THEN 'N'
            ELSE t1.[Shift]
            END AS [OT shift]
            ,CASE WHEN t2.[Company] like 'Mecha%' Then 'P' 
            Else 'S'
            end as [EmpType]
            FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1
            LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t2
            ON t1.[EmpNo] = t2.[EmpNo]
            WHERE [Date] = @Date
            AND
            t1.[Status] = 'Confirmed'
            )

            Select
            [EmpNo]
            ,[Shift]
            ,[RQ.Year]
            ,[RQ.Month]
            ,[RQ.Day]
            ,[OT shift]
            ,[EmpType]
            From TableA
            Where 
            Not [Shift] like 'GROUP%'
            "

            Sql2 = "
            DECLARE @Date varchar(10);
            SET @Date = '" & TxtWorkDate.Text & "';
            With TableA as
            (
            SELECT t1.[EmpNo]
            ,t1.[Shift]
            ,Convert(varchar,Year([Date])) as [RQ.Year]
            ,Convert(varchar,Format([Date],'MM')) as [RQ.Month]
            ,Convert(varchar,Format([Date],'dd')) as [RQ.Day]
            ,CASE
            WHEN t1.[Shift] = 'GROUP1' THEN '" & DrpGroup1.Text & "'
            WHEN t1.[Shift] = 'GROUP2' THEN '" & DrpGroup2.Text & "'
            WHEN t1.[Shift] = 'GROUP3' THEN '" & DrpGroup3.Text & "'
            WHEN t1.[Shift] = 'MN1' THEN '" & DrpMN1.Text & "'
            WHEN t1.[Shift] = 'MN2' THEN '" & DrpMN2.Text & "'
            WHEN t1.[Shift] = 'DDDD' THEN 'D'
            WHEN t1.[Shift] = 'AAAA' THEN 'A'
            WHEN t1.[Shift] = 'MMMM' THEN 'M'
            WHEN t1.[Shift] = 'N' THEN 'N'
            ELSE t1.[Shift]
            END AS [OT shift]
            ,CASE WHEN t2.[Company] like 'Mecha%' Then 'P' 
            Else 'S'
            end as [EmpType]
            FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1
            LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t2
            ON t1.[EmpNo] = t2.[EmpNo]
            WHERE [Date] = @Date
            AND
            t1.[Status] = 'Confirmed'
            )

            Select
            [EmpNo]
            ,[Shift]
            ,[RQ.Year]
            ,[RQ.Month]
            ,[RQ.Day]
            ,[OT shift]
            ,[EmpType]
            From TableA
            Where 
            [Shift] like 'GROUP%'
            "
            'StandardFunction.ExportExcel(Me, Sql, "Overtime_" & DateTime.Now.ToString("yyyyMMdd") & "_" & Session("User"))

            Dim arr As New ArrayList
            arr.Add({Sql1, "MN Normal"})
            arr.Add({Sql2, "Group"})
            StandardFunction.ExportExcelMultiSheet(Me, arr, "Overtime_" & DateTime.Now.ToString("yyyyMMdd") & "_" & Session("User"))
            Clearform()
        End If
    End Sub

    Sub Clearform()
        TxtWorkDate.Text = Nothing
        DrpGroup1.SelectedIndex = 0
        DrpGroup2.SelectedIndex = 0
        DrpGroup3.SelectedIndex = 0
        DrpMN1.SelectedIndex = 0
        DrpMN2.SelectedIndex = 0
    End Sub
End Class