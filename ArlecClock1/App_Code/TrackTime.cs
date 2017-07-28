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
using System.Text;
using System.Security.Cryptography;
using System.IO;
namespace ArlecClock.App_Code
{
    public class TrackTime
    {
        public decimal LunchBreakMin = 0.30m;

        public String LoginAfterScan(object SesstionID,string LoginIdText,String LoginType)
        {
           

            System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Arlec_TimesheetConnectionString"].ToString());
            String Query = "";
            if (SesstionID != null)
            {
                Query = "SELECT Id FROM [WMSDB].[dbo].[Employees] WHERE LoginId='" + LoginIdText + "'";
            }
            else
            {
                Query = "SELECT Id FROM [WMSDB].[dbo].[Employees] WHERE LoginId='" + LoginIdText + "' AND RemoteLogin=1";
            }
            /*Get employee id from Employee table through Loginid */
            //String Query = "SELECT Id FROM [WMSDB].[dbo].[Employees] WHERE LoginId='" + LoginId.Text + "' AND RemoteLogin=1";
            SqlCommand sqlCmd = new SqlCommand(Query, sqlConnection1);

            sqlConnection1.Open();
            //Session["Message"] = "No Record Found";

            var EmpId = sqlCmd.ExecuteScalar();
            if (EmpId != null)
            {
                String sqlQuery = "";
                /*Insert scaned log into EmpAttendanceLog*/
                if (LoginType == "IN")
                {
                    
                    String InTimeCheckQuery = "SELECT INTime From EmpAttendanceLog WHERE EmployeeId="+EmpId+"and CONVERT(date, DateScaned) = CONVERT(date,GETDATE())";
                   
                   // sqlCmd.Parameters.AddWithValue("@EmployeeId", EmpId);
                    sqlCmd = new SqlCommand(InTimeCheckQuery, sqlConnection1);
                    var exsitINTime= sqlCmd.ExecuteScalar();

                    if(exsitINTime == null){

                        sqlQuery = "INSERT INTO EmpAttendanceLog(EmployeeId,INTime,DateScaned) values(@EmployeeId,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP)";
                        using (sqlCmd = new SqlCommand(sqlQuery, sqlConnection1))
                        {
                            sqlCmd.Parameters.AddWithValue("@EmployeeId", EmpId);
                            sqlCmd.ExecuteNonQuery();
                        }
                    }
                    //else{
                    //    return "Y";
                    //   }
                }
                if (LoginType == "OUT")
                {
                    string OutTimeCheckQuery = "SELECT OutTime From EmpAttendanceLog WHERE EmployeeId=" + EmpId + "and CONVERT(date, DateScaned) = CONVERT(date,GETDATE())";
                    sqlCmd= new SqlCommand(OutTimeCheckQuery,sqlConnection1);
                    var exsitOutTime= sqlCmd.ExecuteScalar();

                    if (exsitOutTime != null)
                    {
                        sqlQuery = "Update [WMSDB].[dbo].[EmpAttendanceLog] set OutTime=CURRENT_TIMESTAMP Where EmployeeId=@EmployeeId and CONVERT(date, DateScaned) = CONVERT(date,GETDATE())";
                        using (sqlCmd = new SqlCommand(sqlQuery, sqlConnection1))
                        {
                            sqlCmd.Parameters.AddWithValue("@EmployeeId", EmpId);
                            sqlCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                       sqlQuery = "INSERT INTO EmpAttendanceLog(EmployeeId,INTime,DateScaned) values(@EmployeeId,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP)";
                        using (sqlCmd = new SqlCommand(sqlQuery, sqlConnection1))
                        {
                            sqlCmd.Parameters.AddWithValue("@EmployeeId", EmpId);
                            sqlCmd.ExecuteNonQuery();
                        }
                    }
                }
               
                 
                

                var dt = new DataTable();
                
                String selectQuery = "SELECT INTime, OutTime,DateScaned FROM [WMSDB].[dbo].[EmpAttendanceLog] WHERE CONVERT(date, DateScaned) = CONVERT(date,GETDATE()) AND EmployeeId=@EmployeeId";
                sqlCmd = new SqlCommand(selectQuery, sqlConnection1);
                sqlCmd.Parameters.AddWithValue("@EmployeeId", EmpId);

                var ad = new SqlDataAdapter(sqlCmd);
                ad.Fill(dt);

            
                /*Converting DateTime in to */
                DateTime FinishDateTime;
                DateTime ScanedOUT;
                DateTime ScanedIN;

                DateTime.TryParse(String.Format("{0}", dt.Rows[0]["INTime"]), out ScanedIN);
                DateTime.TryParse(String.Format("{0}", dt.Rows[0]["OutTime"]), out ScanedOUT);
                DateTime.TryParse(String.Format("{0}", dt.Rows[0]["DateScaned"]), out FinishDateTime);

                var ClockIN = ScanedIN.ToString("HH.mm");
                var ClockOut = ScanedOUT.ToString("HH.mm");
                var weekDay = FinishDateTime.ToString("ddd");
                var DateYear = FinishDateTime.ToString("yyyy");


                int DateOfYEAR = FinishDateTime.Year;
                int Emp = Convert.ToInt32(EmpId);

                String meridiemTime = FinishDateTime.ToString();
                String meridiem = meridiemTime.Substring(meridiemTime.Length - 2, 2);


                CultureInfo ciCurr = CultureInfo.CurrentCulture;
                int weekNum = ciCurr.Calendar.GetWeekOfYear(FinishDateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                if (weekNum == 53)
                {
                    
                    DateOfYEAR += 1;
                    weekNum = 1;
                }
               
                var CheckDt = new DataTable();
                String CheckQuery = "SELECT [EmpId] FROM [WMSDB].[dbo].[EmpTimesheets] WHERE WeekNo=@weekNumber AND EmpId=@EmployeeId AND DateYear=@Year";
                sqlCmd = new SqlCommand(CheckQuery, sqlConnection1);
                sqlCmd.Parameters.AddWithValue("@weekNumber", weekNum);
                sqlCmd.Parameters.AddWithValue("@EmployeeId", EmpId);
                sqlCmd.Parameters.AddWithValue("@Year", DateYear);


                var CheckAd = new SqlDataAdapter(sqlCmd);
                CheckAd.Fill(CheckDt);
                int RowCount = CheckDt.Rows.Count;

                    if (LoginType == "IN")
                    {
                        String updateQueryInTime = "";
                        /* if weekday is Monday than Insert query will create record in to EmpTimeSheets Table */
                        if (RowCount == 0)
                        {
                            updateQueryInTime = "INSERT INTO [WMSDB].[dbo].[EmpTimesheets]([EmpId],[DateYear],[WeekNo],[" + weekDay + "TimeIn]) values (" + EmpId + "," + DateYear + "," + weekNum + "," + ClockIN + ") ";
                        }
                        if(RowCount !=0)
                        {
                            updateQueryInTime = "UPDATE [WMSDB].[dbo].[EmpTimesheets] set " + weekDay + "TimeIn=" + ClockIN + " WHERE EmpId=" + EmpId + " AND WeekNo=" + weekNum + " AND DateYear =" + DateYear + "";

                        }
                              
                        sqlCmd = new SqlCommand(updateQueryInTime, sqlConnection1);
                        sqlCmd.ExecuteNonQuery();
                        /*Calling  EmployeeTimeSheet funtion for making timesheet record*/

                        EmployeeTimeSheet(Emp, weekNum, DateOfYEAR);

                        //EmployeeTimeSheet(Emp, weekNum, DateOfYEAR);
                        sqlConnection1.Close();

                    }
                    if (LoginType == "OUT")
                    {
                        String updateQuery = "";
                        if (RowCount == 0)
                        {
                             updateQuery = "INSERT INTO [WMSDB].[dbo].[EmpTimesheets]([EmpId],[DateYear],[WeekNo],[" + weekDay + "TimeOut]) values (" + EmpId + "," + DateYear + "," + weekNum + "," + ClockIN + ") ";
                        }
                        if (RowCount != 0)
                        {
                             updateQuery = "UPDATE [WMSDB].[dbo].[EmpTimesheets] set " + weekDay + "TimeOut=" + ClockOut + " WHERE EmpId=" + EmpId + " AND WeekNo=" + weekNum + "";
                        }
                        sqlCmd = new SqlCommand(updateQuery, sqlConnection1);
                        sqlCmd.ExecuteNonQuery();
                        /*Calling  EmployeeTimeSheet funtion for making timesheet record*/

                        EmployeeTimeSheet(Emp, weekNum, DateOfYEAR);

                        //EmployeeTimeSheet(Emp, weekNum, DateOfYEAR);
                        sqlConnection1.Close();

                    }            

                //Session["LoginId"] = LoginId.Text;
                //LoginId.Text = "";

                string LoginId = LoginIdText;
                return LoginId;
            }
            else
            {
                string LoginId = "";
                return LoginId;
                //Session["LoginId"] = null;
                //LoginId.Text = "";
                /* showing Message if record is not found*/
                //lvLocationDetails.InsertItemPosition = InsertItemPosition.LastItem;
                //lvLocationDetails.DataBind();

            }


        }


        public void EmployeeTimeSheet(int EmployeeId, int WeekNum, int DateOfYear)
        {
            
           
            using (var context = new WMSDBEntities())
            {
                //TextBox txtTimeToSave;
                int iMealAllow = 0;
                //ArlecClock.DAL.EmpTimesheet empTimesheet;
                //ArlecClock.DAL.EmpTimesheetsSummary empTimesheetsSummary;

                var empTimesheet = context.EmpTimesheets.Where(s => s.EmpId == EmployeeId && s.WeekNo == WeekNum && s.DateYear == DateOfYear).FirstOrDefault<EmpTimesheet>();
                if (empTimesheet != null)
                {
                    var empTimesheetsSummary = context.EmpTimesheetsSummaries.Where(s => s.TimesheetID == empTimesheet.Id).FirstOrDefault<EmpTimesheetsSummary>();


                    if (empTimesheetsSummary == null)
                    {
                        empTimesheetsSummary = new EmpTimesheetsSummary();
                        empTimesheetsSummary.TimesheetID = empTimesheet.Id;
                        empTimesheetsSummary.EmpId = (int)empTimesheet.EmpId;
                        empTimesheetsSummary.WeekNo = WeekNum;// weekno
                        empTimesheetsSummary.DateYear = DateOfYear; // date of year
                        context.AddToEmpTimesheetsSummaries(empTimesheetsSummary);
                    }
                    if (empTimesheet != null)
                    {
                        ArlecClock.DAL.Employee emp = context.Employees.Where(s => s.Id == (int)empTimesheet.EmpId).FirstOrDefault<ArlecClock.DAL.Employee>();
                        ArlecClock.DAL.Classification cls = context.Classifications.Where(s => s.Id == emp.ClassificationId).FirstOrDefault<ArlecClock.DAL.Classification>();
                        decimal totalTime;
                        if (empTimesheet.MonTimeIn != null && empTimesheet.MonTimeOut != null)
                        {
                            totalTime = CalcTotal(empTimesheet.MonTimeIn, empTimesheet.MonTimeOut);
                            empTimesheetsSummary.MonTotal = totalTime;// ConvertToTimeFormat(totalTime);

                            empTimesheetsSummary.WeekNo = empTimesheet.WeekNo;
                            empTimesheetsSummary.MonNormalHours = totalTime >= cls.WorkingHours ? cls.WorkingHours : totalTime;
                            empTimesheetsSummary.MonOTHours = totalTime >= cls.WorkingHours + 2 ? 2 : (totalTime < cls.WorkingHours + 2 && totalTime > cls.WorkingHours) ? totalTime - cls.WorkingHours : 0;
                            empTimesheetsSummary.MonDoubleHours = totalTime >= cls.WorkingHours + 2 + 2 ? (totalTime - (cls.WorkingHours + 2)) : (totalTime < cls.WorkingHours + 2 + 2 && totalTime > cls.WorkingHours + 2) ? totalTime - cls.WorkingHours - 2 : 0;
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
                        if (empTimesheet.ThuTimeIn != null && empTimesheet.ThuTimeOut != null)
                        {
                            totalTime = CalcTotal(empTimesheet.ThuTimeIn, empTimesheet.ThuTimeOut);
                            empTimesheetsSummary.ThuTotal = totalTime;// ConvertToTimeFormat(totalTime);

                            empTimesheetsSummary.WeekNo = empTimesheet.WeekNo;
                            empTimesheetsSummary.ThuNormalHours = totalTime >= cls.WorkingHours ? cls.WorkingHours : totalTime;
                            empTimesheetsSummary.ThuOTHours = totalTime >= cls.WorkingHours + 2 ? 2 : (totalTime < cls.WorkingHours + 2 && totalTime > cls.WorkingHours) ? totalTime - cls.WorkingHours : 0;
                            empTimesheetsSummary.ThuDoubleHours = totalTime >= cls.WorkingHours + 2 + 2 ? (totalTime - (cls.WorkingHours + 2)) : (totalTime < cls.WorkingHours + 2 + 2 && totalTime > cls.WorkingHours + 2) ? totalTime - cls.WorkingHours - 2 : 0;
                            if (cls.MealAllowanceDue == 1 ? (empTimesheetsSummary.ThuOTHours > cls.MealAllowanceDue) : (empTimesheetsSummary.ThuOTHours >= cls.MealAllowanceDue))
                                iMealAllow += 1;
                        }
                        else
                        {
                            empTimesheetsSummary.ThuTotal = null;
                            empTimesheetsSummary.ThuNormalHours = null;
                            empTimesheetsSummary.ThuOTHours = null;
                            empTimesheetsSummary.ThuDoubleHours = null;
                        }
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

                    }
                    context.SaveChanges();
                }
            }
        }

        public decimal CalcTotal(decimal? timeIn, decimal? TimeOut)
        {
            decimal totalTime = ConvertToDecimal((decimal)TimeOut) - (ConvertToDecimal((decimal)timeIn) + (TimeOut >= 12.30m ? ConvertToDecimal(LunchBreakMin) : 0));
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


        private const string initVector = "tu89geji340t89u2";

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        public  string Encrypt(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public  string Decrypt(string charText, string passPhrase)
        {
            String cipherText = charText.Replace(" ", "+");
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}