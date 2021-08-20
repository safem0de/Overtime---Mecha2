<%@ Page Title="ลืมรหัสผ่าน" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ForgotPassword.aspx.vb" Inherits="Overtime.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>ลืมรหัสผ่าน (Forgot Password)</h3>
    <div class="container">
        <div class="row">
            <div class="col-6">
                <div class="form-group">
                    <asp:Label ID="Change_EmpNo" runat="server" Text="รหัสพนักงาน"></asp:Label>
                    <asp:TextBox ID="TxtEmpNo" runat="server" CssClass="form-control" placeholder="Employee No"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="Rfid" runat="server" Text="โปรดแตะบัตรพนักงาน"></asp:Label>
                    <asp:TextBox ID="TxtRfid" runat="server" CssClass="form-control" TextMode="Password" placeholder="Please Input Employee's Card" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                </div>
                <asp:Button ID="BtnSeePassword" runat="server" Text="ดูรหัสผ่าน" CssClass="btn btn-success" />
                <asp:Button ID="BtnClear" runat="server" Text="ล้างข้อมูล | Clear" CssClass="btn btn-danger"/>
                <br />
                <br />
                <a href="~/ChangePassword" runat="server">เปลี่ยนรหัสผ่าน(Change Password)</a><br />
                <a href="~/Login" runat="server">กลับไปหน้าล้อคอิน(Back to Login)</a>
            </div>
        </div>
    </div>
</asp:Content>
