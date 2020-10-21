using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class InitialData
    {
        public static Employee[] GetInitialEmployees()
        {
            Employee[] employees = new Employee[] {
                new Employee
                {
                    Id = 1,
                    Name = "Mahmoud",
                    Email = "mahmoud@gmail.com",
                    Department = Dept.IT
                },
                new Employee
                {
                    Id = 2,
                    Name = "Walid",
                    Email = "walid@gmail.com",
                    Department = Dept.HR
                },
                new Employee
                {
                    Id = 3,
                    Name = "Ali",
                    Email = "ali@gmail.com",
                    Department = Dept.Payroll
                }
            };

            return employees;
        }
    }
}
