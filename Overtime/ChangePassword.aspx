<%@ Page Title="เปลี่ยนรหัสผ่าน" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ChangePassword.aspx.vb" Inherits="Overtime.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>เปลี่ยนรหัสผ่าน (Change Password)</h3>
    <div class="container">
        <div class="row">
            <div class="col-6">
                <div class="form-group">
                    <asp:Label ID="Change_EmpNo" runat="server" Text="รหัสพนักงาน"></asp:Label>
                    <asp:TextBox ID="TxtEmpNo" runat="server" CssClass="form-control" placeholder="Employee No"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="Change_Password_Old" runat="server" Text="รหัสผ่านเดิม"></asp:Label>
                    <asp:TextBox ID="TxtPasswordOld" runat="server" CssClass="form-control" TextMode="Password" placeholder="Old Password"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="Change_Password_New" runat="server" Text="รหัสผ่านใหม่"></asp:Label>
                    <asp:TextBox ID="TxtPasswordNew" runat="server" CssClass="form-control" TextMode="Password" placeholder="New Password"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="Change_Password_Conf" runat="server" Text="ยืนยันรหัสผ่านใหม่"></asp:Label>
                    <asp:TextBox ID="TxtPasswordConf" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirm New Password"></asp:TextBox>
                </div>
                <asp:Button ID="BtnChangePassword" runat="server" Text="เปลี่ยนรหัสผ่าน" CssClass="btn btn-success" />
                <asp:Button ID="BtnClear" runat="server" Text="ล้างข้อมูล | Clear" CssClass="btn btn-danger"/>
                <br />
                <br />
                <a href="~/ForgotPassword" runat="server">ลืมรหัสผ่าน(Forgot Password)</a><br />
                <a href="~/Login" runat="server">กลับไปหน้าล้อคอิน(Back to Login)</a>
            </div>
        </div>
    </div>
</asp:Content>
