using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ArlecEmpTimesheet.DAL;
using System.Web.Security;

namespace ArlecEmpTimesheet
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // remove manage user accounts menu item for non-admin users.

            if (!Roles.IsUserInRole("Admin"))
            {
                MenuItem item = NavigationMenu.FindItem("Employee/Manage Employees");
                item.Parent.ChildItems.Remove(item);
                item = NavigationMenu.FindItem("Classification");
                NavigationMenu.Items.Remove(item);
            }
        }
        //public bool IsAdmin(string strName)
        //{
        //    var context = new WMSDBEntities();
        //    //var empTimesheets = (from p in context.EmpTimesheets
        //    //                     where p.WeekNo == iWeekNo
        //    //                     select p);.Where(e => e.FirstName.Contains("/"+strEmpName+"/") )

        //    var users = from user in context.Users.Where(u => u.UserName == strName)
        //                join urole in context.UserRoles.Where(r => r.RoleId == 1) on user.UserId equals urole.UserId
        //                select user;

        //    if (users.Count() > 0 )
        //        return true;

        //    return false;
        //}
    }
}
