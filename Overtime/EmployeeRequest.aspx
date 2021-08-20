<%@ Page Title="พนักงานรีเควสโอ-ที" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="EmployeeRequest.aspx.vb" Inherits="Overtime.EmployeeRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-4">
            <div class="form-group row">
                <label class="col-sm-4 col-form-label">รหัสพนักงาน (Employee)</label>
                <div class="col-sm-8">
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <asp:TextBox ID="TxtEmpNo" runat="server" class="form-control" disabled="true"></asp:TextBox>
                        </div>
                        <div class="input-group-append">
                            <asp:Button ID="BtnInfo" runat="server" Text="..." class="btn btn-info"/>
                        </div>
                    </div>
                    <%--<asp:TextBox ID="TxtEmpNo" runat="server" class="form-control" disabled="true"></asp:TextBox>--%>
                    <%--<asp:Button ID="BtnInfo" runat="server" Text="..." class="btn btn-info"/>--%>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-sm-4 col-form-label">วันที่ (Date)</label>
                <div class="col-sm-8">
                    <asp:TextBox ID="TxtWorkDate" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-sm-4 col-form-label">กะ (Shift)</label>
                <div class="col-sm-8">
                    <asp:DropDownList ID="DrpShift" runat="server" class="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-sm-4 col-form-label">ชั่วโมง (Hours)</label>
                <div class="col-sm-8">
                    <asp:DropDownList ID="DrpHours" runat="server" class="form-control">
                        <asp:ListItem></asp:ListItem>
                    <asp:ListItem>1.5</asp:ListItem>
                    <asp:ListItem>2.5</asp:ListItem>
                    <asp:ListItem>3.5</asp:ListItem>
                    <asp:ListItem>4.5</asp:ListItem>
                    <asp:ListItem>5.5</asp:ListItem>
                    <asp:ListItem>6.5</asp:ListItem>
                    <asp:ListItem>7.5</asp:ListItem>
                    <asp:ListItem>10.5</asp:ListItem>
                    <asp:ListItem>14.5</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-sm-4 col-form-label">หน่วยงาน (Process)</label>
                <div class="col-sm-8">
                    <asp:DropDownList ID="DrpProcess" runat="server" class="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-sm-4 col-form-label">เหตุผล (Reason)</label>
                <div class="col-sm-8">
                    <asp:DropDownList ID="DrpReason" runat="server" class="form-control">
                        <asp:ListItem></asp:ListItem>
                    <asp:ListItem>รันเครื่อง</asp:ListItem>
                    <asp:ListItem>เซทเครื่องจักร</asp:ListItem>
                    <asp:ListItem>แทนคนลา</asp:ListItem>
                    <asp:ListItem>Sort งาน</asp:ListItem>
                    <asp:ListItem>เช็คอินเวนฯ</asp:ListItem>
                    <asp:ListItem>งานเอกสาร</asp:ListItem>
                    <asp:ListItem>อื่นๆ</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

                <div class="d-flex justify-content-end">
                    <asp:Button ID="BtnCancle" runat="server" Text="ล้างรายการ | Clear" class="btn btn-danger" />
                    &nbsp;
                    <asp:Button ID="BtnConfirm" runat="server" Text="ยืนยัน | Confirm" class="btn btn-success" />
                </div>

        </div>
        <div class="col-8">
            <div class="card">
                <div class="card-header">
                    ประวัติการทำงานล่วงเวลา (Overtime) ที่<b><u>รอ</u>การยืนยัน</b>จากหัวหน้างาน
                </div>
                <div class="card-body">
                    <asp:GridView ID="GrdWaitConfirm" runat="server" CellPadding="1" ForeColor="#333333" GridLines="Vertical" CssClass="col-12 text-center" >
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:ButtonField ButtonType="Button" CommandName="Delete" Text="ลบ" ControlStyle-CssClass="btn btn-danger" />
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
            <br />
            <div class="card">
                <div class="card-header">
                    ประวัติการทำงานล่วงเวลา (Overtime) ที่<b>ได้รับการยืนยัน</b>จากหัวหน้างาน
                </div>
                <div class="card-body">
                    <asp:GridView ID="GrdConfrimed" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" CssClass="col-12 text-center">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#000065" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">ข้อมูลส่วนตัว (Personal Information)</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:Label ID="LblName" runat="server" Text="ชื่อ-นามสกุล : "></asp:Label><br />
                    <asp:Label ID="LblEmpNo" runat="server" Text="รหัสพนักงาน : "></asp:Label><br />
                    <asp:Label ID="lblOCurrWeek" runat="server" Text="จำนวนโอทีในสัปดาห์นี้ : "></asp:Label><br />
                    <asp:Label ID="lblOCurrMonth" runat="server" Text="จำนวนโอทีในเดือนนี้ : "></asp:Label><br />
                    <br />
                    <asp:GridView ID="GrdPersonal" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" CssClass="col-12 text-center">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#000065" />
                    </asp:GridView>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">ปิด</button>
                </div>
            </div>
        </div>
    </div>
    <%--<asp:TextBox ID="TextTest" runat="server" TextMode="MultiLine"></asp:TextBox>--%>
    <%--<asp:GridView ID="GrdTest" runat="server"></asp:GridView>--%>
    
</asp:Content>
