using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ArlecClock
{
    public partial class SelectPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ClockIN_Click(object sender, EventArgs e)
        {
            Response.Redirect("Tracker.aspx?type=IN&Admin=" + Session["ID"]+"");
        }

        protected void ClockOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("Tracker.aspx?type=OUT&Admin=" + Session["ID"] + "");

        }

        
    }
}