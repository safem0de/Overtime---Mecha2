<%@ Page Title="เข้าสู่ระบบ" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="Overtime.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>เข้าสู่ระบบ (Login)</h3>

    <div class="container">
        <div class="row">
            <div class="col-6">
                <div class="form-group">
                    <asp:Label ID="Login_EmpNo" runat="server" Text="รหัสพนักงาน"></asp:Label>
                    <asp:TextBox ID="TxtEmpNo" runat="server" CssClass="form-control" placeholder="Employee No"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="Login_Password" runat="server" Text="รหัสผ่าน"></asp:Label>
                    <asp:TextBox ID="TxtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                </div>
                <asp:Button ID="BtnLogin" runat="server" Text="เข้าสู่ระบบ | Login" CssClass="btn btn-success" />
                <asp:Button ID="BtnClear" runat="server" Text="ล้างข้อมูล | Clear" CssClass="btn btn-danger"/>
                <br />
                <br />
                <a href="~/ChangePassword" runat="server">เปลี่ยนรหัสผ่าน(Change Password)</a><br />
                <a href="~/ForgotPassword" runat="server">ลืมรหัสผ่าน(Forgot Password)</a>
            </div>
        </div>
    </div>

    <%--<asp:TextBox ID="TxtTest" runat="server" TextMode="MultiLine"></asp:TextBox>--%>
</asp:Content>
