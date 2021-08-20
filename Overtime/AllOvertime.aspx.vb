Public Class AllOvertime
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadPage()
        End If
    End Sub

    Sub LoadPage()
        'Dim SqlAll = "
        'SELECT Convert(varchar(10),[Date],23) as [Date]
        '      ,[EmpNo]
        '      ,[Name]
        '      ,[Surname]
        '      ,[Shift]
        '      ,[Section]
        '      ,[Process]
        '      ,[Hours]
        '      ,[Accum_Hours]
        '      ,[Status]
        '      ,[Reason]
        '  FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
        '"
        Dim SqlAll = "
        SELECT
        IsNull(Convert(varchar,[Date_Req],20),'-') as [Date]
        ,Convert(varchar(10),[Date],23) as [WorkDate]
                      ,[EmpNo]
                      ,[Name]
                      ,[Surname]
                      ,[Shift]
                      ,t1.[Process]
                      ,[Hours]
                      ,[Accum_Hours] as [Accum Hours]
			          ,IsNull(t2.[Request_By],'-') as [Confirm By]
                      ,t1.[Reason]
        FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1
        LEFT JOIN[Manpower_Mecha2].[dbo].[Request_OT] as t2
        ON t1.[Req_Id] = t2.[Request_Id]
        ORDER BY [WorkDate] desc
        "

        StandardFunction.fillDataTableToDataGrid(GrdAll, SqlAll, "")
    End Sub
End Class