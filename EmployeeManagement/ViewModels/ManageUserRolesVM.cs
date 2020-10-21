using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class ManageUserRolesVM
    {
        public ManageUserRolesVM()
        {
            Roles = new List<UserRole>();
        }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<UserRole> Roles { get; set; }
    }

    public class UserRole
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
