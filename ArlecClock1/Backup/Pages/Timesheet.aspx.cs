using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ArlecEmpTimesheet.DAL;
using System.Globalization;

namespace ArlecEmpTimesheet.Pages
{
    public partial class Timesheet : System.Web.UI.Page
    {
        private decimal LunchBreakMin = 0.30m;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateDropDownLists();
                //ddlWeekNo.SelectedValue = "1";
                GenerateTimesheetListView();
                
            }
            FirstDateOfWeek(Convert.ToInt32(ddlDateYear.SelectedValue), Convert.ToInt32(ddlWeekNo.SelectedValue));
        }
        public DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            
            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            lblDate.Text = "<b>From    : </b>" + result.AddDays(-3).ToString("MMMM dd, yyyy") + "    <b>To    : </b>" + result.AddDays(3).ToString("MMMM dd, yyyy");

            if ((year == DateTime.Now.Year && weekOfYear <= GetWeekNumber(DateTime.Now)) || (year < DateTime.Now.Year))
            {
                GenerateTimesheetListView();
                imgbSave.Visible = true;
            }
            else
            {
                lblDate.Text = lblDate.Text + "<br/><br/>  <p style='color:red'> You can't enter time for future weeks. </p>";
                imgbSave.Visible = false;
            }
            return result.AddDays(-3);
        }
        private void PopulateDropDownLists()
        {
            using (var context = new WMSDBEntities())
            {
                var allYears = context.Get_Years();
                ddlDateYear.DataSource = allYears;
                ddlDateYear.DataBind();
                ddlDateYear.SelectedValue = DateTime.Now.Year.ToString();

                var allWeekNo = context.Get_WeekNo(0);
                ddlWeekNo.DataSource = allWeekNo;
                ddlWeekNo.DataBind();
                ddlWeekNo.SelectedValue = GetWeekNumber(DateTime.Now).ToString();
                ddlWeekNo.Visible = true;


                var employees = context.Get_Employees();
                ddlEmpName.DataSource = employees;
                ddlEmpName.DataBind();
                //ddlEmpName.SelectedValue = GetWeekNumber(DateTime.Now).ToString();
                ddlEmpName.Visible = true;
            }
        }
        public static int GetWeekNumber(DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
        public string GetWeekEndDate( DateTime dateTime)
        {
            string numberofdays = Convert.ToDateTime(dateTime.ToShortDateString()).DayOfWeek.ToString("d");
            int numofdays       = Convert.ToInt32(numberofdays);
            numofdays           = (numofdays == 0) ? 0 : 7 - numofdays;
            DateTime select = Convert.ToDateTime(dateTime);
            return select.AddDays(numofdays).ToString("dd/MM/yyyy");
        }

        public void GenerateTimesheetListView()
        {
            int iWeekNo = Convert.ToInt16(ddlWeekNo.SelectedItem.Value);
            int iDateYear = Convert.ToInt16(ddlDateYear.SelectedItem.Value);
            //string strEmpName = txtEmpName.Text == string.Empty ? "" : txtEmpName.Text;
            var context = new WMSDBEntities();
            //var empTimesheets = (from p in context.EmpTimesheets
            //                     where p.WeekNo == iWeekNo
            //                     select p);.Where(e => e.FirstName.Contains("/"+strEmpName+"/") )

            var employeestemp = from emp in context.Employees.Where(e => e.IsActive == 1)
                                join item in context.EmpTimesheets.Where(cal => cal.WeekNo == iWeekNo).Where(cal => cal.DateYear == iDateYear) on emp.Id equals item.EmpId into matchemp
                                from subemp in matchemp.DefaultIfEmpty()
                                where subemp == null
                                select emp;

            if (employeestemp.Count() != 0)
            {
                foreach (ArlecEmpTimesheet.DAL.Employee temp in employeestemp)
                {
                    ArlecEmpTimesheet.DAL.EmpTimesheet newEmpTimesheet = new EmpTimesheet();
                    newEmpTimesheet.WeekNo = Convert.ToInt16(ddlWeekNo.SelectedItem.Value);
                    newEmpTimesheet.DateYear = Convert.ToInt16(ddlDateYear.SelectedItem.Value);
                    newEmpTimesheet.EmpId = temp.Id;
                    context.EmpTimesheets.AddObject(newEmpTimesheet);
                }
                context.SaveChanges();
            }

        }
        public string GetEmpName(int empId)
        {
            var context = new WMSDBEntities();
            var employee = (from e in context.Employees
                            where e.Id == empId
                            select e);
            ArlecEmpTimesheet.DAL.Employee emp = employee.First();
            return emp.FirstName + " " + emp.LastName;
            //ArlecEmpTimesheet.DAL.Employee
        }
        protected void imgbSave_Click(object sender, ImageClickEventArgs e)
        {

                using (var context = new WMSDBEntities())
                {
                    TextBox txtTimeToSave;
                    int iMealAllow;
                    try
                    {
                        foreach (ListViewDataItem i in this.lvTimesheet.Items)
                        {
                            ArlecEmpTimesheet.DAL.EmpTimesheet empTimesheet;
                            ArlecEmpTimesheet.DAL.EmpTimesheetsSummary empTimesheetsSummary;
                            Label itemID = (Label)(i.FindControl("lblID"));
                            int editIndex = Convert.ToInt32(itemID.Text);
                            empTimesheet = context.EmpTimesheets.Where(s => s.Id == editIndex).FirstOrDefault<EmpTimesheet>();
                            empTimesheetsSummary = context.EmpTimesheetsSummaries.Where(s => s.TimesheetID == editIndex).FirstOrDefault<EmpTimesheetsSummary>();
                            iMealAllow = 0;
                            if (empTimesheetsSummary == null)
                            {
                                empTimesheetsSummary = new EmpTimesheetsSummary();
                                empTimesheetsSummary.TimesheetID = editIndex;
                                empTimesheetsSummary.EmpId = (int)empTimesheet.EmpId;
                                empTimesheetsSummary.WeekNo = Convert.ToInt16(ddlWeekNo.SelectedItem.Value);// empTimesheet.StartOfTheWeek;
                                empTimesheetsSummary.DateYear = Convert.ToInt16(ddlDateYear.SelectedItem.Value);
                                context.AddToEmpTimesheetsSummaries(empTimesheetsSummary);
                            }
                            // change student name in disconnected mode (out of DBContext scope)
                            if (empTimesheet != null)
                            {
                                txtTimeToSave = (TextBox)(i.FindControl("txtMonTimeIn"));
                                empTimesheet.MonTimeIn = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));
                                txtTimeToSave = (TextBox)(i.FindControl("txtMonTimeOut"));
                                empTimesheet.MonTimeOut = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));

                                ArlecEmpTimesheet.DAL.Employee emp = context.Employees.Where(s => s.Id == (int)empTimesheet.EmpId).FirstOrDefault<ArlecEmpTimesheet.DAL.Employee>();
                                ArlecEmpTimesheet.DAL.Classification cls = context.Classifications.Where(s => s.Id == emp.ClassificationId).FirstOrDefault<ArlecEmpTimesheet.DAL.Classification>();
                                decimal totalTime;
                                if (empTimesheet.MonTimeIn != null && empTimesheet.MonTimeOut != null)
                                {
                                    totalTime = CalcTotal(empTimesheet.MonTimeIn, empTimesheet.MonTimeOut);
                                    empTimesheetsSummary.MonTotal = totalTime;// ConvertToTimeFormat(totalTime);

                                    empTimesheetsSummary.WeekNo = empTimesheet.WeekNo;
                                    empTimesheetsSummary.MonNormalHours = totalTime >= cls.WorkingHours ? cls.WorkingHours : totalTime;
                                    empTimesheetsSummary.MonOTHours = totalTime >= cls.WorkingHours + 2 ? 2 : (totalTime < cls.WorkingHours + 2 && totalTime > cls.WorkingHours) ? totalTime - cls.WorkingHours : 0;
                                    empTimesheetsSummary.MonDoubleHours = totalTime >= cls.WorkingHours + 2 + 2 ? (totalTime - (cls.WorkingHours + 2 )) : (totalTime < cls.WorkingHours + 2 + 2 && totalTime > cls.WorkingHours + 2) ? totalTime - cls.WorkingHours - 2 : 0;
                                    if (cls.MealAllowanceDue == 1 ? (empTimesheetsSummary.MonOTHours > cls.MealAllowanceDue) : (empTimesheetsSummary.MonOTHours >= cls.MealAllowanceDue))
                                        //if (empTimesheetsSummary.MonOTHours >= cls.MealAllowanceDue)
                                        //iMealAllow += (empTimesheetsSummary.MealAllow == null) ? 0 : 1;
                                        iMealAllow += 1;
                                }
                                else
                                {
                                    empTimesheetsSummary.MonTotal = null;
                                    empTimesheetsSummary.MonNormalHours = null;
                                    empTimesheetsSummary.MonOTHours = null;
                                    empTimesheetsSummary.MonDoubleHours = null;
                                }
                                txtTimeToSave = (TextBox)(i.FindControl("txtTueTimeIn"));
                                empTimesheet.TueTimeIn = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));
                                txtTimeToSave = (TextBox)(i.FindControl("txtTueTimeOut"));
                                empTimesheet.TueTimeOut = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));
                                if (empTimesheet.TueTimeIn != null && empTimesheet.TueTimeOut != null)
                                {
                                    totalTime = CalcTotal(empTimesheet.TueTimeIn, empTimesheet.TueTimeOut);
                                    empTimesheetsSummary.TueTotal = totalTime;

                                    empTimesheetsSummary.WeekNo = empTimesheet.WeekNo;
                                    empTimesheetsSummary.TueNormalHours = totalTime >= cls.WorkingHours ? cls.WorkingHours : totalTime;
                                    empTimesheetsSummary.TueOTHours = totalTime >= cls.WorkingHours + 2 ? 2 : (totalTime < cls.WorkingHours + 2 && totalTime > cls.WorkingHours) ? totalTime - cls.WorkingHours : 0;
                                    empTimesheetsSummary.TueDoubleHours = totalTime >= cls.WorkingHours + 2 + 2 ? (totalTime - (cls.WorkingHours + 2)) : (totalTime < cls.WorkingHours + 2 + 2 && totalTime > cls.WorkingHours + 2) ? totalTime - cls.WorkingHours - 2 : 0;
                                    //if (empTimesheetsSummary.TueOTHours >= cls.MealAllowanceDue)
                                    if (cls.MealAllowanceDue == 1 ? (empTimesheetsSummary.TueOTHours > cls.MealAllowanceDue) : (empTimesheetsSummary.TueOTHours >= cls.MealAllowanceDue))
                                        //iMealAllow += (empTimesheetsSummary.MealAllow == null) ? 0 : 1;
                                        iMealAllow += 1;
                                }
                                else
                                {
                                    empTimesheetsSummary.TueTotal = null;
                                    empTimesheetsSummary.TueNormalHours = null;
                                    empTimesheetsSummary.TueOTHours = null;
                                    empTimesheetsSummary.TueDoubleHours = null;
                                }


                                txtTimeToSave = (TextBox)(i.FindControl("txtWedTimeIn"));
                                empTimesheet.WedTimeIn = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));
                                txtTimeToSave = (TextBox)(i.FindControl("txtWedTimeOut"));
                                empTimesheet.WedTimeOut = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));
                                if (empTimesheet.WedTimeIn != null && empTimesheet.WedTimeOut != null)
                                {
                                    totalTime = CalcTotal(empTimesheet.WedTimeIn, empTimesheet.WedTimeOut);
                                    empTimesheetsSummary.WedTotal = totalTime;// ConvertToTimeFormat(totalTime);

                                    empTimesheetsSummary.WedNormalHours = totalTime >= cls.WorkingHours ? cls.WorkingHours : totalTime;
                                    empTimesheetsSummary.WedOTHours = totalTime >= cls.WorkingHours + 2 ? 2 : (totalTime < cls.WorkingHours + 2 && totalTime > cls.WorkingHours) ? totalTime - cls.WorkingHours : 0;
                                    empTimesheetsSummary.WedDoubleHours = totalTime >= cls.WorkingHours + 2 + 2 ? (totalTime - (cls.WorkingHours + 2)) : (totalTime < cls.WorkingHours + 2 + 2 && totalTime > cls.WorkingHours + 2) ? totalTime - cls.WorkingHours - 2 : 0;
                                    //if (empTimesheetsSummary.WedOTHours >= cls.MealAllowanceDue)
                                    if (cls.MealAllowanceDue == 1 ? (empTimesheetsSummary.WedOTHours > cls.MealAllowanceDue) : (empTimesheetsSummary.WedOTHours >= cls.MealAllowanceDue))
                                        //iMealAllow += (empTimesheetsSummary.MealAllow == null) ? 0 : 1;
                                        iMealAllow += 1;
                                }
                                else
                                {
                                    empTimesheetsSummary.WedTotal = null;
                                    empTimesheetsSummary.WedNormalHours = null;
                                    empTimesheetsSummary.WedOTHours = null;
                                    empTimesheetsSummary.WedDoubleHours = null;
                                }
                                txtTimeToSave = (TextBox)(i.FindControl("txtThuTimeIn"));
                                empTimesheet.ThuTimeIn = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));
                                txtTimeToSave = (TextBox)(i.FindControl("txtThuTimeOut"));
                                empTimesheet.ThuTimeOut = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));
                                if (empTimesheet.ThuTimeIn != null && empTimesheet.ThuTimeOut != null)
                                {
                                    totalTime = CalcTotal(empTimesheet.ThuTimeIn, empTimesheet.ThuTimeOut);
                                    empTimesheetsSummary.ThuTotal = totalTime;// ConvertToTimeFormat(totalTime);

                                    empTimesheetsSummary.ThuNormalHours = totalTime >= cls.WorkingHours ? cls.WorkingHours : totalTime;
                                    empTimesheetsSummary.ThuOTHours = totalTime >= cls.WorkingHours + 2 ? 2 : (totalTime < cls.WorkingHours + 2 && totalTime > cls.WorkingHours) ? totalTime - cls.WorkingHours : 0;
                                    empTimesheetsSummary.ThuDoubleHours = totalTime >= cls.WorkingHours + 2 + 2 ? (totalTime - (cls.WorkingHours + 2)) : (totalTime < cls.WorkingHours + 2 + 2 && totalTime > cls.WorkingHours + 2) ? totalTime - cls.WorkingHours - 2 : 0;
                                    //if (empTimesheetsSummary.ThuOTHours >= cls.MealAllowanceDue)
                                    if (cls.MealAllowanceDue == 1 ? (empTimesheetsSummary.ThuOTHours > cls.MealAllowanceDue) : (empTimesheetsSummary.ThuOTHours >= cls.MealAllowanceDue))
                                        //iMealAllow += (empTimesheetsSummary.MealAllow == null) ? 0 : 1;
                                        iMealAllow += 1;
                                }
                                else
                                {
                                    empTimesheetsSummary.ThuTotal = null;
                                    empTimesheetsSummary.ThuNormalHours = null;
                                    empTimesheetsSummary.ThuOTHours = null;
                                    empTimesheetsSummary.ThuDoubleHours = null;
                                }
                                txtTimeToSave = (TextBox)(i.FindControl("txtFriTimeIn"));
                                empTimesheet.FriTimeIn = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));
                                txtTimeToSave = (TextBox)(i.FindControl("txtFriTimeOut"));
                                empTimesheet.FriTimeOut = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));
                                if (empTimesheet.FriTimeIn != null && empTimesheet.FriTimeOut != null)
                                {
                                    totalTime = CalcTotal(empTimesheet.FriTimeIn, empTimesheet.FriTimeOut);
                                    empTimesheetsSummary.FriTotal = totalTime;// ConvertToTimeFormat(totalTime);

                                    empTimesheetsSummary.FriNormalHours = totalTime >= cls.WorkingHours ? cls.WorkingHours : totalTime;
                                    empTimesheetsSummary.FriOTHours = totalTime >= cls.WorkingHours + 2 ? 2 : (totalTime < cls.WorkingHours + 2 && totalTime > cls.WorkingHours) ? totalTime - cls.WorkingHours : 0;
                                    empTimesheetsSummary.FriDoubleHours = totalTime >= cls.WorkingHours + 2 + 2 ? (totalTime - (cls.WorkingHours + 2)) : (totalTime < cls.WorkingHours + 2 + 2 && totalTime > cls.WorkingHours + 2) ? totalTime - cls.WorkingHours - 2 : 0;
                                    //if (empTimesheetsSummary.FriOTHours >= cls.MealAllowanceDue)
                                    if (cls.MealAllowanceDue == 1 ? (empTimesheetsSummary.FriOTHours > cls.MealAllowanceDue) : (empTimesheetsSummary.FriOTHours >= cls.MealAllowanceDue))
                                        //iMealAllow += (empTimesheetsSummary.MealAllow == null) ? 0 : 1;
                                        iMealAllow += 1;
                                }
                                else
                                {
                                    empTimesheetsSummary.FriTotal = null;
                                    empTimesheetsSummary.FriNormalHours = null;
                                    empTimesheetsSummary.FriOTHours = null;
                                    empTimesheetsSummary.FriDoubleHours = null;
                                }
                                txtTimeToSave = (TextBox)(i.FindControl("txtSatTimeIn"));
                                empTimesheet.SatTimeIn = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));
                                txtTimeToSave = (TextBox)(i.FindControl("txtSatTimeOut"));
                                empTimesheet.SatTimeOut = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));
                                if (empTimesheet.SatTimeIn != null && empTimesheet.SatTimeOut != null)
                                {
                                    totalTime = CalcTotal(empTimesheet.SatTimeIn, empTimesheet.SatTimeOut);
                                    empTimesheetsSummary.SatTotal = totalTime;// ConvertToTimeFormat(totalTime);                                                        
                                    empTimesheetsSummary.SatOTHours = totalTime >= 2 ? 2 : (totalTime < 2 && totalTime > 0) ? totalTime : 0;
                                    empTimesheetsSummary.SatDoubleHours = totalTime >= 2 ? totalTime - 2 : 0;

                                }
                                else
                                {
                                    empTimesheetsSummary.SatTotal = null;
                                    empTimesheetsSummary.SatNormalHours = null;
                                    empTimesheetsSummary.SatOTHours = null;
                                    empTimesheetsSummary.SatDoubleHours = null;
                                }

                                txtTimeToSave = (TextBox)(i.FindControl("txtSunTimeIn"));
                                empTimesheet.SunTimeIn = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));
                                txtTimeToSave = (TextBox)(i.FindControl("txtSunTimeOut"));
                                empTimesheet.SunTimeOut = (txtTimeToSave.Text == "" ? (Decimal?)null : Convert.ToDecimal(txtTimeToSave.Text));

                                if (empTimesheet.SunTimeIn != null && empTimesheet.SunTimeOut != null)
                                {
                                    totalTime = CalcTotal(empTimesheet.SunTimeIn, empTimesheet.SunTimeOut);
                                    empTimesheetsSummary.SunTotal = totalTime;// ConvertToTimeFormat(totalTime);
                                    empTimesheetsSummary.SunDoubleHours = totalTime;// totalTime >= cls.WorkingHours + 2 + 2 ? 2 : (totalTime < cls.WorkingHours + 2 + 2 && totalTime > cls.WorkingHours + 2) ? totalTime - cls.WorkingHours - 2 : 0;
                                }
                                else
                                {
                                    empTimesheetsSummary.SunTotal = null;
                                    empTimesheetsSummary.SunNormalHours = null;
                                    empTimesheetsSummary.SunOTHours = null;
                                    empTimesheetsSummary.SunDoubleHours = null;
                                }
                                empTimesheetsSummary.MealAllow = iMealAllow;
                            }
                            //save modified entity using new DBContext
                            context.SaveChanges();

                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    

                }

           // System.Threading.Thread.Sleep(5000);
        }
        public decimal CalcTotal(decimal? timeIn, decimal? TimeOut)
        {
            decimal totalTime = ConvertToDecimal((decimal)TimeOut) - (ConvertToDecimal((decimal)timeIn) + (TimeOut >= 12.30m ? ConvertToDecimal(LunchBreakMin): 0)   );
            return totalTime;
        }

        public decimal ConvertToDecimal(decimal timeToConvert)
        {
            return timeToConvert = Math.Floor(timeToConvert) + Math.Round(((timeToConvert - Math.Floor(timeToConvert)) / 0.60m), 2);
        }
        public decimal ConvertToTimeFormat(decimal timeToConvert)
        {
            return timeToConvert = Math.Floor(timeToConvert) + ((timeToConvert - Math.Floor(timeToConvert)) * 0.60m);
        }

        protected void lvTimesheet_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            edsTimesheetDataSource.DataBind();
            lvTimesheet.DataBind(); 
        }

        protected void ddlWeekNo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlDateYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlEmpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string empid = ddlEmpName.SelectedItem.Value;
            //edsTimesheetDataSource.WhereParameters["EmpId"].DefaultValue = empid;
        }





        protected void lvTimesheet_PreRender(object sender, EventArgs e)
        {
            lvTimesheet.DataBind();
            base.OnPreRender(e);
        }


    }
}
