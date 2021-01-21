using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeRegistration.Models
{
    public class Employee
    {
        [Key]
        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Employee Id")]
        public int emp_id { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Employee Name")]
        public string emp_name { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = " Date of Birth")]
        public string date_of_birth { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Date of Join")]
        public string date_of_join { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Department")]
        public int dep_id { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string modified_by { get; set; }
        public string modified_date { get; set; }
        [NotMapped]
        public string Department { get; set; }

    }
}
