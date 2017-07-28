using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace ArlecEmpTimesheet.Models
{
    public class Employee
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(100), Display(Name = "FirstName")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime HireDate { get; set; }

        public string Job { get; set; }

        public string Sex { get; set; }

        public DateTime BirthDate { get; set; }
        public int ClassificationId { get; set; }

        public Classification Classification { get; set; }
        public int? IsActive { get; set; }

    }
}
