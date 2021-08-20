<%@ Page Title="ลีดเดอร์คอนเฟิร์ม" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="LeaderConfirm.aspx.vb" Inherits="Overtime.LeaderConfirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <button class="btn btn-secondary" type="button" data-toggle="collapse" data-target="#multiCollapseExample1" aria-expanded="false" aria-controls="multiCollapseExample1">ซ่อน/แสดง (Wait Confirm)</button>
        <button class="btn btn-secondary" type="button" data-toggle="collapse" data-target="#multiCollapseExample2" aria-expanded="false" aria-controls="multiCollapseExample2">ซ่อน/แสดง (Confirmed)</button>
        <%--<button class="btn btn-primary" type="button" data-toggle="collapse" data-target=".multi-collapse" aria-expanded="false" aria-controls="multiCollapseExample1 multiCollapseExample2">Toggle both elements</button>--%>
    </p>
    <%--<div class="row">--%>
        <div class="col-12">
            <div id="multiCollapseExample1">
                <div class="card">
                    <div class="card-header">
                        รายชื่อคนทำ Overtime ที่รอ Confirm (Wait Confirm)
                        <div class="form-group row float-right">
                            วันที่
                            <div class="col-auto">
                                <asp:TextBox ID="TxtUnConfDate" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <asp:Button ID="BtnConfUnConfDate" runat="server" Text="ยืนยันรายชื่อทั้งหมด" class="btn btn-success" OnClientClick="return confirm('ยืนยันรายชื่อทั้งหมด');"/>
                        </div>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="GrdWaitConf" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" CssClass="col-12 text-center">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <Columns>
                                <asp:ButtonField ButtonType="Button" CommandName="Select" Text="เลือก" ControlStyle-CssClass="btn btn-secondary"/>
                            </Columns>
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="Gray" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#383838" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="col-12">
            <div id="multiCollapseExample2">
                <div class="card">
                    <div class="card-header">
                        รายชื่อคนทำ Overtime ที่ Confirm เรียบร้อยแล้ว (Confirmed)
                        <div class="form-group row float-right">
                            วันที่
                            <div class="col-auto">
                                <asp:TextBox ID="TxtConfDate" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="GrdConfirm" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="1" ForeColor="Black" GridLines="Vertical" CssClass="col-12 text-center">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <Columns>
                                <asp:ButtonField ButtonType="Button" CommandName="Select" Text="เลือก" ControlStyle-CssClass="btn btn-secondary"/>
                            </Columns>
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#808080" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#383838" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    <%--</div>--%>

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
                    <div class="form-group row">
                        <label class="col-sm-auto col-form-label">ชั่วโมงการทำงาน : </label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="TxtEditHour" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnUnConfSave" runat="server" Text="บันทึกการแก้ไข" class="btn btn-success" />
                    <asp:Button ID="BtnUnConfDel" runat="server" Text="ลบ" class="btn btn-danger"/>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="exampleModal2" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">แก้ไข/ลบ โอทีรีเควส (Edit/Delete)</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:Label ID="LblInfo" runat="server" Text=""></asp:Label>
                    <br />
                    <div class="form-group row">
                        <asp:Label ID="LblShift" runat="server" Text="กะ" class="col-3 col-form-label"></asp:Label>
                        <div class="col-6">
                            <asp:DropDownList ID="DrpShift" runat="server" class="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label ID="LblHours" runat="server" Text="จำนวนชั่วโมง" class="col-3 col-form-label"></asp:Label>
                        <div class="col-6">
                            <asp:DropDownList ID="DrpHours" runat="server" class="form-control">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>1.5</asp:ListItem>
                                <asp:ListItem>2.5</asp:ListItem>
                                <asp:ListItem>3.5</asp:ListItem>
                                <asp:ListItem>5.5</asp:ListItem>
                                <asp:ListItem>6.5</asp:ListItem>
                                <asp:ListItem>7.5</asp:ListItem>
                                <asp:ListItem>10.5</asp:ListItem>
                                <asp:ListItem>14.5</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label ID="LblReason" runat="server" Text="เหตุผล" class="col-3 col-form-label"></asp:Label>
                        <div class="col-6">
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
                <div class="modal-footer">
                    <asp:Button ID="BtnUpdate" runat="server" Text="อัพเดท" class="btn btn-primary"/>
                    <asp:Button ID="BtnConfSave" runat="server" Text="แก้ไข" class="btn btn-warning" />
                    <asp:Button ID="BtnConfDel" runat="server" Text="ลบ" class="btn btn-danger"/>
                </div>
            </div>
        </div>
    </div>
    <asp:TextBox ID="TextTest" runat="server" TextMode="MultiLine"></asp:TextBox>
    <%--<asp:GridView ID="GrdTest" runat="server"></asp:GridView>--%>
</asp:Content>
