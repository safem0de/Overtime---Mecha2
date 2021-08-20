<%@ Page Title="ลงทะเบียน" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Register.aspx.vb" Inherits="Overtime.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>ลงทะเบียน (Register)</h3>
    <div class="container">
        <div class="row">
            <div class="col-6">
                <div class="form-group m-2">
                    <asp:Label ID="Regist_EmpNo" runat="server" Text="รหัสพนักงาน"></asp:Label>
                    <asp:TextBox ID="Txt_Regist_EmpNo" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group m-2">
                    <asp:Label ID="Regist_Password" runat="server" Text="รหัสผ่าน"></asp:Label>
                    <asp:TextBox ID="Txt_Regist_Password" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                </div>
                <div class="form-group m-2">
                    <asp:Label ID="Regist_Conf_Password" runat="server" Text="ยืนยันรหัสผ่าน"></asp:Label>
                    <asp:TextBox ID="Txt_Regist_Conf_Password" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                </div>
                <div class="form-group m-2">
                    <asp:Label ID="Rfid_card" runat="server" Text="แตะบัตรพนักงาน"></asp:Label>
                    <asp:TextBox ID="TxtRfid" runat="server" CssClass="form-control" TextMode="Password" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                </div>

                <asp:Button ID="BtnRegister" runat="server" Text="ลงทะเบียน | Register" CssClass="btn btn-success" />
            </div>
        </div>
    </div>
</asp:Content>
