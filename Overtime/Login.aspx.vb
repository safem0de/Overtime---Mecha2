Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles BtnLogin.Click
        Dim SQLLoginPass = "SELECT 'True'
          FROM [Manpower_Mecha2].[dbo].[User_Login]
          WHERE [Password] = '" & StandardFunction.Encrypt(TxtPassword.Text) & "'
          AND [EmpNo] = '" & TxtEmpNo.Text.ToUpper() & "'"

        Dim SQLLoginRFID = "SELECT 'True'
          FROM [Manpower_Mecha2].[dbo].[User_Login]
          WHERE [Encrypt_RFID] = '" & StandardFunction.Encrypt(TxtPassword.Text) & "'
          AND [EmpNo] = '" & TxtEmpNo.Text.ToUpper() & "'"

        Dim Pass = StandardFunction.getSQLDataString(SQLLoginPass)
        Dim Rfid = StandardFunction.getSQLDataString(SQLLoginRFID)
        Dim SpecialPos = Nothing
        Dim Pos = Nothing
        If Not (String.IsNullOrEmpty(Pass)) Or Not (String.IsNullOrEmpty(Rfid)) Then
            Session("CanAccess") = True
            Session("User") = TxtEmpNo.Text.ToUpper()
            Session.Timeout = 30

            'ต่อให้เป็น อะไรก็ตาม ถ้ามี Authorize ก็จะ Change to Leader
            SpecialPos = "
                DECLARE @Emp Varchar(5);
                SET @Emp = '" & Session("User") & "';

                IF (SELECT 'TRUE'
	                FROM [Manpower_Mecha2].[dbo].[Authorize_Process]
	                WHERE [EmpNo] = @Emp
	                GROUP BY [EmpNo]) = 'TRUE'
	                SELECT 'TRUE' as [Position]
                ELSE
	                SELECT 'FALSE' as [Position]
            "

            Pos = "
                SELECT [Position]
                  FROM [Manpower_Mecha2].[dbo].[Emp_Master]
                  WHERE [EmpNo] = '" & Session("User") & "'
                  AND [Status] = 'ACTIVE'"
            Session("Position") = StandardFunction.getSQLDataString(Pos)

            If CBool(StandardFunction.getSQLDataString(SpecialPos)) _
                And Not (Session("Position") = "STAFF" Or Session("Position") = "CLERK") Then
                Session("Position") = "LEADER"
            End If

        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('รหัสผ่านไม่ถูกต้อง');", True)
            Exit Sub
        End If

        'TxtTest.Text += CStr(Session("CanAccess"))
        'TxtTest.Text += Session("User")
        'TxtTest.Text += Session("Position")

        'TxtTest.Text += SpecialPos
        'TxtTest.Text += Pos

        Response.Redirect("~/")
    End Sub
End Class