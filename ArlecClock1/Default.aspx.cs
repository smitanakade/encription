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

namespace ArlecClock
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AdminLogin.Focus();
        }

        protected void AdminLogin_TextChanged(object sender, EventArgs e)
        {
            AdminRecordResult.InsertItemPosition = InsertItemPosition.None;
            System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Arlec_TimesheetConnectionString"].ToString());
            String AdminLoginID = "";
            String Password = "abcdef1234";
            
            AdminLoginID = new ArlecClock.App_Code.TrackTime().Decrypt(AdminLogin.Text, Password);
            /*Get employee id from Employee table through Loginid */
            String Query = "SELECT Id FROM [WMSDB].[dbo].[Employees] WHERE LoginId='" + AdminLoginID + "' AND RemoteLogin =1";
            SqlCommand sqlCmd = new SqlCommand(Query, sqlConn);

            sqlConn.Open();
            var AdminId = sqlCmd.ExecuteScalar();
            if (AdminId != null)
            {
                Session["ID"] = "Y";
                Server.Transfer("Tracker.aspx", true);
            }
            else
            {
                AdminLogin.Text = "";
                /* showing Message if record is not found*/
                AdminRecordResult.InsertItemPosition = InsertItemPosition.LastItem;
                AdminRecordResult.DataBind();
            }
        }
    }
}
