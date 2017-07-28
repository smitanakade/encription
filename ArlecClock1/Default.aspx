<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ArlecClock._Default" %>
<asp:content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
   

</asp:content>
<asp:content  ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="max-width:100%; margin:0 auto;font-size:80px; margin-top:-10px; color:brown; text-align:center;">
<br />
       
<strong>Clock Card </strong>  <br />   

 <asp:label Width="100%" id="LabelMesage" runat="server" Font-Size="Large" Text="Scan Here To Login " ><asp:TextBox  ID="AdminLogin" AutoPostBack="true" runat="server" OnTextChanged="AdminLogin_TextChanged" Height="42px" Width="824px"></asp:TextBox></asp:label>
       </div>






 </asp:content>