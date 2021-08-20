<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="EmployeeActionRequest.aspx.vb" Inherits="Overtime.EmployeeActionRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">

        <div class="col-6">
            <asp:RadioButtonList ID="RadioBtnShiftProcess" runat="server" CssClass="form-check">
                <asp:ListItem Value="1" Selected="True">เปลี่ยนกะ (Change Shift)</asp:ListItem>
                <asp:ListItem Value="2">เปลี่ยนกระบวนการ (Change Process)</asp:ListItem>
                <asp:ListItem Value="3">เปลี่ยนกะ และ กระบวนการ (Change Shift &amp; Process)</asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <div class="col-6">
            <div class="input-group">
                <div class="input-group-prepend">
                    <span class="input-group-text">Date&emsp;</span>
                </div>
                <asp:TextBox ID="TxtActionDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="input-group mt-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">Shift&emsp;</span>
                </div>
                <asp:DropDownList ID="DrpShiftOld" runat="server" class="form-control"></asp:DropDownList>
                <div class="input-group-prepend">
                    <span class="input-group-text">To</span>
                </div>
                <asp:DropDownList ID="DrpShiftNew" runat="server" class="form-control"></asp:DropDownList>
            </div>
            <div class="input-group mt-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">Process</span>
                </div>
                <asp:DropDownList ID="DrpProcessOld" runat="server" class="form-control"></asp:DropDownList>
                <div class="input-group-prepend">
                    <span class="input-group-text">To</span>
                </div>
                <asp:DropDownList ID="DrpProcessNew" runat="server" class="form-control"></asp:DropDownList>
            </div>
        </div>

    </div>

    <div class="col-6 mt-3">
        <div class="input-group mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text"><i class="fa fa-search"></i>&emsp;Search (ค้นหา)</span>
            </div>
            <asp:TextBox ID="TxtSearch" runat="server" placeholder="รหัสพนักงาน, ชื่อ หรือ กระบวนการ" class="form-control"></asp:TextBox>
            <div class="input-group-append">
                <asp:Button ID="BtnClear" runat="server" Text="ล้างรายการ" class="btn btn-danger" />
            </div>
        </div>
    </div>

    <div class="col-6">
        <asp:GridView ID="GrdSearch" runat="server" CellPadding="1" ForeColor="#333333" GridLines="None" CssClass="col-12 text-center" AllowPaging="True" PageSize="4">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="Select" Text="เพิ่ม" ControlStyle-CssClass="btn btn-outline-info">
                    <ControlStyle CssClass="btn btn-outline-info"></ControlStyle>
                </asp:ButtonField>
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
    </div>

    <div class="card mt-3">
        <div class="card-header">
            รายการที่ต้องการเปลี่ยนแปลง
        </div>
        <div class="card-body">
            <asp:GridView ID="GrdAllChange" runat="server" CellPadding="1" ForeColor="#333333" GridLines="None" CssClass="col-12 text-center" AllowPaging="True" PageSize="20">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:ButtonField ButtonType="Button" CommandName="Delete" Text="ลบ" ControlStyle-CssClass="btn btn-outline-danger"/>
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Left" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
        </div>
        <div class="card-footer">
            <div class="text-center">
                <asp:Button ID="BtnConfirm" runat="server" Text="ยืนยันการเปลี่ยนแปลง" class="btn btn-success" />
            </div>
        </div>
    </div>

    <asp:TextBox ID="TxtTest" runat="server" TextMode="MultiLine"></asp:TextBox>
</asp:Content>
