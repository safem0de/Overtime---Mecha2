<%@ Page Title="เพิ่มบริษัท" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AddCompany.aspx.vb" Inherits="Overtime.AddCompany" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h4>เพิ่มรายชื่อบริษัท/แผนก</h4>
    <div class="row">
        <div class="col-8">
            <div class="form-group row">
                <label class="col-sm-4 col-form-label">ชื่อบริษัท (Company's Name)</label>
                <div class="col-sm-8">
                    <asp:TextBox ID="TxtCompany" runat="server" class="form-control"></asp:TextBox>
                </div>
            </div>

            <div>
                <asp:Button ID="BtnCancle" runat="server" Text="ล้างรายการ | Clear" class="btn btn-danger" />
                &nbsp;
                <asp:Button ID="BtnEdit" runat="server" Text="แก้ไข | Edit Company"  class="btn btn-info"/>
                &nbsp;
                <asp:Button ID="BtnAdd" runat="server" Text="เพิ่มชื่อ | Add Company" class="btn btn-success" />
            </div>

        </div>
        <div class="col-4">
            <div class="card">
                <div class="card-header">
                    รายชื่อบริษัท/แผนก (Company List)
                </div>
                <div class="card-body">
                    <asp:GridView ID="GrdCompany" runat="server" CellPadding="3" ForeColor="#333333" GridLines="Vertical" CssClass="col-12 text-center">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:ButtonField ButtonType="Button" CommandName="Select" Text="เลือก" ControlStyle-CssClass="btn btn-info"/>
                            <asp:ButtonField ButtonType="Button" CommandName="Delete" Text="ลบ" ControlStyle-CssClass="btn btn-danger"/>
                        </Columns>
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
                </div>
            </div>
        </div>
    </div>
</asp:Content>
