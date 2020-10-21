using EmployeeManagement.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        [Remote(controller: "Account", action: "IsDuplicatedEmail")]
        [ValidEmailDomain(validDomain: "gmail.com", ErrorMessage = "Email must be @gmail.com")]
        public string Email { get; set; }

        //[Display(Name = "User Name")]
        //public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and comnfirm password do not match")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}
