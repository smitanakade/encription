using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ArlecClock.Models;
using System.Web.Routing;
using System.Data;


namespace ArlecClock.BL
{
    public class EmployeeBLL
    {
        public IQueryable<Employee> GetEmployees(
        int? empId,
        string empName)
        {
            var _db = new ArlecClock.Models.EmployeeContext();

            IQueryable<Employee> query = _db.Employees;



            if (empId.HasValue && empId > 0)
            {
                query = query.Where(p => p.Id == empId);
            }

            if (!String.IsNullOrEmpty(empName))
            {
                query = query.Where(p =>
                String.Compare(p.FirstName,
                empName) == 0);
            }
            return query;
        }
        public void Update(Employee newValues)
        {
            Employee emp;
            // Get student from DB
            using (var ctx = new ArlecClock.Models.EmployeeContext())
            {
                emp = ctx.Employees.Where(s => s.Id == newValues.Id).FirstOrDefault<Employee>();
            }

            // change student name in disconnected mode (out of DBContext scope)
            if (emp != null)
            {
                emp.ClassificationId = 1;
                emp.FirstName = newValues.FirstName;
                emp.LastName = newValues.LastName;
                emp.BirthDate = newValues.BirthDate;
                emp.HireDate = newValues.HireDate;
                emp.Sex = newValues.Sex;
                emp.Job = newValues.Job;
                emp.IsActive = newValues.IsActive;
            }

            //save modified entity using new DBContext
            using (var dbCtx = new ArlecClock.Models.EmployeeContext())
            {
                //Mark entity as modified
                dbCtx.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                dbCtx.SaveChanges();
            }
        }
        public void Insert(Employee newEmp)
        {
            using (var _db = new ArlecClock.Models.EmployeeContext())
            {
                newEmp.ClassificationId = 1;
                _db.Employees.Add(newEmp);
                _db.SaveChanges();
            }
        }
        public void Delete(Employee newEmp)
        {
            using (var _db = new ArlecClock.Models.EmployeeContext())
            {
                _db.Employees.Remove(newEmp);
                _db.SaveChanges();
            }
        }
    }
}
