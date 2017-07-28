using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using ArlecClock.DAL;
using System.Globalization;
using System.Web.Services;

namespace ArlecClock.Pages
{
    public partial class Tracker : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            String LoginType = Request["type"];
            if (LoginType != null)
            {
                if (LoginType == "IN")
                {
                    ClockIN.CssClass = "Active";
                   
                    ScanMessage.Text = "Clock IN";
                   

                }
                if (LoginType == "OUT")
                {
                    ClockOut.CssClass = "Active";
                    
                    ScanMessage.Text = "Clock Out";


                }
            } 

            LoginId.Focus(); 


        }

        protected void ClockIN_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("Tracker.aspx?type=IN&Admin=" + Session["ID"] + "");
        }

        protected void ClockOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("Tracker.aspx?type=OUT&Admin=" + Session["ID"] + "");

        }

        protected void tmrUpdate_Tick(object sender, EventArgs e)
        {

            ///* Time taking from Database server and Passing into Label*/
            //var DBcontext = new WMSDBEntities();
            //var dateQuery = DBcontext.CreateQuery<DateTime>("CurrentDateTime() ");
            //DateTime dateFromSql = dateQuery.AsEnumerable().First();
            
             lblTime.Text = DateTime.Now.ToString("T");
           //  lblTime.Text = dateFromSql.ToString("T");
        }


        public string ProcessMyDataItem(object valObj)
        {
            /*This funtion returning Message baise on AM and PM after scan*/

            string time = valObj.ToString();
            time.Substring(time.Length - 2, 2);

            if (time.Substring(time.Length - 2, 2) == "AM")
            {
                return "Good Morning!";
            }
            else
            {
                return "Have a Nice Day";
            }
        }


        public void LoginId_TextChanged(object sender, EventArgs e)
        {




            if (!string.IsNullOrEmpty(Request["type"]))
            {
                if (string.IsNullOrEmpty(LoginId.Text))
                {
                    /* below code is clearing not found message  */
                    MissingQueryString.InsertItemPosition = InsertItemPosition.None;
                    MissingQueryString.DataBind();
                    lvLocationDetails.InsertItemPosition = InsertItemPosition.None;
                    lvLocationDetails.DataBind();
                    //alreadyClockIN.InsertItemPosition = InsertItemPosition.None;
                    //alreadyClockIN.DataBind();

                    return;
                }
                else
                {


                    String LoginType = Request["type"];
                    String ScanUpdateReturnValue = "";
                    String LoginID = "";
                    object sessionVal = Request["Admin"];

                    String Password = "abcdef1234";
                    LoginID = new ArlecClock.App_Code.TrackTime().Decrypt(LoginId.Text, Password);
                    ScanUpdateReturnValue = new ArlecClock.App_Code.TrackTime().LoginAfterScan(sessionVal, LoginID, LoginType);

                    if (ScanUpdateReturnValue != "")
                    {

                        Session["LoginId"] = ScanUpdateReturnValue;
                        LoginId.Text = "";
                        lvLocationDetails.InsertItemPosition = InsertItemPosition.None;
                        lvLocationDetails.DataBind();

                    }
                    if (ScanUpdateReturnValue == "")
                    {
                        /* showing Message if record is not found*/

                        //alreadyClockIN.InsertItemPosition = InsertItemPosition.None;
                        //alreadyClockIN.DataBind();
                        lvLocationDetails.InsertItemPosition = InsertItemPosition.LastItem;
                        lvLocationDetails.DataBind();
                        Session["LoginId"] = null;
                        LoginId.Text = "";

                    }

                }
            }
            if (string.IsNullOrEmpty(Request["type"]))
            {
                MissingQueryString.InsertItemPosition = InsertItemPosition.LastItem;
                MissingQueryString.DataBind();
            }
        }

        protected void LoginId_TextChanged1(object sender, EventArgs e)
        {

        }

   

      



    }
}