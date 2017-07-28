<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Classification.aspx.cs" Inherits="ArlecEmpTimesheet.Pages.Classification" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" href="../Styles/round.css" rel="stylesheet" />    
    <link type="text/css" href="../Styles/grid.css" rel="stylesheet" />  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="scriptManager" runat="server" />
    
  
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
                        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                            <asp:ImageButton ID="imgbNew" runat="server" CausesValidation="False" 
                                    CommandName="Insert" ImageUrl="~/Images/add_icon_mono.gif" ToolTip="Add New" 
                                                    width="25" onclick="imgbNew_Click"/>
                                <asp:ListView ID="lvClassifications" runat="server" 
                                    DataSourceID="edsClassDataSource" 
                                    oniteminserting="lvClassifications_ItemInserting" 
                                    
                                    onitemcommand="lvClassifications_ItemCommand">
                                    <LayoutTemplate>
                                        <table class="gridview" cellpadding="0" cellspacing="0">
                                            <tr class="lightheader">
                                                <th>
                                                </th>
                                                <th>ID </th>
                                                <th>Name</th>
                                                <th>Award</th>
                                                <th>Meal Allowance Amount</th>
                                                <th>Mon-Fri < 2 Hrs</th>
                                                <th>Mon-Fri > 2 Hrs</th>
                                                <th>Sat</th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr class='<%# Container.DataItemIndex % 2 == 0 ? "row" : "altrow" %>'>
                                            <td class="command">
                                                <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" />
                                            </td>
                                            <td><%# Eval("Id") %></td>
                                            <td><%# Eval("Name")  ?? "&nbsp;" %></td>
                                            <td><%# Eval("Award") ?? "&nbsp;"%></td>
                                            <td><%# Eval("MealAllowanceAmount") ?? "&nbsp;"%></td>
                                            <td><%# Eval("MonToFriFirst2HoursRate") ?? "&nbsp;"%></td>
                                            <td><%# Eval("MonToFriGraterThan2HoursRate") ?? "&nbsp;"%></td>
                                            <td><%# Eval("SatFirst2Hours") ?? "&nbsp;"%></td>
                                        </tr>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <tr class='edit-info <%# Container.DataItemIndex == 0 ? "first" : string.Empty %>'>
                                            <td class="command">
                                                <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" />
                                            </td>
                                            <td><%# Eval("Id") %></td>
                                            <td colspan = "10"><%# Eval("Name") ?? "&nbsp;"%></td>
                                        </tr>
                                        <tr>
                                            <td class="edit" colspan="10">
                                                <div class="details">
                                                    <div class="lightheader">
                                                        Edit details for '<%# Eval("Id")%>'</div>
                                                   <table class="detailview" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <th>ID</th>
                                                            <td><asp:Label ID="lblID" runat="server" Text='<%# Bind("Id") %>' /></td>
                                                            <th>Name</th>
                                                            <td><asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' TextMode="multiline" /> </td>
                                                        </tr>
                                                    <tr>
                                                            <th>Award</th>
                                                            <td><asp:TextBox ID="txtLastName" runat="server" Text='<%# Bind("Award") %>' TextMode="multiline" /></td>
                                                        <th>MealAllowance</th>
                                                        <td><asp:TextBox ID="txtHiredate" runat="server" Text='<%# Bind("MealAllowanceAmount") %>' />
                                                        </td>
                                                        </tr>
                                                    <tr>
                                                        <th>MealAllowanceDue</th>
                                                        <td><asp:TextBox ID="txtJob" runat="server" Text='<%# Bind("MealAllowanceDue") %>' /></td>
                                                        <th>Mon-Fri < 2 Hrs Rate</th>
                                                        <td><asp:TextBox ID="txtSex" runat="server" Text='<%# Bind("MonToFriFirst2HoursRate") %>' /></td></tr>
                                                    <tr>   
                                                        <th>Mon-Fri > 2 Hrs Rate</th>
                                                        <td><asp:TextBox ID="txtBirthDate" runat="server" Text='<%# Bind("MonToFriGraterThan2HoursRate") %>' />
                                                        </td>
                                                        <th>Sat First 2 Hours</th>                                                    
                                                        <td><asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("SatFirst2Hours") %>' />
                                                        </td>
                                                    </tr> 
                                                    <tr> 
                                                        <th>Sat After 2 Hours</th>                                                    
                                                        <td><asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("SatAfter2Hours") %>' />
                                                        </td>                                                      
                                                        <th>SatTea?</th>                                                    
                                                        <td>
                                                            <asp:DropDownList ID="ddlSatTea" runat="server"  SelectedValue='<%# Bind("SatTeaMoney") %>'>
                                                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                <asp:ListItem Value="N">No</asp:ListItem>
                                                            </asp:DropDownList> 
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <th>Sun</th>
                                                        <td><asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("SunAllHours") %>' />
                                                        </td>
                                                        <th>SunTea?</th>
                                                        <td><asp:DropDownList ID="ddlSunTea" runat="server"  SelectedValue='<%# Bind("SunTeaMoney") %>'>
                                                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                <asp:ListItem Value="N">No</asp:ListItem>
                                                            </asp:DropDownList> 
                                                        </td>

                                                    </tr>      
                                                    <tr>
                                                        <th>GoodFriday/Xmas</th>
                                                        <td><asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("GoodFridayorXmasRate") %>' />
                                                        </td>
                                                        <th>OtherPubHolidays</th>
                                                        <td><asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("AllOtherPubHolidays") %>' />
                                                        </td>

                                                    </tr>    
                                                    <tr>
                                                        <th>WorkingHours</th>                                                    
                                                        <td><asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("WorkingHours") %>' />
                                                        </td>
                                                        <th>PayRate</th>
                                                        <td><asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("PayRate") %>' />
                                                        </td>
                                                    </tr>                                                                                                                                                      
                                                    </table>
                                                    <div class="footer command">
                                                        <asp:LinkButton ID="btnSave" runat="server" Text="Save" CommandName="Update" />
                                                        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CommandName="Cancel" />
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <tr>
                                            <td class="edit" colspan="10">
                                                <div class="details">
                                                    <div class="lightheader">
                                                        Insert details for </div>
                                                        <table class="detailview" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <th>Name</th>
                                                            <td><asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>' TextMode="multiline" /> </td>
                                                            <th></th>
                                                            <td><asp:Label ID="lblID" runat="server" Text='<%# Bind("Id") %>' /></td>
                                                        </tr>
                                                        <tr>
                                                            <th>Award</th>
                                                            <td><asp:TextBox ID="txtLastName" runat="server" Text='<%# Bind("Award") %>' TextMode="multiline"/></td>
                                                            <th>MealAllowance</th>
                                                            <td><asp:TextBox ID="txtHiredate" runat="server" Text='<%# Bind("MealAllowanceAmount") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>MealAllowanceDue</th>
                                                            <td><asp:TextBox ID="txtJob" runat="server" Text='<%# Bind("MealAllowanceDue") %>' /></td>
                                                            <th>Mon-Fri < 2 Hrs Rate</th>
                                                            <td><asp:TextBox ID="txtSex" runat="server" Text='<%# Bind("MonToFriFirst2HoursRate") %>' /></td></tr>
                                                        <tr>   
                                                            <th>Mon-Fri > 2 Hrs Rate</th>                                                    
                                                            <td><asp:TextBox ID="txtBirthDate" runat="server" Text='<%# Bind("MonToFriGraterThan2HoursRate") %>' />
                                                            </td>
                                                            <th>Sat First 2 Hours</th>                                                    
                                                            <td><asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("SatFirst2Hours") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr> 
                                                            <th>Sat After 2 Hours</th>                                                    
                                                            <td><asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("SatAfter2Hours") %>' />
                                                            </td>                                                      
                                                            <th>SatTea?</th>                                                    
                                                            <td>
                                                                <asp:DropDownList ID="ddlSatTea" runat="server"  SelectedValue='<%# Bind("SatTeaMoney") %>'>
                                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                                </asp:DropDownList> 
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <th>Sun</th>
                                                            <td><asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("SunAllHours") %>' />
                                                            </td>
                                                            <th>SunTea?</th>
                                                            <td><asp:DropDownList ID="ddlSunTea" runat="server"  SelectedValue='<%# Bind("SunTeaMoney") %>'>
                                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                                </asp:DropDownList> 
                                                            </td>

                                                        </tr>      
                                                        <tr>
                                                            <th>GoodFriday/Xmas</th>
                                                            <td><asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("GoodFridayorXmasRate") %>' />
                                                            </td>
                                                            <th>OtherPubHolidays</th>
                                                            <td><asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("AllOtherPubHolidays") %>' />
                                                            </td>

                                                        </tr>    
                                                        <tr>
                                                            <th>WorkingHours</th>                                                    
                                                            <td><asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("WorkingHours") %>' />
                                                            </td>
                                                            <th>PayRate</th>
                                                            <td><asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("PayRate") %>' />
                                                            </td>
                                                        </tr>                                                                                                                                                      
                                                    </table>
                                                        <div class="footer command">
                                                        <asp:LinkButton ID="btnInsert" runat="server" Text="Insert" CommandName="Insert" />
                                                        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CommandName="Cancel" />
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </InsertItemTemplate>
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
    <asp:ObjectDataSource ID="odbClassificationList" runat="server" SelectMethod="GetClassifications"
        TypeName="ArlecEmpTimesheet.BL.ClassificationBLL" DataObjectTypeName="ArlecEmpTimesheet.Models.Classification"
        OldValuesParameterFormatString="original_{0}" UpdateMethod="Update">
        <SelectParameters>
            <asp:Parameter Name="classId" Type="Int32" />
            <asp:Parameter Name="className" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:EntityDataSource ID="edsClassDataSource" runat="server" ContextTypeName="ArlecEmpTimesheet.DAL.WMSDBEntities"
    EntitySetName="Classifications" EnableUpdate="True" EnableInsert="True" EnableDelete="True"/>
  
</asp:Content>
