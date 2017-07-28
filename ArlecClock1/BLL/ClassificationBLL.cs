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
    public class ClassificationBLL
    {
        public IQueryable<Classification> GetClassifications(
        int? classId,
        string className)
        {
            var _db = new ArlecClock.Models.EmployeeContext();
            IQueryable<Classification> query = _db.Classifications;

            if (classId.HasValue && classId > 0)
            {
                query = query.Where(p => p.Id == classId);
            }

            if (!String.IsNullOrEmpty(className))
            {
                query = query.Where(p =>
                String.Compare(p.Name,
                className) == 0);
            }
            return query;
        }
        public void Update(Classification newValues)
        {
            Classification cls;
            // Get student from DB
            using (var ctx = new ArlecClock.Models.EmployeeContext())
            {
                cls = ctx.Classifications.Where(s => s.Id == newValues.Id).FirstOrDefault<Classification>();
            }

            // change student name in disconnected mode (out of DBContext scope)
            if (cls != null)
            {
                //cls.FirstName = newValues.FirstName;
                //cls.LastName = newValues.LastName;
                //cls.BirthDate = newValues.BirthDate;
                //cls.HireDate = newValues.HireDate;
                //cls.Sex = newValues.Sex;
                //cls.Job = newValues.Job;
            }

            //save modified entity using new DBContext
            using (var dbCtx = new ArlecClock.Models.EmployeeContext())
            {
                //Mark entity as modified
                dbCtx.Entry(cls).State = System.Data.Entity.EntityState.Modified;
                dbCtx.SaveChanges();
            }
        }
    }
}
