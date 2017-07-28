<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Timesheet.aspx.cs" Inherits="ArlecClock.Pages.Timesheet" %>
<asp:content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" href="../Styles/round.css" rel="stylesheet" />    
    <link type="text/css" href="../Styles/grid.css" rel="stylesheet" />   
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../scripts/jquery.blockUI.js" type="text/javascript"></script>

    <script type = "text/javascript">
    function BlockUI(elementID)
    {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(function(){
            $("#" + elementID).block({ message: '<table><tr><td>' + '<img src="../Images/please_wait.gif"/></td></tr></table>',
            css: {},
            overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6 }
            });
        });
        prm.add_endRequest(function() {
            $("#" + elementID).unblock();
        });
        }
        $(document).ready(function() {
            BlockUI("divMain"); //"<%=lvTimesheet.ClientID %>");  divMain
            $.blockUI.defaults.css = {};

        });

    </script>

</asp:content>
<asp:content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:scriptmanager ID="scriptManager" runat="server" />
<%--<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"/>--%>
   
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
                        <asp:updatepanel ID="updatePanel" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate>      
                                Year&nbsp; :
                                <asp:DropDownList ID="ddlDateYear" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="ddlDateYear_SelectedIndexChanged"/>                                
                                &nbsp;&nbsp;&nbsp; Week&nbsp;&nbsp;:
                                <asp:DropDownList ID="ddlWeekNo" runat="server" AutoPostBack="True" 
                                    DataTextField="Week" DataValueField="WeekNo" 
                                    onselectedindexchanged="ddlWeekNo_SelectedIndexChanged"  />   
                                Emp Name&nbsp;:<asp:DropDownList ID="ddlEmpName" runat="server" AutoPostBack="True" 
                                    DataTextField="Name" DataValueField="Id" 
                                    onselectedindexchanged="ddlEmpName_SelectedIndexChanged"  />
                                <br />
