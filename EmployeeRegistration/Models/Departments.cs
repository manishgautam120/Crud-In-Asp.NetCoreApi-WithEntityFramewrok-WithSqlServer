using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRegistration.Models
{
    public class Departments
    {
        [Key]
        public int dept_id { get; set; }
        public string dept_name { get; set; }

        //public string created_by { get; set; }
        //public string created_date { get; set; }
        //public string modified_by { get; set; }
        //public string modified_date { get; set; }
    }
}
