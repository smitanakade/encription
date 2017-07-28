<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Employee.aspx.cs" Inherits="ArlecEmpTimesheet.Pages.Employee" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" href="../Styles/round.css" rel="stylesheet" />    
    <link type="text/css" href="../Styles/grid.css" rel="stylesheet" />   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<asp:ScriptManager ID="scriptManager" runat="server" />--%>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />  
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
                        <%--<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional" >--%>
                            <ContentTemplate>
                                <asp:UpdatePanel id="Updatepanel2" runat="server" >
                                    <contenttemplate>
                                    Name&nbsp; :
                            <asp:DropDownList ID="ddlEmpName" runat="server" AutoPostBack="True" 
                                                                DataTextField="Name" DataValueField="Id" onselectedindexchanged="ddlEmpName_SelectedIndexChanged"  />
                                <div style="float: right">
                                    <asp:ImageButton ID="imgbNew" runat="server" CausesValidation="False" CommandName="Insert"
                                        ImageUrl="~/Images/add_icon_mono.gif" ToolTip="Add New" Width="25" OnClick="imgbNew_Click" />
                                </div>
                                <br />
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
                                                <%--<th>Birth Date</th>--%>
                                                <th>Classification</th>
                                                <th>Active?</th>
                                                <th></th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server" />
                                            <tr id="Tr1" runat="server" style="background-color: Silver" align="center">
                                                <td id="Td1" runat="server" colspan="10">
                                                    <asp:DataPager ID="DataPager1" runat="server" PageSize="20">
                                                        <Fields>                                                        
                                                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True"
                                                                FirstPageText="&lt;&lt;" PreviousPageText="&lt;" NextPageText="&gt;" LastPageText="&gt;&gt;" />
                                                        </Fields>
                                                    </asp:DataPager>
                                                </td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr class='<%# Container.DataItemIndex % 2 == 0 ? "row" : "altrow" %>'>
                                            <td class="command">
                                                <%--<asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" />--%>
                                            </td>
                                            <td><asp:Label ID="lblID" runat="server" Text='<%# Bind("Id") %>' /></td>
                                            <td><%# (Eval("FirstName") ??String.Empty).ToString() + " "+ (Eval("LastName")?? String.Empty).ToString() ?? "&nbsp;" %></td>
                                            <td><%# Eval("HireDate", "{0:MM/dd/yyyy}") ?? "&nbsp;"%></td>
                                            <td><%# Eval("Job")?? "&nbsp;" %></td>
                                            <td><%#  ((Eval("Sex")?? String.Empty).ToString() == "M" ? "Male" :"FeMale")  ?? "&nbsp;" %></td>
                                            <%--<td><%# Eval("BirthDate", "{0:MM/dd/yyyy}") ?? "&nbsp;"%></td>--%>
                                            <td><asp:TemplateField HeaderText="Department">
                                                    <%--<ItemTemplate>
                                                        
                                                    </ItemTemplate>--%>
                                                </asp:TemplateField><asp:Label ID="Label2" runat="server" Text='<%# Eval("Classification.Name") %>'></asp:Label></td>
                                            <td><%# Eval("IsActive").ToString() == "1" ? "Yes":"No" ?? "&nbsp;" %></td>
                                            <td><asp:ImageButton ID="imgbEdit" runat="server" CommandName="Edit" ToolTip="Edit" ImageUrl="~/Images/edit_icon_mono.gif" />
                                                <asp:ImageButton ID="imgbDelete" runat="server" CommandName="Delete"  ImageUrl="~/Images/delete_icon_mono.gif" OnClientClick="return confirm('Are you sure you want to delete this contact?');" ToolTip="Delete"/> </td>
                                        </tr>                                            
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <tr>
                                            <td class="edit" colspan="10">
                                                <div class="details">
                                                    <div class="lightheader">
                                                        Insert details for </div>
                                                    <table class="detailview" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <th>FirstName</th>
                                                            <td>
                                                                <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("FirstName") %>' />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtName"
                                                                    ErrorMessage="FirstName" Text="*" runat="server" ForeColor="Red"/>
                                                            </td>
                                                            <th>LastName</th>
                                                            <td><asp:TextBox ID="txtLastName" runat="server" Text='<%# Bind("LastName") %>' /><asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtLastName"
                                                                    ErrorMessage="LastName" Text="*" runat="server" ForeColor="Red" /></td>
                                                        </tr>
                                                        <tr>
                                                            <th>HireDate</th>
                                                            <td><asp:TextBox ID="txtHireDate" runat="server" Text='<%# Bind("HireDate") %>' /><asp:CalendarExtender
                                                                    ID="cxHireDate" runat="server" TargetControlID="txtHireDate" SelectedDate='<%# Eval("HireDate") %>'  Format="dd/MM/yyyy" />
                                                            </td>
                                                            <th>Job</th>
                                                            <td><asp:TextBox ID="txtJob" runat="server" Text='<%# Bind("Job") %>' /></td>
                                                        </tr>
                                                    <tr>
                                                        <th>Sex</th>
                                                        <td>
                                                                <asp:DropDownList ID="ddlSex" runat="server"  SelectedValue='<%# Bind("Sex")  %>'>
                                                                    <asp:ListItem Value="M">Male</asp:ListItem>
                                                                    <asp:ListItem Value="F">Female</asp:ListItem>
                                                                </asp:DropDownList>                                                            
                                                        </td>
                                                        <th>BirthDate</th>
                                                        <td><asp:TextBox ID="txtBirthDate" runat="server" Text='<%# Bind("BirthDate") %>' />
                                                        <asp:CalendarExtender
                                                                    ID="CalendarExtender1" runat="server" TargetControlID="txtBirthDate" SelectedDate='<%# Eval("BirthDate") %>' Format="dd/MM/yyyy"/></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Active?</th>
                                                        <td>
                                                                <asp:DropDownList ID="ddlActive" runat="server"  SelectedValue='<%# Bind("IsActive") %>'>
                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                                </asp:DropDownList>                                                            
                                                        </td></tr>
                                                    <tr>
                                                        <th>Lead Hand Allowance?</th>
                                                        <td>
                                                                <asp:DropDownList ID="ddlLeadhandA" runat="server"  SelectedValue='<%# Bind("IsLeadHandA") %>'>
                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                                </asp:DropDownList>                                                            
                                                        </td>
                                                        <th>First Aid Allowance?</th>
                                                        <td>
                                                                <asp:DropDownList ID="ddlIsFirstAidA" runat="server"  SelectedValue='<%# Bind("IsFirstAidA") %>'>
                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                                </asp:DropDownList>                                                            
                                                        </td>                                                                                                       
                                                    </tr>
                                                        <tr>
                                                         <th>Classification</th>
                                                        <td colspan="3"><asp:EntityDataSource ID="ClassificationsEntityDataSource" runat="server" ConnectionString="name=WMSDBEntities"
                                                                                        DefaultContainerName="WMSDBEntities" EnableFlattening="False" AutoGenerateWhereClause="true"
                                                                                        EntitySetName="Classifications" EntityTypeFilter="Classification">
                                                                         <WhereParameters>
                                                                            <asp:Parameter DefaultValue= "True" DbType="Boolean" Name="Active" />
                                                                          </WhereParameters>
                                                                    </asp:EntityDataSource>
                                                                    <asp:DropDownList ID="ddlClassificationInsert" runat="server" DataSourceID="ClassificationsEntityDataSource"
                                                                        DataTextField="Name" DataValueField="ID" OnInit="ClassificationsDropDownList_Init" >
                                                                    </asp:DropDownList></td>
                                                    </tr> 
                                                    </table>
                                                    <asp:ValidationSummary ID="ValidationSummary1"
                                                        ShowMessageBox="true"
                                                        ShowSummary="false"
                                                        HeaderText="You must enter a value in the following fields:"
                                                        EnableClientScript="true"
                                                        runat="server"/>
                                                    <div class="footer command">
                                                        <asp:LinkButton ID="btnInsert" runat="server" Text="Insert" CommandName="Insert" />
                                                        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="False" />
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <tr class='edit-info <%# Container.DataItemIndex == 0 ? "first" : string.Empty %>'>
                                            <td class="command">
                                                <%--<asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" />--%>
                                            </td>
                                            <td><%# Eval("ID") %></td>
                                            <td colspan = "10"><%# Eval("FirstName") ?? "&nbsp;"%></td>
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
                                                            <th>FirstName</th>
                                                            <td><asp:TextBox ID="txtName" runat="server" Text='<%# Bind("FirstName") %>' /> </td>
                                                        </tr>
                                                        <tr>                                                            
                                                            <th>LastName</th>
                                                            <td><asp:TextBox ID="txtLastName" runat="server" Text='<%# Bind("LastName") %>' /></td>
                                                            <th>HireDate</th>
                                                            <td><asp:TextBox ID="txtHireDate" runat="server" Text='<%# Bind("HireDate") %>' />                                                            
                                                            <asp:CalendarExtender
                                                                    ID="cxHireDate" runat="server" TargetControlID="txtHireDate" SelectedDate='<%# Eval("HireDate") %>' Format="dd/MM/yyyy"/></td>
                                                        </tr>
                                                    <tr>  
                                                        <th>Job</th>
                                                        <td><asp:TextBox ID="txtJob" runat="server" Text='<%# Bind("Job") %>' /></td>                                                                                                          
                                                        <th>Sex</th>
                                                        <td>
                                                                <asp:DropDownList ID="ddlSex" runat="server"  SelectedValue='<%# Bind("Sex") %>'>
                                                                    <asp:ListItem Value="M">Male</asp:ListItem>
                                                                    <asp:ListItem Value="F">Female</asp:ListItem>
                                                                </asp:DropDownList>                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>BirthDate</th>
                                                        <td><asp:TextBox ID="txtBirthDate" runat="server" Text='<%# Bind("BirthDate") %>' />
                                                        <asp:CalendarExtender
                                                                    ID="CalendarExtender1" runat="server" TargetControlID="txtBirthDate" SelectedDate='<%# Eval("BirthDate") %>' Format="dd/MM/yyyy"/></td>
                                                        <th>Active?</th>
                                                        <td>
                                                                <asp:DropDownList ID="ddlActive" runat="server"  SelectedValue='<%# Bind("IsActive") %>'>
                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                                </asp:DropDownList>                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Lead Hand Allowance?</th>
                                                        <td>
                                                                <asp:DropDownList ID="ddlLeadhandA" runat="server"  SelectedValue='<%# Bind("IsLeadHandA") %>'>
                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                                </asp:DropDownList>                                                            
                                                        </td>
                                                        <th>First Aid Allowance?</th>
                                                        <td>
                                                                <asp:DropDownList ID="ddlIsFirstAidA" runat="server"  SelectedValue='<%# Bind("IsFirstAidA") %>'>
                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                                </asp:DropDownList>                                                            
                                                        </td>                                                                                                       
                                                    </tr>
                                                    <tr>
                                                        <th>Classification</th>
                                                        <td colspan="2"><asp:EntityDataSource ID="ClassificationsEntityDataSource" runat="server" ConnectionString="name=WMSDBEntities"
                                                                                        DefaultContainerName="WMSDBEntities" EnableFlattening="False" AutoGenerateWhereClause="true"
                                                                                        EntitySetName="Classifications" EntityTypeFilter="Classification" >
                                                                         <WhereParameters>
                                                                            <asp:Parameter DefaultValue= "True" DbType="Boolean" Name="Active" />
                                                                          </WhereParameters>
                                                                    </asp:EntityDataSource>
                                                                    <asp:DropDownList ID="ClassificationsDropDownList" runat="server" DataSourceID="ClassificationsEntityDataSource"
                                                                        DataTextField="Name" DataValueField="ID" OnInit="ClassificationsDropDownList_Init" SelectedValue='<%# Eval("ClassificationId") %>'>
                                                                    </asp:DropDownList></td>
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
                                </asp:ListView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        <%--</asp:UpdatePanel>--%>
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
<%--    <asp:ObjectDataSource ID="odbEmployeesList" runat="server" SelectMethod="GetEmployees"
        TypeName="ArlecEmpTimesheet.BL.EmployeeBLL" DataObjectTypeName="ArlecEmpTimesheet.Models.Employee"
        OldValuesParameterFormatString="original_{0}" UpdateMethod="Update" 
        InsertMethod="Insert" DeleteMethod="Delete">
        <SelectParameters>
            <asp:Parameter Name="empId" Type="Int32" />
            <asp:Parameter Name="empName" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>--%>

