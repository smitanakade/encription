<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Tracker.aspx.cs" Inherits="ArlecClock.Pages.Tracker" %>
<asp:content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" href="../Styles/round.css" rel="stylesheet" />    
    <link type="text/css" href="../Styles/grid.css" rel="stylesheet" />   
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../scripts/jquery.blockUI.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.clock.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.js" type="text/javascript" ></script> 

</asp:content>
<asp:content  ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style=" width:300px;  margin:0 auto; font-size:large; margin-bottom:-20px; ">
   <span style="text-align:left; margin-left:0px; font-weight:bold;">Select your Options To Scan!! </span><br />
   
    <asp:Button ID="ClockIN" runat="server" Text="Clock IN " CssClass="InActive" OnClick="ClockIN_Click"  />       <asp:Button ID="ClockOut" runat="server" Text="Clock Out" CssClass="InActive" OnClick="ClockOut_Click" />
</div>

    <div style="max-width:100%; margin:0 auto;font-size:80px; color:#10a31c; text-align:center; margin-top:-34px;">
    <asp:scriptmanager ID="scriptManager" runat="server"  />
        <asp:Timer ID="tmrUpdate" runat="server" Interval="1000" 
            ontick="tmrUpdate_Tick">
        </asp:Timer>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="tmrUpdate" EventName="Tick" />
            </Triggers>
            <ContentTemplate><br />
               <asp:Label ID="ScanMessage" runat="server" ></asp:Label>
                <asp:Label ID="lblTime" runat="server" Text="" Font-Bold="true"  Font-Size="120px" ForeColor="#666666" Width="100%"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    
       <asp:TextBox  ID="LoginId" AutoPostBack="true" runat="server" OnTextChanged="LoginId_TextChanged"  Height="42px" Width="824px"></asp:TextBox>
       </div>

<script type="text/javascript">
    $(document).ready(function () {
        $("#<%=LoginId.ClientID %>").keyup(function () {
            if ($("#<%=LoginId.ClientID %>").val() > 3) {
                __doPostBack("LoginId", "TextChanged");
                
            }

     });
    });

</script>
   


 <asp:ListView ID="lvLocationDetails" runat="server" DataSourceID="SqlDataSource2" >
     
<ItemTemplate>
   
   <div style=" max-width:100%; margin:0 auto; text-align:center;">
  
    <span style="font-size:80px; color:#BF2A1D; text-align:center;">   Welcome  <asp:label id="Message" runat="server" Text='<%# Eval("FirstName") %>'></asp:label></span><br />
      <span style="font-size:70px; color:#10a31c;"><%--Scanned Successfully <br />--%>
           <%--<asp:Label ID="Label7" runat="server" Text='<%# ProcessMyDataItem(Eval("scan")) %>'/><br> --%>
          <asp:Label ID="Label11" runat="server" Text='<%# (Eval("INTime").ToString()== "" ) ?  "" : "Clock IN Time : "+Eval("INTime") %>'/><br />
      <asp:Label ID="Label5" runat="server" Text='<%# (Eval("OutTime").ToString()== "" ) ?   "":"Clock Out Time : "+Eval("OutTime") %>'/>   
 <%-- <strong>Clock IN Time  :</strong> --%></span>
   </div>
   </ItemTemplate>
         
    <%--Below InsertItemTemplate will only display if  --%>
     <InsertItemTemplate>
                    <table border="0" width="100%" style="text-align: left; font-size:xx-large; color:#BF2A1D;"">
                        <tr>
                            <td style="font-size:xx-large;">No Record Found! <br /> Kindly Check with your supervisor.</td>

                        </tr>
                    </table>
                </InsertItemTemplate>
     </asp:ListView>
    <%--Below InsertItemTemplate will only display if clock in or clock out button is not selected  --%>
    <asp:ListView ID="MissingQueryString" runat="server">
        <InsertItemTemplate>
            <div style="margin:0 auto; width:400px; font-size:xx-large; color:#BF2A1D;">
                Please Select Clock IN or Clock Out Option
            </div>
        </InsertItemTemplate>
    </asp:ListView>
<%--<asp:ListView ID="alreadyClockIN" runat="server">
        <InsertItemTemplate>
            <div style="margin:0 auto; width:400px;  font-size:xx-large; color:#BF2A1D;">
                Your Have Already Scan !!
            </div>
        </InsertItemTemplate>
    </asp:ListView>--%>



<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Arlec_TimesheetConnectionString %>" SelectCommand="SELECT MAX(et.INTime) AS INTime, MAX(et.OutTime) AS OutTime,e.FirstName,e.LastName,e.Job
  FROM [WMSDB].[dbo].[EmpAttendanceLog] et INNER JOIN [WMSDB].[dbo].Employees e ON e.Id = et.EmployeeId AND e.LoginId = @LoginId and  CONVERT(date, et.DateScaned) = CONVERT(date,GETDATE()) GROUP BY e.FirstName,e.LastName,e.Job
">
   
     <SelectParameters>
                <asp:SessionParameter  Name="LoginId" SessionField="LoginId"  Type="String" />

             
            </SelectParameters>

</asp:SqlDataSource>


 </asp:content>





