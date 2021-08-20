Public Class EditOvertime
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
        End If
    End Sub

    Sub LoadPage()
        Dim SqlAll = "
        SELECT Convert(varchar(10),[Date],23) as [Date]
              ,[EmpNo]
              ,[Name]
              ,[Surname]
              ,[Shift]
              ,[Section]
              ,[Process]
              ,[Hours]
              ,[Accum_Hours]
              ,[Status]
              ,[Reason]
          FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]
          WHERE DATEDIFF(DAY,GETDATE(),[Date]) >= 0
          AND
          DATEDIFF(DAY,GETDATE(),[Date]) <= 1
        "

        StandardFunction.fillDataTableToDataGrid(GrdAll, SqlAll, "")
    End Sub
End Class