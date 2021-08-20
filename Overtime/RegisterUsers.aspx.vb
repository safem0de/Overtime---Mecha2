Public Class RegisterUsers
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadPage()
    End Sub

    Sub LoadPage()
        Dim Sql = "
            SELECT
	            t2.[EmpNo],
	            t2.[Name],
	            t2.[Surname],
                t2.[Process],
	            t2.[Position]
              FROM [Manpower_Mecha2].[dbo].[User_Login] as t1

            LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master] as t2
            ON t1.[EmpNo] = t2.[EmpNo]

            WHERE Not t2.[EmpNo] Is Null

            ORDER BY t2.[EmpNo] asc
        "
        StandardFunction.fillDataToDataGrid(GrdRegistUser, Sql)
    End Sub

End Class