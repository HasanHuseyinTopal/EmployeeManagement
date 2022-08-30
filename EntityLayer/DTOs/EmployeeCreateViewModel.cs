using EntityLayer.Entities;
using EntityLayer.Utulities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EntityLayer.Entities.Employee;

namespace EntityLayer.DTOs
{
    public class EmployeeCreateViewModel
    {
        [Required(ErrorMessage = "Name is required value"), MaxLength(50, ErrorMessage = "Name be max 50 char"),MinLength(3,ErrorMessage = "Name be min 3 char"), Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must have only characters")]
        public string employeeName { get; set; }


        [Required(ErrorMessage = "Email is required value")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",ErrorMessage ="Invalid email adress")]
        [Display(Name ="Email")]
        public string? employeeEmail { get; set; }


        [Display(Name = "Confirm-Email")]
        [Compare("employeeEmail", ErrorMessage ="Emails Are Not Matched")]
        public string? employeeEmailConfirmation { get; set; }


        [Required(ErrorMessage = "Department is required value")]
        [Display(Name ="Department")]
        public Dept? employeeDepartmant { get; set; }
        [ValidateNever]
        public string employeeImageUrl { get; set; }
    }
}
