Imports System.Data.SqlClient

Public Class Register
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub BtnRegister_Click(sender As Object, e As EventArgs) Handles BtnRegister.Click
        Dim check As Boolean = True
        Dim alert As New StringBuilder("Pls input")
        Dim CheckDuplicate As String = "
        SELECT [EmpNo]
        FROM [Manpower_Mecha2].[dbo].[User_Login]
        WHERE [EmpNo] = '" & Txt_Regist_EmpNo.Text & "'
        "
        If Not (StandardFunction.getSQLDataString(CheckDuplicate)) = Nothing Then
            alert.Append(" ลงทะเบียนซ้ำ,")
            check = False
        End If

        If Txt_Regist_EmpNo.Text = "" Or (Len(Txt_Regist_EmpNo.Text) > 5 And Len(Txt_Regist_EmpNo.Text) < 4) Then
            alert.Append(" รหัสพนักงาน,")
            check = False
        End If

        If Len(Txt_Regist_Password.Text) < 4 Then
            alert.Append(" รหัสผ่านอย่างน้อย 4 หลัก,")
            check = False
        End If

        If Not Txt_Regist_Password.Text.Equals(Txt_Regist_Conf_Password.Text) Then
            alert.Append(" ยืนยันรหัสผ่านไม่ถูกต้อง,")
            check = False
        End If

        If TxtRfid.Text Is Nothing Or Len(TxtRfid.Text) <> 10 Then
            alert.Append(" บัตรพนักงาน,")
            check = False
        End If

        If check = False Then
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & alert.ToString.Substring(0, alert.Length - 1) & "');", True)
        Else
            Dim con As New SqlConnection
            Dim command As SqlCommand

            Dim SqlRegister = "INSERT INTO [Manpower_Mecha2].[dbo].[User_Login]
           ([EmpNo]
           ,[Password]
           ,[RFID]
           ,[Encrypt_RFID])
     VALUES
           ('" & Txt_Regist_EmpNo.Text & "'
           ,'" & StandardFunction.Encrypt(Txt_Regist_Password.Text) & "'
           ,'" & TxtRfid.Text & "'
           ,'" & StandardFunction.Encrypt(TxtRfid.Text) & "')"

            Try
                ' พยายามลองทำคำสั่งในนี้
                con.ConnectionString = StandardFunction.connectionString
                con.Open()
                command = New SqlCommand(SqlRegister, con)
                command.ExecuteNonQuery()
            Catch ex As Exception
                ' Error/fail แล้วทำตรงนี้
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลผิดพลาด');", True)
            Finally
                'ไม่ว่าจะ Error หรือ OK ก็ต้องเข้า loop นี้
                Clearform()
                con.Close()
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('ลงทะเบียนสำเร็จ (Completed)');", True)
            End Try
        End If
    End Sub

    Sub Clearform()
        Txt_Regist_EmpNo.Text = ""
    End Sub
End Class