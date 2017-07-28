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
    public partial class EncryptionAndDecryption : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Arlec_TimesheetConnectionString"].ToString());
            String Query = "SELECT LoginID From Employees Where IsActive=1 AND LoginID IS NOT null";
            sqlConnection1.Open();
           SqlDataAdapter DA = new SqlDataAdapter(Query, sqlConnection1);
           DataTable Dt = new DataTable();
           Dt.Columns.Add("LoginID");
           Dt.Columns.Add("EncryptStrings");
           DA.Fill(Dt);
            String Password = "abcdef1234";
        
           var EncryptClass = new ArlecClock.App_Code.TrackTime();
           foreach (DataRow dr in Dt.Rows)
           {
               dr["EncryptStrings"]=EncryptClass.Encrypt(dr["LoginID"].ToString(), Password);

           }

           GridView1.DataSource = Dt;
           GridView1.DataBind();
            
            
           
        }
    }
}