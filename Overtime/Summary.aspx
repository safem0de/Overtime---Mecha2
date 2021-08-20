<%@ Page Title="สรุปยอดโอ-ที" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Summary.aspx.vb" Inherits="Overtime.Summary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        td
        {
            white-space: nowrap;
        }
    </style>
    <h3>จำนวน Overtime(ชั่วโมง)ปัจจุบัน by Section</h3>
    <asp:GridView ID="GrdSection" runat="server" CellPadding="1" ForeColor="#333333" GridLines="None" CssClass="col-12 text-center">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
        <EditRowStyle BackColor="#999999"></EditRowStyle>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></FooterStyle>
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></HeaderStyle>
        <PagerStyle HorizontalAlign="Center" BackColor="#284775" ForeColor="White"></PagerStyle>
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333"></RowStyle>
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>
        <SortedAscendingCellStyle BackColor="#E9E7E2"></SortedAscendingCellStyle>
        <SortedAscendingHeaderStyle BackColor="#506C8C"></SortedAscendingHeaderStyle>
        <SortedDescendingCellStyle BackColor="#FFFDF8"></SortedDescendingCellStyle>
        <SortedDescendingHeaderStyle BackColor="#6F8DAE"></SortedDescendingHeaderStyle>
    </asp:GridView>
    <br />
    <h3>จำนวน Overtime(ชั่วโมง)ปัจจุบัน by Process</h3>
    <asp:GridView ID="GrdProcess" runat="server" CellPadding="1" ForeColor="#333333" GridLines="None" CssClass="col-12 text-center">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
    <br />
    <h3>จำนวน Manpower by Process</h3>
    <asp:GridView ID="GrdManProcess" runat="server" CellPadding="1" ForeColor="#333333" GridLines="None" CssClass="col-12 text-center">
        <AlternatingRowStyle BackColor="White" />
        <EditRowStyle BackColor="#7C6F57" />
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#E3EAEB" />
        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F8FAFA" />
        <SortedAscendingHeaderStyle BackColor="#246B61" />
        <SortedDescendingCellStyle BackColor="#D4DFE1" />
        <SortedDescendingHeaderStyle BackColor="#15524A" />
    </asp:GridView>
    <br />
    <h3>จำนวน Overtime(ชั่วโมง)เดือนถัดไป by Section</h3>
    <asp:GridView ID="GrdSectionNext" runat="server" CellPadding="1" ForeColor="#333333" GridLines="None" CssClass="col-12 text-center">
        <AlternatingRowStyle BackColor="White" />
        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <SortedAscendingCellStyle BackColor="#FDF5AC" />
        <SortedAscendingHeaderStyle BackColor="#4D0000" />
        <SortedDescendingCellStyle BackColor="#FCF6C0" />
        <SortedDescendingHeaderStyle BackColor="#820000" />
    </asp:GridView>
    <br />
    <h3>จำนวน Overtime(ชั่วโมง)เดือนถัดไป by Process</h3>
    <asp:GridView ID="GrdProcessNext" runat="server" CellPadding="1" ForeColor="#333333" GridLines="None" CssClass="col-12 text-center">
        <AlternatingRowStyle BackColor="White" />
        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <SortedAscendingCellStyle BackColor="#FDF5AC" />
        <SortedAscendingHeaderStyle BackColor="#4D0000" />
        <SortedDescendingCellStyle BackColor="#FCF6C0" />
        <SortedDescendingHeaderStyle BackColor="#820000" />
    </asp:GridView>
</asp:Content>
