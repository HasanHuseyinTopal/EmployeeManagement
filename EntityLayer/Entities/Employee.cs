using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class Employee 
    {
        [Key]
        public int employeeId { get; set; }
        [Required]
        public string employeeName { get; set; }
        [Required]
        public string employeeEmail { get; set; }
        [Required]
        public Dept? employeeDepartmant { get; set; }
        [Required]
        [ValidateNever]
        public string employeeImageUrl { get; set; }

    }
}
