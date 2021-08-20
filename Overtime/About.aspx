<%@ Page Title="About" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.vb" Inherits="Overtime.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %> Request Overtime Website</h2>
    <div class="row">
        <div class="col-4">
            <div class="list-group" id="list-tab" role="tablist">
                <a class="list-group-item list-group-item-action active" id="list-opr1-list" data-toggle="list" href="#list-opr1" role="tab" aria-controls="home">1) พนักงาน Request Overtime</a>
                <a class="list-group-item list-group-item-action" id="list-opr2-list" data-toggle="list" href="#list-opr2" role="tab" aria-controls="profile">2) หัวหน้า Confirm Overtime</a>
                <a class="list-group-item list-group-item-action" id="list-opr3-list" data-toggle="list" href="#list-opr3" role="tab" aria-controls="messages">3) หัวหน้า Print ใบ Request</a>
                <a class="list-group-item list-group-item-action" id="list-opr4-list" data-toggle="list" href="#list-opr4" role="tab" aria-controls="settings">4) พนักงานเซ็นต์ยินยอมทำ Overtime</a>
                <a class="list-group-item list-group-item-action" id="list-opr5-list" data-toggle="list" href="#list-opr5" role="tab" aria-controls="settings">5) ส่งใบ Request ให้ Clerk</a>
            </div>
        </div>
        <div class="col-8">
            <div class="tab-content" id="nav-tabContent">
                <div class="tab-pane fade show active" id="list-opr1" role="tabpanel" aria-labelledby="list-home-list">a</div>
                <div class="tab-pane fade" id="list-opr2" role="tabpanel" aria-labelledby="list-profile-list">b</div>
                <div class="tab-pane fade" id="list-opr3" role="tabpanel" aria-labelledby="list-messages-list">c</div>
                <div class="tab-pane fade" id="list-opr4" role="tabpanel" aria-labelledby="list-settings-list">d</div>
                <div class="tab-pane fade" id="list-opr5" role="tabpanel" aria-labelledby="list-settings-list">e</div>
            </div>
        </div>
    </div>
</asp:Content>
