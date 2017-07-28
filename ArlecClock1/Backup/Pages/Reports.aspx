<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="ArlecEmpTimesheet.Pages.Reports" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    Year&nbsp; : <asp:DropDownList ID="ddlDateYear" runat="server" 
        onselectedindexchanged="ddlDateYear_SelectedIndexChanged" AutoPostBack="true"/>
    WeekNo&nbsp; :<asp:DropDownList ID="ddlWeekNo" runat="server"  AutoPostBack="true"
        DataTextField="Week" DataValueField="WeekNo" 
        onselectedindexchanged="ddlWeekNo_SelectedIndexChanged"/>
    Name&nbsp; : 
    <asp:TextBox ID="txtEmpName" runat="server" AutoPostBack="True" 
        ontextchanged="txtEmpName_TextChanged"></asp:TextBox>
        <br /><br />
    <asp:Label ID="lblDate" runat="server" Text=""></asp:Label> 
    <br /><br />
    <rsweb:ReportViewer ID="rvGetEmployee" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="754px" 
        Width="932px">
        <LocalReport ReportPath="Reports\EmployeePayroll.rdlc">
        </LocalReport>
</rsweb:ReportViewer>
<%--    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>--%>
</asp:Content>
