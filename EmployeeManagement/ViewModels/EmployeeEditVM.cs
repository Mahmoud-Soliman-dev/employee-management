using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeEditVM : EmployeeCreateVM
    {
        public int Id { get; set; }
        public string ExistingPhotoPath { get; set; }

        public static EmployeeEditVM CreateViewModel(Employee employee)
        {
            EmployeeEditVM employeeEditVM = new EmployeeEditVM
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };

            return employeeEditVM;
        }
    }
}
