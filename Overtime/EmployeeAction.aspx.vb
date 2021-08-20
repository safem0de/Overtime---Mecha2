Public Class EmployeeAction
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadPage()
    End Sub

    Sub LoadPage()
        Dim SqlShowList = "
            SELECT 
	            Convert(Varchar(10),[Action_Date],23) as [Action Date]
                ,t1.[EmpNo]
                ,t1.[Name]
                ,t1.[Surname]
	            ,t3.[Shift] as [Old Shift]
                ,t1.[Shift] as [New Shift]
	            ,t3.[Process] as [Old Process]
                ,t1.[Process] as [New Process]
	            ,t2.[Req_By]
            FROM [Manpower_Mecha2].[dbo].[Emp_Action_Detail] as t1

            LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Action] as t2
            ON t1.[EAR_Id] = t2.[Req_EAR_Id]

            LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t3
            ON t1.[EmpNo] = t3.[EmpNo]

            WHERE 
			NOT t1.[Shift] = t3.[Shift]
			OR
			NOT t1.[Process] = t3.[Process]
        "
        StandardFunction.fillDataTableToDataGrid(GrdEmpUpdate, SqlShowList, "")
    End Sub

End Class