<%--                                <asp:comparevalidator runat="server" Id="chkStartIsDate"
                                      errormessage="The date must be less than today"
                                      controltovalidate="txtStartOfWeek" type="Date" Operator="LessThanEqual" ForeColor="Red" />    --%>                               
                                    <%--<ajaxToolkit:CalendarExtender ID="calStartOfWeek1" runat="server" 
                                    TargetControlID="txtStartOfWeek" FirstDayOfWeek="Monday" ondatabinding="calStartOfWeek1_DataBinding" Format="dd/MM/yyyy"/>--%>
                                <br />
                                <asp:Label ID="lblDate" runat="server" Text=""></asp:Label> 
                                <div id="divMain" style=""> 

                                <div id="div1" style=" float:right">  
                                <asp:ImageButton ID="imgbSave" runat="server" AlternateText="Save" Text="Save" ImageUrl="~/Images/save_icon_mono.gif" OnClick="imgbSave_Click" />
                                </div>
                                <asp:ListView ID="lvTimesheet" runat="server" 
                                    DataSourceID="edsTimesheetDataSource" 
                                    onitemdatabound="lvTimesheet_ItemDataBound" 
                                        onprerender="lvTimesheet_PreRender" >
                                    <LayoutTemplate>
                                        <table class="gridview" cellpadding="0" cellspacing="0" border="0">
                                            <tr class="lightheader">
                                                
                                                <th >Name</th><th></th>
                                                <th >Mon</th>
                                                <th>Tue</th>
                                                <th>Wed</th>
                                                <th>Thu</th>
                                                <th>Fri</th>
                                                <th>Sat</th>
                                                <th>Sun</th>
                                                <th><%--<asp:ImageButton ID="imgbSave" runat="server" AlternateText="Save" Text="Save" ImageUrl="~/Images/save_icon_mono.gif" OnClick="imgbSave_Click" />--%></th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server" />
                                            <tr id="Tr1" runat="server" style="background-color: Silver" align="center">
                                                <td id="Td1" runat="server" colspan="10">
                                                    <asp:DataPager ID="DataPager1" runat="server" PageSize="10" >
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
                                            <td>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("Id") %>' Visible="false" /><%# Container.DataItemIndex+1 %>
                                                <asp:Label ID="Label1" runat="server" Text='<%# GetEmpName((int)Eval("EMPId")) %>' />
                                            </td>
                                            <td>
                                                <div style="position:relative; top:2px">Start</div>
                                                <div style="position:relative; top:6px">Finish</div>
                                                <div style="position:relative; top:12px; visibility:hidden">Total</div>
                                            </td>                                         
                                            <td>
                                                <div style="position:relative"><asp:TextBox ID="txtMonTimeIn" runat="server" Text='<%# Bind("MonTimeIn") %>' Width="36" MaxLength="5" /></div>
                                                <div><asp:TextBox ID="txtMonTimeOut" runat="server" Text='<%# Bind("MonTimeOut") %>' Width="36"  MaxLength="5" /></div>
                                                <div  style="position:relative; top:6px"><asp:Label ID="lblMonSum" runat="server" Text=""></asp:Label></div>
                                            </td>                                                        
                                            <td>
                                                <div><asp:TextBox ID="txtTueTimeIn" runat="server" Text='<%# Bind("TueTimeIn") %>' Width="36"  MaxLength="5" /></div>
                                                <div><asp:TextBox ID="txtTueTimeOut" runat="server" Text='<%# Bind("TueTimeOut") %>' Width="36"  MaxLength="5" /></div>   
                                                <div><asp:Label ID="lblTueSum" runat="server" Text=""></asp:Label></div>                                           
                                            </td>
                                            <td>
                                                <div><asp:TextBox ID="txtWedTimeIn" runat="server" Text='<%# Bind("WedTimeIn") %>'  Width="36"  MaxLength="5" /></div>
                                                <div><asp:TextBox ID="txtWedTimeOut" runat="server" Text='<%# Bind("WedTimeOut") %>' Width="36"  MaxLength="5" /></div>    
                                                <div><asp:Label ID="lblWedSum" runat="server" Text=""></asp:Label></div>                                             
                                            </td>
                                            <td>
                                                <div><asp:TextBox ID="txtThuTimeIn" runat="server" Text='<%# Bind("ThuTimeIn") %>'  Width="36"  MaxLength="5" /></div>
                                                <div><asp:TextBox ID="txtThuTimeOut" runat="server" Text='<%# Bind("ThuTimeOut") %>'  Width="36"  MaxLength="5" /></div>    
                                                <div><asp:Label ID="lblThuSum" runat="server" Text=""></asp:Label></div>                                          
                                            </td>
                                            <td>
                                                <div><asp:TextBox ID="txtFriTimeIn" runat="server" Text='<%# Bind("FriTimeIn") %>'  Width="36"  MaxLength="5" /></div>
                                                <div><asp:TextBox ID="txtFriTimeOut" runat="server" Text='<%# Bind("FriTimeOut") %>' Width="36"  MaxLength="5" /></div>     
                                                <div><asp:Label ID="lblFriSum" runat="server" Text=""></asp:Label></div>                                        
                                            </td>
                                            <td>
                                                <div><asp:TextBox ID="txtSatTimeIn" runat="server" Text='<%# Bind("SatTimeIn") %>' Width="36"  MaxLength="5" /></div>
                                                <div><asp:TextBox ID="txtSatTimeOut" runat="server" Text='<%# Bind("SatTimeOut") %>'  Width="36"  MaxLength="5" /></div>
                                                <div><asp:Label ID="lblSatSum" runat="server" Text=""></asp:Label></div>
                                            </td>
                                            <td>
                                            <div><asp:TextBox ID="txtSunTimeIn" runat="server" Text='<%# Bind("SunTimeIn") %>' Width="36"  MaxLength="5" /></div>
                                            <div><asp:TextBox ID="txtSunTimeOut" runat="server" Text='<%# Bind("SunTimeOut") %>'  Width="36"  MaxLength="5" /></div>   
                                            <div><asp:Label ID="lblSunSum" runat="server" Text=""></asp:Label></div>                                           
                                            </td>
                                            <td></td>
                                        </tr>
                                    </ItemTemplate>
                                    <emptyitemtemplate>
                                        <tr>
                                            <td>
                                                empty Item that isn't displaying
                                            </td>
                                        </tr>
                                    </emptyitemtemplate>
                                    <emptydatatemplate>
                                        <p>
                                            Empty Data that will be displayed.</p>
                                    </emptydatatemplate>
                                </asp:ListView>
                                </div>  
                                    <script type="text/javascript" language="javascript">
                                     function pageLoad(sender, args)
                                     {
                                            $("input").focus(function () {
                                                var currentId = $(this).attr('id');
                                                //alert(currentId);
                                                if (currentId.indexOf("TimeIn") >= 0 && !$(this).val()) {
                                                    $(this).val("7.00");
                                                    $(this).select();
                                                }
                                                else if (currentId.indexOf("LunchBreak") >= 0 && !$(this).val()) {
                                                    $(this).val("00:30");
                                                    $(this).select();
                                                }
                                                else if (currentId.indexOf("TimeOut") >= 0 && !$(this).val()) {
                                                    $(this).val("15.00");
                                                    $(this).select();

                                                    $(this).keyup(function () {
                                                        calculateSum();
                                                    });
                                                }
                                            });


                                            /*keydown*/
                                            $("input").keydown(function (event) {

                                                // Allow: backspace, delete, tab, escape, and enter
                                                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 ||
                                                // Allow: Ctrl+A
                                                    (event.keyCode == 65 && event.ctrlKey === true) ||
                                                // Allow: Ctrl+C
                                                    (event.keyCode == 67 && event.ctrlKey === true) ||
                                                // Allow: Ctrl+V
                                                    (event.keyCode == 86 && event.ctrlKey === true) ||
                                                // Allow: home, end, left, right
                                                    (event.keyCode >= 35 && event.keyCode <= 39)) {
                                                    // let it happen, don't do anything
                                                    // entering "0" if the third digit is not .
                                                    if (this.value.length == 1 && event.keyCode == 190) {
                                                        this.value = '0' + this.value;
                                                    }
                                                    return;
                                                }
                                                else if (this.value.length == 2 && event.keyCode != 190 && (event.keyCode >= 48 || event.keyCode <= 57)
                                                        ) {
                                                    this.value = this.value + '.';
                                                }
                                                else {
                                                    // Ensure that it is a number and stop the keypress
                                                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                                                        event.preventDefault();
                                                    }
                                                }
                                            });

                                            $("input").keyup(function (event) {

                                            });
                                             /*                 KeyDown                     */




                                            $("input").blur(function () {
                                                var currentId = $(this).attr('id');
                                                var inputVal = $(this).val();

                                                if ($(this).val()) {
                                                    var dateReg = /^([0-1][0-9]|([2][0-3])|([0-9])|([0-9])).[0-5][0-9](\\s)?$/;
                                                    var dtArray = inputVal.match(dateReg); // is format OK?
                                                    $(this).nextAll('span').remove();
                                                    if (dtArray == null) {
                                                        $(this).after('<span class="error error-keyup-4">HH.mm</span>');
                                                        return false;
                                                    }
                                                }

                                                // if (!characterReg.test(inputVal)) {
                                                //     $(this).after('<span class="error error-keyup-4">Incorrect time format fool!: HH:mm am|pm</span>');
                                                // }
                                                if (currentId.indexOf("TimeOut") >= 0 && $(this).val()) {

                                                    var TimeOut = $(this).val();
                                                    TimeOut = TimeOut.replace(".", ":");
                                                    TimeOut = new Date("Aug 08 2012 ") + TimeOut;
                                                    //alert(new Date("Aug 08 2012 ") + TimeOut);
                                                    var day = currentId.substr(27, 3);
                                                    var no = currentId.substr(38, 1);
                                                    var TimeIn = ($("#MainContent_lvTimesheet_txt" + day + "TimeIn_" + no).val());
                                                    TimeIn = TimeIn.replace(".", ":");
                                                    TimeIn = new Date("Aug 08 2012 ") + TimeIn;

                                                    //var diff =  Math.abs(TimeOut.getTime()-TimeIn.getTime());
                                                    //alert(diff);


                                                    // $("#MainContent_lvTimesheet_lbl" + day + "Sum_" + no).html(TimeOut - TimeIn);
                                                }
                                            });
                                        }
                                        function calculateSum() {                                            
                                            var sum = 10;
                                            $("#lblFriSum").html(sum.toFixed(2));
                                        }                                                                                                        
                                    </script>                           
                            </ContentTemplate>
                            <Triggers>                            
                            <asp:AsyncPostBackTrigger ControlID = "imgbSave" />
                            <asp:AsyncPostBackTrigger ControlID = "ddlEmpName" />
                            </Triggers> 
                        </asp:updatepanel>
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
        <asp:entitydatasource ID="edsTimesheetDataSource"   runat="server" ContextTypeName="ArlecClock.DAL.WMSDBEntities"
        AutoGenerateWhereClause="false" EnableFlattening="False" 
        EntitySetName="EmpTimesheets" EnableUpdate="True" EnableInsert="True" EnableDelete="True"        
        Where="it.EmpId != 56 AND it.EmpId != 59 AND (it.WeekNo = @WeekNo OR @WeekNo IS NULL) AND (it.DateYear = @DateYear OR @DateYear IS NULL) AND (it.EmpId = @EmpId OR @EmpId IS NULL)" 
        >
            <WhereParameters>
            <asp:ControlParameter ControlID="ddlWeekNo" Name="WeekNo" PropertyName="Text" Type="Int16" />
            <asp:ControlParameter ControlID="ddlDateYear" Name="DateYear" PropertyName="Text" Type="Int16" />
            <asp:ControlParameter ControlID="ddlEmpName" Name="EmpId"  PropertyName="SelectedValue" Type="Int16" />
        </WhereParameters>
    </asp:entitydatasource>
</asp:content>
