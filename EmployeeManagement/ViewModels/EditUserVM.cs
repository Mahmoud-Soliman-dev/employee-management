using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EditUserVM
    {
        public EditUserVM()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        public string City { get; set; }

        public List<string> Roles { get; set; }
        public List<string> Claims { get; set; }
    }
}
