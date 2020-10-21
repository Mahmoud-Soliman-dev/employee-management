using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeDetailsVM
    {
        public string PageTitle { get; set; }
        public Employee Employee { get; set; }
        public EmployeeDetailsVM(string pageTitle, Employee employee)
        {
            PageTitle = pageTitle;
            Employee = employee;
        }
    }
}
