<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PrintEAR.aspx.vb" Inherits="Overtime.PrintEAR" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-3" style="left: 0px; top: 0px">
            <label>วันที่ (Date.)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text" ><i class="fas fa-calendar-check"></i></span>
                </div>
                <asp:TextBox ID="TxtActionDate" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
            </div>
        </div>

        <div class="col-3">
            <label>ผู้รีเควส (Requester.)</label>
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text" ><i class="fas fa-city"></i></span>
                </div>
                <asp:DropDownList ID="DrpCompany" runat="server" class="form-control"></asp:DropDownList>
            </div>
        </div>

    </div>

    <div class="card">
        <div class="card-header">
            <b>พิมพ์ใบเปลี่ยนกะ / กระบวนการ (Print Employee Action Report)</b>
        </div>
        <div class="card-body">
            <rsweb:ReportViewer ID="ReportViewerEAR" runat="server" Width="100%" ZoomMode="PageWidth" ></rsweb:ReportViewer>
        </div>
    </div>
</asp:Content>
