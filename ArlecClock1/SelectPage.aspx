<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectPage.aspx.cs" Inherits="ArlecClock.SelectPage" %>
<asp:content  ID="Content2" ContentPlaceHolderID="MainContent" runat="server"><br />
<div style=" width:500px;  margin:0 auto; font-size:xx-large; ">
   <span style="text-align:center; margin-left:20px;"> Welcome to Clock Card<br /></span><br />
   
    <asp:Button ID="ClockIN" runat="server" Text="Log IN " CssClass="selectbutton" OnClick="ClockIN_Click" Height="60px" Width="494px" /><br /><br />
        <asp:Button ID="ClockOut" runat="server" Text="Log Out" CssClass="selectOutbutton" OnClick="ClockOut_Click" Height="60px" Width="494px" />
</div>
    </asp:content>