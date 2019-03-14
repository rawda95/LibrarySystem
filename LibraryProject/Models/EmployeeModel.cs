using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryProject.Models
{
    [Table("Employees")]
    public class EmployeeModel
    {
        [Key]

        [ForeignKey("User")]
        public string EmployeeId { get; set; }
        public double Salary { get; set; }
        public DateTime HireDate { get; set; }


        [ForeignKey("status")]
        public int status_id { get; set; }

        public virtual List<flag> status { get; set; }

        public ApplicationUser User { get; set; }

    }
}
