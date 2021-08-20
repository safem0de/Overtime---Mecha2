Public Class ForgotPassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Sub Clearform()
        TxtEmpNo.Text = Nothing
        TxtRfid.Text = Nothing
    End Sub

    Protected Sub BtnSeePassword_Click(sender As Object, e As EventArgs) Handles BtnSeePassword.Click
        Dim SqlForgotPass = "
        SELECT [Password]
          FROM [Manpower_Mecha2].[dbo].[User_Login]
          WHERE [EmpNo] = '" & TxtEmpNo.Text & "'
          AND [RFID] = '" & TxtRfid.Text & "'
        "
        Dim Password = StandardFunction.getSQLDataString(SqlForgotPass)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ForgotPassword", "alert('รหัสผ่านของคุณคือ : " & StandardFunction.Decrypt(Password) & "');", True)
    End Sub

    Protected Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        Clearform()
    End Sub
End Class