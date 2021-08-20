<%@ Page Title="ดาว์นโหลดข้อมูล" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Download.aspx.vb" Inherits="Overtime.Download" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <div class="card-body">
            <div class="row">
                <div class="col-3">
                    
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="basic-addon3">วันที่</span>
                        </div>
                        <asp:TextBox ID="TxtWorkDate" runat="server" TextMode="Date" class="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-3">
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <label class="input-group-text" for="inputGroupSelect01">Group 1</label>
                        </div>
                        <asp:DropDownList ID="DrpGroup1" runat="server" class="custom-select">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>A</asp:ListItem>
                            <asp:ListItem>B</asp:ListItem>
                            <asp:ListItem>C</asp:ListItem>
                            <asp:ListItem>M</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                            <asp:ListItem>H</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <label class="input-group-text" for="inputGroupSelect01">Group 2</label>
                        </div>
                        <asp:DropDownList ID="DrpGroup2" runat="server" class="custom-select">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>A</asp:ListItem>
                            <asp:ListItem>B</asp:ListItem>
                            <asp:ListItem>C</asp:ListItem>
                            <asp:ListItem>M</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                            <asp:ListItem>H</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <label class="input-group-text" for="inputGroupSelect01">Group 3</label>
                        </div>
                        <asp:DropDownList ID="DrpGroup3" runat="server" class="custom-select">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>A</asp:ListItem>
                            <asp:ListItem>B</asp:ListItem>
                            <asp:ListItem>C</asp:ListItem>
                            <asp:ListItem>M</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                            <asp:ListItem>H</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <label class="input-group-text" for="inputGroupSelect01">MN1</label>
                        </div>
                        <asp:DropDownList ID="DrpMN1" runat="server" class="custom-select">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>M</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <label class="input-group-text" for="inputGroupSelect01">MN2</label>
                        </div>
                        <asp:DropDownList ID="DrpMN2" runat="server" class="custom-select">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>M</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-3">
                    <asp:Button ID="BtnDownload" runat="server" Text="ดาวน์โหลด" class="btn btn-primary"/>
                </div>
                
            </div>
            
        </div>
    </div>
</asp:Content>
