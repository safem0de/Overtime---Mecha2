<%@ Page Title="รายชื่อพนักงานทั้งหมด" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Manpower.aspx.vb" Inherits="Overtime.Manpower" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-3">
            <label>รหัสพนักงาน (Employee No.)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text" id="basic-addon1"><i class="far fa-id-card"></i></span>
                </div>
                <asp:TextBox ID="TxtEmpNo" runat="server" class="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-3">
            <label>ชื่อ (Name)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text"><i class="fas fa-user-tie"></i></span>
                </div>
                <asp:TextBox ID="TxtName" runat="server" class="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-3">
            <label>นามสกุล (Surname)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text"><i class="fas fa-user-tie"></i></span>
                </div>
                <asp:TextBox ID="TxtSurName" runat="server" class="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-3">
            <label>เพศ (Gender)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text"><i class="fas fa-venus-mars"></i></span>
                </div>
                <asp:DropDownList ID="DrpGender" runat="server" class="form-control">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem>MALE (ชาย)</asp:ListItem>
                    <asp:ListItem>FEMALE (หญิง)</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-3">
            <label>สถานะ (Status)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text"><i class="fas fa-check-circle"></i></span>
                </div>
                <asp:DropDownList ID="DrpStatus" runat="server" class="form-control">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem>ACTIVE</asp:ListItem>
                    <asp:ListItem>IN-ACTIVE</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-3">
            <label>ส่วนงาน (Section)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text"><i class="fas fa-user-cog"></i></span>
                </div>
                <asp:DropDownList ID="DrpSection" runat="server" class="form-control"></asp:DropDownList>
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
        <div class="col-3">
            <label>กะการทำงาน (Shift)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text"><i class="fas fa-user-clock"></i></span>
                </div>
                <asp:DropDownList ID="DrpShift" runat="server" class="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="col-3">
            <label>บริษัท (Company)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text"><i class="fas fa-store"></i></span>
                </div>
                <asp:DropDownList ID="DrpCompany" runat="server" class="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="col-3">
            <label>หน้าที่การทำงาน (Position)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text"><i class="fas fa-graduation-cap"></i></span>
                </div>
                <asp:DropDownList ID="DrpPosition" runat="server" class="form-control">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-6 justify-content-end">
            <br />
            <asp:Button ID="BtnAdd" runat="server" Text="เพิ่มรายการ" class="btn btn-primary mt-2"/>
            &nbsp;<asp:Button ID="BtnEdit" runat="server" Text="แก้ไขรายการ" class="btn btn-warning mt-2"/>
            &nbsp;<asp:Button ID="BtnClear" runat="server" Text="ล้างฟอร์ม" class="btn btn-danger mt-2"/>
            &nbsp;<asp:Button ID="BtnSearch" runat="server" Text="ค้นหารายการ" class="btn btn-outline-success mt-2"/>
        </div>
    </div>

    <div class="card">
        <div class="card-header">
            <div class="row">
                <div class="col-2"><b>รายชื่อ</b></div>

                <div class="input-group mb-3 mt-3 col-10 justify-content-end">
                    <asp:FileUpload ID="FileUpload" runat="server" CssClass="border rounded-left py-1" />
                    <div class="input-group-append">
                        <asp:Button ID="BtnSelect" runat="server" Text="Select Sheet" class="btn btn-outline-secondary" />
                        <asp:DropDownList ID="DrpSheet" runat="server"></asp:DropDownList>
                        <asp:Button ID="BtnUpload" runat="server" Text="Upload" class="btn btn-outline-secondary" />
                    </div>
                </div>
            </div>
            
        </div>
        <div class="card-body">
            <asp:GridView ID="GrdEmployee" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="1" GridLines="Vertical" AllowPaging="True" CssClass="col-12 mx-auto text-center" PageSize="15" PagerSettings-PageButtonCount="15">
                <AlternatingRowStyle BackColor="#DCDCDC" />
                <Columns>
                    <asp:ButtonField ButtonType="Button" CommandName="Select" Text="Select" ControlStyle-CssClass="btn btn-outline-primary"/>
                </Columns>
                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />

                <PagerSettings PageButtonCount="15"></PagerSettings>

                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Left" />
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#0000A9" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#000065" />
            </asp:GridView>
        </div>
        <div class="card-footer">
            <asp:Button ID="BtnDownloadformat" runat="server" Text="Download Format" class="btn btn-outline-warning"/>
            &nbsp;
            <asp:Button ID="BtnDownloadAll" runat="server" Text="Download All Data" class="btn btn-outline-info"/>
        </div>
    </div>
    <asp:GridView ID="GrdTest" runat="server"></asp:GridView>
    <asp:TextBox ID="TxtTest" runat="server" TextMode="MultiLine"></asp:TextBox>
</asp:Content>
