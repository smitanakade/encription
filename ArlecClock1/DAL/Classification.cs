using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace ArlecClock.Models
{
    public class Classification
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(100), Display(Name = "Name")]
        public string Name { get; set; }
        public string Award { get; set; }
        public decimal MealAllowanceAmount { get; set; }
        public decimal MealAllowanceDue { get; set; }
        public decimal MonToFriFirst2HoursRate { get; set; }
        public decimal MonToFriGraterThan2HoursRate { get; set; }
        public decimal SatAllHoursRate { get; set; }
        public string SatTeaMoney { get; set; }
        public decimal SunAllHours { get; set; }
        public string SunTeaMoney { get; set; }
        public decimal GoodFridayorXmasRate { get; set; }
        public decimal AllOtherPubHolidays { get; set; }
        public decimal WorkingHours { get; set; }
        public double? PayRate { get; set; }


    }
}
