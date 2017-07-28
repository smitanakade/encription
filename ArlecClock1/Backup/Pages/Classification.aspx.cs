using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ArlecEmpTimesheet.Pages
{
    public partial class Classification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void imgbNew_Click(object sender, ImageClickEventArgs e)
        {
            lvClassifications.InsertItemPosition = InsertItemPosition.FirstItem;
            lvClassifications.EditIndex = -1;
        }

        protected void lvClassifications_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = true;
        }

        protected void lvClassifications_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                lvClassifications.InsertItemPosition = InsertItemPosition.None;
            }
        }
    }
}