<%--    <asp:DetailsView ID="EmployeesDetailsView" runat="server" AutoGenerateRows="False"
        DataSourceID="edsEmployeesDataSource" DataKeyNames="Id" 
        DefaultMode="Insert" oniteminserting="EmployeesDetailsView_ItemInserting"
        >
        <Fields>
            <asp:BoundField DataField="Id" HeaderText="ID" />
            <asp:BoundField DataField="FirstName" HeaderText="FirstName" />
            <asp:BoundField DataField="LastName" HeaderText="LastName" />
            <asp:TemplateField HeaderText="Classification">
                <InsertItemTemplate>
                    <asp:EntityDataSource ID="ClassificationsEntityDataSource" runat="server" ConnectionString="name=WMSDBEntities"
                        DefaultContainerName="WMSDBEntities" EnableFlattening="False"
                        EntitySetName="Classifications" EntityTypeFilter="Classification" 
                        >
                    </asp:EntityDataSource>
                    <asp:DropDownList ID="ClassificationsDropDownList" runat="server" DataSourceID="ClassificationsEntityDataSource"
                        DataTextField="Name" DataValueField="ID" OnInit="ClassificationsDropDownList_Init">
                    </asp:DropDownList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>--%>


                        

        <asp:EntityDataSource ID="edsEmployeesDataSource"  runat="server" ContextTypeName="ArlecEmpTimesheet.DAL.WMSDBEntities"
        AutoGenerateWhereClause="true" EnableFlattening="False" EntitySetName="Employees" EnableUpdate="True" EnableInsert="True" EnableDelete="True">
        <WhereParameters>
            <asp:ControlParameter ControlID="ddlEmpName" Name="Id" PropertyName="Text"
                Type="Int16" />
        </WhereParameters>
    </asp:EntityDataSource>
    
</asp:Content>
