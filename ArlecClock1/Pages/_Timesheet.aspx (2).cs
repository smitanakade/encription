using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ArlecEmpTimesheet.DAL;
namespace ArlecEmpTimesheet.Pages
{
    public partial class Timesheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                chkStartIsDate.ValueToCompare = GetWeekEndDate(DateTime.Now);
                GenerateTimesheetListView();
            }
        }
        public string GetWeekEndDate( DateTime dateTime)
        {
            string numberofdays = Convert.ToDateTime(dateTime.ToShortDateString()).DayOfWeek.ToString("d"); ;
            int numofdays = Convert.ToInt32(numberofdays);
            numofdays = (numofdays == 0) ? 0 : 7 - numofdays;
            DateTime select = Convert.ToDateTime(dateTime);
            
            return select.AddDays(numofdays).ToString("dd/MM/yyyy");
        }
        /*protected void calStartOfWeek1_DataBinding(object sender, EventArgs e)
        {
            string numberofdays = Convert.ToDateTime(this.txtStartOfWeek.Text).DayOfWeek.ToString("d");
            DateTime select = Convert.ToDateTime(txtStartOfWeek.Text);
            string lblStartOfWeek = select.AddDays(-Convert.ToInt32(numberofdays)).ToString("dd MMMM yyyy");
            string lblEndOfWeek = select.AddDays(6 - Convert.ToInt32(numberofdays)).ToString("dd MMMM yyyy");
        }*/
        public void GenerateTimesheetListView()
        {
            if (this.txtStartOfWeek.Text == "")
            {
                this.txtStartOfWeek.Text = GetWeekEndDate(DateTime.Now);
            }

            DateTime startofWeek = Convert.ToDateTime(this.txtStartOfWeek.Text);
            var context= new WMSDBEntities();
            var empTimesheets = (from p in context.EmpTimesheets
                                    where p.StartOfTheWeek == startofWeek 
                                    select p);
            /*var employeestemp = (from emp in context.Employees
                                 join item in context.EmpTimesheets on emp.Id equals item.EmpId
                                 where item.StartOfTheWeek == startofWeek
                                 select emp); 

            var employeestemp = (from emp in context.Employees
                                 join item in context.EmpTimesheets on emp.Id equals item.EmpId 
                                 where item.StartOfTheWeek == startofWeek into match
                                 select match);*/



            var employeestemp = from emp in context.Employees 
                join item in context.EmpTimesheets.Where(cal => cal.StartOfTheWeek == startofWeek) on emp.Id equals item.EmpId                                 
                                into matchemp
                from subemp in matchemp.DefaultIfEmpty()
                where subemp == null
                select emp;

            /*var employeestemp = (from emp in context.Employees                                 
                                 where emp.Id in (
                                 from item in context.EmpTimesheets 
                                     where item.StartOfTheWeek == startofWeek 
                                     select item.EmpId);                                 
                                 select emp);*/

            if (employeestemp.Count() != 0)
            {
                foreach (ArlecEmpTimesheet.DAL.Employee temp in employeestemp)
                {
                    ArlecEmpTimesheet.DAL.EmpTimesheet newEmpTimesheet = new EmpTimesheet();
                    newEmpTimesheet.StartOfTheWeek = startofWeek;
                    newEmpTimesheet.EmpId = temp.Id;
                    context.EmpTimesheets.AddObject(newEmpTimesheet);
                }
                context.SaveChanges();
            }
            /*if (empTimesheets.Count() == 0)
            {
                var employees = (from e in context.Employees
                                 select e);
                foreach (ArlecEmpTimesheet.DAL.Employee temp in employees)
                {
                    ArlecEmpTimesheet.DAL.EmpTimesheet newEmpTimesheet = new EmpTimesheet();
                    newEmpTimesheet.StartOfTheWeek = startofWeek;
                    newEmpTimesheet.EmpId = temp.Id;
                    context.EmpTimesheets.AddObject(newEmpTimesheet);
                }
                context.SaveChanges();
            }*/
        }
        public string GetEmpName(int empId)
        {
            var context = new WMSDBEntities();
            var employee = (from e in context.Employees
                                where e.Id == empId
                                select e);
            ArlecEmpTimesheet.DAL.Employee emp = employee.First();
            return emp.FirstName;
                //ArlecEmpTimesheet.DAL.Employee
        }
        protected void imgbSave_Click(object sender, ImageClickEventArgs e)
        {
            using (var context = new WMSDBEntities())
            {
                TextBox txtTimeToSave;

                foreach (ListViewDataItem i in this.lvTimesheet.Items)
                {
                    ArlecEmpTimesheet.DAL.EmpTimesheet empTimesheet; 
                    Label itemID = (Label)(i.FindControl("lblID"));
                    int editIndex = Convert.ToInt32( itemID.Text);
                    
                    empTimesheet = context.EmpTimesheets.Where(s => s.Id == editIndex).FirstOrDefault<EmpTimesheet>();
                    // change student name in disconnected mode (out of DBContext scope)
                    if (empTimesheet != null)
                    {
                        txtTimeToSave = (TextBox)(i.FindControl("txtMonTimeIn"));
                        empTimesheet.MonTimeIn = (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));
                        //empTimesheet.MonTimeIn = Convert.ToDateTime(txtTimeToSave.Text);

                        txtTimeToSave = (TextBox)(i.FindControl("txtMonTimeOut"));
                        empTimesheet.MonTimeOut = (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text)); 

                        txtTimeToSave = (TextBox)(i.FindControl("txtTueTimeIn"));
                        empTimesheet.TueTimeIn = (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));
                        txtTimeToSave = (TextBox)(i.FindControl("txtTueTimeOut"));
                        empTimesheet.TueTimeOut = (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));

                        txtTimeToSave = (TextBox)(i.FindControl("txtWedTimeIn"));
                        empTimesheet.WedTimeIn = (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));
                        txtTimeToSave = (TextBox)(i.FindControl("txtWedTimeOut"));
                        empTimesheet.WedTimeOut = (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));

                        txtTimeToSave = (TextBox)(i.FindControl("txtThuTimeIn"));
                        empTimesheet.ThuTimeIn = (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));
                        txtTimeToSave = (TextBox)(i.FindControl("txtThuTimeOut"));
                        empTimesheet.ThuTimeOut = (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));

                        txtTimeToSave = (TextBox)(i.FindControl("txtFriTimeIn"));
                        empTimesheet.FriTimeIn = (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));
                        txtTimeToSave = (TextBox)(i.FindControl("txtFriTimeOut"));
                        empTimesheet.FriTimeOut = (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));

                        txtTimeToSave = (TextBox)(i.FindControl("txtSatTimeIn"));
                        empTimesheet.SatTimeIn= (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));
                        txtTimeToSave = (TextBox)(i.FindControl("txtSatTimeOut"));
                        empTimesheet.SatTimeOut= (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));

                        txtTimeToSave = (TextBox)(i.FindControl("txtSunTimeIn"));
                        empTimesheet.SunTimeIn = (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));
                        txtTimeToSave = (TextBox)(i.FindControl("txtSunTimeOut"));
                        empTimesheet.SunTimeOut = (txtTimeToSave.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtTimeToSave.Text));
                    }
                    //save modified entity using new DBContext
                    context.SaveChanges();                        
                }                
            }
        }



        /*protected void txtStartOfWeek_TextChanged(object sender, EventArgs e)
        {
            string numberofdays = Convert.ToDateTime(this.txtStartOfWeek.Text).DayOfWeek.ToString("d");
            DateTime select = Convert.ToDateTime(txtStartOfWeek.Text);
            string lblStartOfWeek = select.AddDays(-Convert.ToInt32(numberofdays)).ToString("dd MMMM yyyy");
            this.txtStartOfWeek.Text = select.AddDays(-Convert.ToInt32(numberofdays)+1).ToString("dd/MM/yyyy");            
        }*/

        protected void calStartOfWeek_SelectionChanged(object sender, EventArgs e)
        {
            string numberofdays = Convert.ToDateTime(calStartOfWeek.SelectedDate).DayOfWeek.ToString("d");
            int numofdays = Convert.ToInt32(numberofdays);
            numofdays = (numofdays == 0) ? 0 : 7- numofdays;
            DateTime select = Convert.ToDateTime(calStartOfWeek.SelectedDate);
            string lblStartOfWeek   = select.AddDays(-Convert.ToInt32(numberofdays)).ToString("dd MMMM yyyy");
            this.txtStartOfWeek.Text = select.AddDays(numofdays).ToString("dd/MM/yyyy");
            chkStartIsDate.Validate();
            if(chkStartIsDate.IsValid)
                GenerateTimesheetListView();
        }

        protected void lvTimesheet_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }
    }
}