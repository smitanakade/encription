using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ArlecEmpTimesheet.Models;
using System.Globalization;
using ArlecEmpTimesheet.DAL;

namespace ArlecEmpTimesheet.Pages
{
    public partial class Employee : System.Web.UI.Page
    {
        private DropDownList clsDropDownList;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateDropDownLists();
            }
        }
        private void PopulateDropDownLists()
        {
            using (var context = new WMSDBEntities())
            {

                var employees = context.Get_Employees();
                ddlEmpName.DataSource = employees;
                ddlEmpName.DataBind();
                //ddlEmpName.SelectedValue = GetWeekNumber(DateTime.Now).ToString();
                ddlEmpName.Visible = true;
            }
        }
        protected void imgbNew_Click(object sender, ImageClickEventArgs e)
        {
            lvEmployees.InsertItemPosition = InsertItemPosition.FirstItem;
            lvEmployees.EditIndex = -1;
        }

        protected void imgbNew_Click1(object sender, ImageClickEventArgs e)
        {

        }

        protected void ClassificationsDropDownList_Init(object sender, EventArgs e)
        {
            clsDropDownList = sender as DropDownList;
            //clsDropDownList.SelectedIndex = 3;
        }

        protected void EmployeesDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            var ClassificationId = Convert.ToInt32(clsDropDownList.SelectedValue);
            e.Values["ClassificationId"] = ClassificationId;
        }

        protected void lvEmployees_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            var ClassificationId            = Convert.ToInt32(clsDropDownList.SelectedValue);
            e.Values["ClassificationId"]    = ClassificationId;
            e.Values["HireDate"]  = (e.Values["HireDate"] ==  null ?  (DateTime?)null : Convert.ToDateTime(e.Values["HireDate"]) );
            e.Values["BirthDate"]  = (e.Values["BirthDate"] == null ? (DateTime?)null : Convert.ToDateTime(e.Values["BirthDate"]));
        }

        protected void lvEmployees_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.NewValues["HireDate"] = (e.NewValues["HireDate"] == null ? (DateTime?)null : Convert.ToDateTime(e.NewValues["HireDate"]));
            e.NewValues["BirthDate"] = (e.NewValues["BirthDate"] == null ? (DateTime?)null : Convert.ToDateTime(e.NewValues["BirthDate"]));
            var ClassificationId = Convert.ToInt32(clsDropDownList.SelectedValue);
            e.NewValues["ClassificationId"] = ClassificationId;
        }

        protected void lvEmployees_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
        }

        protected void lvEmployees_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                lvEmployees.InsertItemPosition = InsertItemPosition.None;
            }
        }

        protected void ddlEmpName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
