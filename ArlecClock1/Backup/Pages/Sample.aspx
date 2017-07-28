<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sample.aspx.cs" Inherits="ArlecEmpTimesheet.Pages.Sample" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:EntityDataSource ID="edsTimesheetDataSource"  runat="server" ContextTypeName="ArlecEmpTimesheet.DAL.WMSDBEntities"
            EnableFlattening="False" EntitySetName="EmpTimesheet" EnableUpdate="True" EnableInsert="True" EnableDelete="True">
    </asp:EntityDataSource>
    <asp:ScriptManager ID="scriptManager" runat="server" />
    
    <h2>Employees</h2>
    <div class="grid">
        <div class="rounded">
            <div class="top-outer">
                <div class="top-inner">
                    <div class="top">
                    </div>
                </div>
            </div>
            <div class="mid-outer">
                <div class="mid-inner">
                    <div class="mid">
                        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate>
                            <asp:ImageButton ID="imgbNew" runat="server" CausesValidation="False" 
                                    CommandName="Insert" ImageUrl="~/Images/add_icon_mono.gif" ToolTip="Add New" 
                                                    width="25" onclick="imgbNew_Click"/>
                                <asp:ListView ID="lvEmployees" runat="server" 
                                    DataSourceID="edsEmployeesDataSource" 
                                    oniteminserting="lvEmployees_ItemInserting" 
                                    onitemupdating="lvEmployees_ItemUpdating" 
                                    onitemdatabound="lvEmployees_ItemDataBound" 
                                    onitemcommand="lvEmployees_ItemCommand">
                                    <LayoutTemplate>
                                        <table class="gridview" cellpadding="0" cellspacing="0">
                                            <tr class="lightheader">
                                                <th></th>
                                                <th>ID </th>
                                                <th>Name</th>
                                                <th>Hire Date</th>
                                                <th>Job</th>
                                                <th>Sex</th>
                                                <th>Birth Date</th>
                                                <th>Classification</th>
                                                <th>Active?</th>
                                                <th></th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr class='<%# Container.DataItemIndex % 2 == 0 ? "row" : "altrow" %>'>
                                            <td class="command">
                                                
                                            </td>
                                            <td><asp:Label ID="lblID" runat="server" Text='<%# Bind("Id") %>' /></td>
                                            <td><%# (Eval("FirstName") ??String.Empty).ToString() + " "+ (Eval("LastName")?? String.Empty).ToString() ?? "&nbsp;" %></td>
                                            <td><%# Eval("HireDate", "{0:MM/dd/yyyy}") ?? "&nbsp;"%></td>
                                            <td><%# Eval("Job")?? "&nbsp;" %></td>
                                            <td><%#  ((Eval("Sex")?? String.Empty).ToString() == "M" ? "Male" :"FeMale")  ?? "&nbsp;" %></td>
                                            <td><%# Eval("BirthDate", "{0:MM/dd/yyyy}") ?? "&nbsp;"%></td>
                                            <td></td>
                                            <td><%# Eval("IsActive").ToString() == "1" ? "Yes":"No" ?? "&nbsp;" %></td>
                                            <td><asp:ImageButton ID="imgbEdit" runat="server" CommandName="Edit" Text="Edit" ImageUrl="~/Images/edit_icon_mono.gif" />
                                                <asp:ImageButton ID="imgbDelete" runat="server" CommandName="Delete" Text="Delete" ImageUrl="~/Images/delete_icon_mono.gif" OnClientClick="return confirm('Are you sure you want to delete this contact?');" ToolTip="Delete"/> </td>
                                        </tr>
                                    </ItemTemplate>
                                    </asp:ListView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="bottom-outer">
                <div class="bottom-inner">
                    <div class="bottom">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
