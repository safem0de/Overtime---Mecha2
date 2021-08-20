<%@ Page Title="ลีดเดอร์รีเควสโอ-ที" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="LeaderRequest.aspx.vb" Inherits="Overtime.LeaderRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <marquee><asp:Label ID="LblMNStatus" runat="server" Text=""></asp:Label></marquee>

    <div class="jumbotron">
        <h5>1. เลือก "วัน" และ "กระบวนการ" <b><u>หรือ</u></b> เลือก "วัน" และ "Preset" ที่ต้องการ</h5>
        <div class="row">
            <div class="col-3">
                <label>วันที่ทำโอ-ที (WorkDate)</label>
                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text" id="basic-addon1"><i class="fa fa-calendar"></i></span>
                    </div>
                    <asp:TextBox ID="TxtWorkDate" runat="server" class="form-control" aria-describedby="basic-addon1" TextMode="Date"></asp:TextBox>
                </div>
            </div>

            <div class="col-3">
                <label>กระบวนการ (Process)</label>
                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text"><i class="fas fa-cogs"></i></span>
                    </div>
                    <asp:DropDownList ID="DrpProcess" runat="server" class="form-control"></asp:DropDownList>
                </div>
            </div>

        </div>
        <br />
        <h5>2. เลือก "กะ", "เวลาทำงาน" และ "เหตุผล" ที่ต้องการ Request <br />
            จากนั้นกด <button class="btn btn-outline-info">Select</button>
            เลือกรายชื่อที่ตารางรายชื่อทั้งหมด</h5>
        <div class="row">
            <div class="col-3">
                <label>กะการทำงาน (Shift)</label>
                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text"><i class="fas fa-user-cog"></i></span>
                    </div>
                    <asp:DropDownList ID="DrpShift" runat="server" class="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-3">
                <label>ชั่วโมงการทำงาน (Hours)</label>
                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text"><i class="fas fa-clock"></i></span>
                    </div>
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
            <div class="col-3">
                <label>เหตุผล (Reason)</label>
                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text"><i class="fa fa-question"></i></span>
                    </div>
                    <asp:DropDownList ID="DrpReason" runat="server" class="form-control">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>รันเครื่อง</asp:ListItem>
                        <asp:ListItem>เซทเครื่องจักร</asp:ListItem>
                        <asp:ListItem>แทนคนลา</asp:ListItem>
                        <asp:ListItem>Sort งาน</asp:ListItem>
                        <asp:ListItem>เช็คอินเวนฯ</asp:ListItem>
                        <asp:ListItem>งานเอกสาร</asp:ListItem>
                        <asp:ListItem>QC เซตอัพ</asp:ListItem>
                        <asp:ListItem>QC พาสงาน</asp:ListItem>
                        <asp:ListItem>QC เดินไลน์</asp:ListItem>
                        <asp:ListItem>QC เกจ,เครื่องมือวัด</asp:ListItem>
                        <asp:ListItem>จ่าย Material</asp:ListItem>
                        <asp:ListItem>ซ่อมเครื่องจักร</asp:ListItem>
                        <asp:ListItem>อื่นๆ</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            </div>
            <br />
            <h5>3. เลือก "Preset" ที่ต้องการ</h5>
            <div class="row">

                <div class="col-3">
                    <label>เลือกจาก Preset (from Preset)</label>
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fas fa-save"></i></span>
                        </div>
                        <asp:DropDownList ID="DrpPreset" runat="server" class="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-3">
                    <br />
                    <asp:CheckBox ID="ChkboxDel" runat="server" />
                    <asp:Button ID="BtnDelPreset" runat="server" Text="ลบ Preset | Delete Preset" class="btn btn-danger mt-2" 
                        OnClientClick="return confirm('ยืนยันการลบ Preset');" />
                </div>

            </div>
    </div>

    <div class="row">
        <div class="col-5">
            <div class="card">
                <div class="card-header">
                    รายชื่อพนักงาน
                </div>
                <div class="card-body">
                    <asp:GridView ID="GrdEmpSelect" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" CssClass="col-12 text-center">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                        <Columns>
                            <asp:ButtonField ButtonType="Button" CommandName="Select" Text="เลือก" ControlStyle-CssClass="btn btn-outline-info"/>
                        </Columns>
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
        <div class="col-7">
            <div class="card">
                <div class="card-header">
                    รายชื่อคนที่ถูก Request Overtime
                </div>
                <div class="card-body">
                    <asp:GridView ID="GrdEmpConfirm" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" CssClass="col-12 text-center">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                        <Columns>
                            <asp:ButtonField ButtonType="Button" CommandName="Select" Text="ลบ" ControlStyle-CssClass="btn btn-outline-danger"/>
                        </Columns>
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
    <div class="jumbotron col-4">
        <div class="form-group">
            <label for="exampleInputPassword1">หมายเหตุ (Remark)</label>
            <asp:TextBox ID="TxtRemark" runat="server" class="form-control" TextMode="MultiLine"></asp:TextBox>
        </div>

        <div class="form-check">
            <asp:CheckBox ID="CheckBoxPreset" runat="server" class="form-check-input"/>
            <label>บันทึกเพื่อใช้ซ้ำ (Save as Preset)</label>
        </div>

        <div class="form-group">
            <asp:TextBox ID="TxtPreset" runat="server" class="form-control" Enabled="False"></asp:TextBox>
        </div>

        <div class="form-group">
            <label>รีเควสให้กระบวนการ (Request for Process)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text"><i class="fas fa-cogs"></i></span>
                </div>
                <asp:DropDownList ID="DrpRequestfor" runat="server" class="form-control"></asp:DropDownList>
            </div>
        </div>

        <asp:Button ID="BtnConfirm" runat="server" Text="ยืนยัน | Confirm" class="btn btn-success" />
        <asp:Button ID="BtnClear" runat="server" Text="ล้างฟอร์ม | Clear" class="btn btn-danger" />
    </div>

    <%--Modal--%>
    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">เพิ่มรายการรีเควส (Add Request)</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:Label ID="Label" runat="server" Text="Label"></asp:Label>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnAdd" runat="server" Text="Add" class="btn btn-warning" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <%--TextCheckSQL--%>
    <asp:TextBox ID="TextBoxTest" runat="server" TextMode="MultiLine"></asp:TextBox>
    <asp:GridView ID="GrdStorage" runat="server"></asp:GridView>
</asp:Content>
