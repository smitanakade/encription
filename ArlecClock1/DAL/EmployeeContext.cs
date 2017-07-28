using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ArlecClock.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext()
            : base("Arlec_TimesheetConnectionString")
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Classification> Classifications { get; set; }

    }
}
