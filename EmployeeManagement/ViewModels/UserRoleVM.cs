using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class UserRoleVM
    {
        public UserRoleVM()
        {
            Users = new List<RoleUser>();
        }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<RoleUser> Users { get; set; }
    }

    public class RoleUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
