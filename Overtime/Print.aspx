<%@ Page Title="พิมพ์รายการรีเควส" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Print.aspx.vb" Inherits="Overtime.Print" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-3">
            <label>วันที่ (Date.)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text" ><i class="fas fa-calendar-check"></i></span>
                </div>
                <asp:TextBox ID="TxtWorkDate" runat="server" class="form-control" aria-describedby="basic-addon1" TextMode="Date"></asp:TextBox>
            </div>
        </div>

        <div class="col-3">
            <label>บริษัท (Company.)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text" ><i class="fas fa-city"></i></span>
                </div>
                <asp:DropDownList ID="DrpCompany" runat="server" class="form-control"></asp:DropDownList>
            </div>
        </div>

        <div class="col-3">
            <asp:RadioButtonList ID="RadioButtonSelect" runat="server" CssClass="form-check">
                <asp:ListItem Value="1"> เลือกตามกระบวนการ (All)</asp:ListItem>
                <asp:ListItem Value="2"> เลือกตามผู้รีเควส (Leader)</asp:ListItem>
                <asp:ListItem Value="3"> รายบุคคล (Individual)</asp:ListItem>
            </asp:RadioButtonList>
            <asp:DropDownList ID="DrpProcess" runat="server" Visible="false" class="form-control"></asp:DropDownList>
            <asp:DropDownList ID="DrpShift" runat="server" Visible="false" class="form-control"></asp:DropDownList>
            <asp:DropDownList ID="DrpRequester" runat="server" Visible="false" class="form-control"></asp:DropDownList>
            <asp:DropDownList ID="DrpRequestTime" runat="server" Visible="false" class="form-control"></asp:DropDownList>
            <asp:DropDownList ID="DrpProcessIndiv" runat="server" Visible="false" class="form-control"></asp:DropDownList>
            <asp:DropDownList ID="DrpName" runat="server" Visible="false" class="form-control"></asp:DropDownList>
        </div>

        <div class="col-3 justify-content-end">
            <br />
            <asp:Button ID="BtnShow" runat="server" Text="แสดงรายการ" class="btn btn-primary mt-2"/>
            &nbsp;<asp:Button ID="BtnClear" runat="server" Text="ล้างรายการ" class="btn btn-danger mt-2"/>
        </div>
    </div>
    <div class="card">
        <div class="card-header">
            <b>พิมพ์ใบรีเควส (Print Requested List)</b>
        </div>
        <div class="card-body">
            <rsweb:ReportViewer ID="ReportViewerPrint" runat="server" Width="100%"></rsweb:ReportViewer>
        </div>
    </div>
    <asp:TextBox ID="TxtTest" runat="server" TextMode="MultiLine"></asp:TextBox>
</asp:Content>
