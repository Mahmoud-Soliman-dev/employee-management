using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeCreateVM
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name can not exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
        [Display(Name = "Personal Email")]
        public string Email { get; set; }

        [Required]
        public Dept? Department { get; set; }

        public IFormFile Photo { get; set; }
    }
}
