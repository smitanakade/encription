using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ArlecEmpTimesheet.DAL;
using System.Globalization;

namespace ArlecEmpTimesheet.Pages
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rvGetEmployee.Visible = true;
                PopulateDropDownLists();

                ReportDataSource dataSource = new ReportDataSource("GetEmpPayrollDS", LoadData().Tables[0]);
                rvGetEmployee.LocalReport.DataSources.Clear();
                ReportParameter p1 = new ReportParameter("WeekNo", ddlWeekNo.SelectedItem.Value);
                ReportParameter p2 = new ReportParameter("Year", ddlDateYear.SelectedItem.Value);
                rvGetEmployee.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });
                rvGetEmployee.LocalReport.DataSources.Add(dataSource);
                rvGetEmployee.LocalReport.Refresh();
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
            return result.AddDays(-3);
        }
        private void UpdateReport()
        {
            ReportDataSource dataSource = new ReportDataSource("GetEmpPayrollDS", LoadData().Tables[0]);
            rvGetEmployee.LocalReport.DataSources.Clear();
            ReportParameter p1 = new ReportParameter("WeekNo", ddlWeekNo.SelectedItem.Value);
            ReportParameter p2 = new ReportParameter("Year", ddlDateYear.SelectedItem.Value);
            ReportParameter p3 = new ReportParameter("Name", txtEmpName.Text);
            rvGetEmployee.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });
            rvGetEmployee.LocalReport.DataSources.Add(dataSource);
            rvGetEmployee.LocalReport.Refresh();
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
            }
        }
        public static int GetWeekNumber(DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
        private DataSet LoadData()
        {
            var dataset = new DataSet();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Arlec_TimesheetConnectionString"].ConnectionString))
            {
                var command = new SqlCommand("[RPT_GetEmpPayroll_SP]")
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("WeekNo", ddlWeekNo.SelectedItem.Value);
                command.Parameters.AddWithValue("Year", ddlDateYear.SelectedItem.Value);
                command.Parameters.AddWithValue("Name", txtEmpName.Text);
                var adap = new SqlDataAdapter(command);
                con.Open();
                adap.Fill(dataset, "GetEmpPayrollDS");
            }

            return dataset;
        }

        protected void ddlWeekNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateReport();
        }

        protected void ddlDateYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateReport();
        }

        protected void txtEmpName_TextChanged(object sender, EventArgs e)
        {
            UpdateReport();
        }
    }
}
