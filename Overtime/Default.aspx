<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="Overtime._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h3>ประวัติการ Request Overtime ของแต่ละ Process</h3>
        <canvas class="img-fluid border rounded" id="myChart" height="100vh"></canvas>
        <div style="position:absolute;"></div>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>วิธีการใช้งาน Website</h2>
            <p>
                ขั้นตอนพื้นฐานสำหรับการรีเควส Overtime, การตั้งค่าเครื่องพิมพ์ Fuji Xerox และอื่นๆ
            </p>
            <p>
                <a class="btn btn-default" href="~/WorkInstruction" runat="server">เพิ่มเติม.. &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>รายงาน สรุปข้อมูล เกี่ยวกับ Overtime ของแผนก Mecha</h2>
            <p>
                - ประวัติการใช้งาน WebSite<br />
                - รายงานสรุปภาพรวมของการทำ Overtime<br />
                - อื่นๆ ...<br />
            </p>
            <p>
                <a class="btn btn-default" href="~/Summary" runat="server">เพิ่มเติม.. &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>เกี่ยวกับเว็บไซต์ Request Overtime</h2>
            <p>
                ประวัติการอัพเดท ฟังก์ชั่น และแก้ไขบัค ต่างๆ ...
            </p>
            <p>
                <a class="btn btn-default" href="~/About" runat="server">เพิ่มเติม.. &raquo;</a>
            </p>
        </div>
    </div>
    <asp:GridView ID="GrdTest" runat="server"></asp:GridView>
</asp:Content>
