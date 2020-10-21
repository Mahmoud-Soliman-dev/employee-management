using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _db;

        public EmployeeRepository(AppDbContext db)
        {
            _db = db;
        }
        public Employee Add(Employee employee)
        {
            _db.Employees.Add(employee);
            _db.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _db.Employees.Find(id);
            if (employee != null)
            {
                _db.Employees.Remove(employee);
                _db.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _db.Employees;
        }

        public Employee GetEmployee(int id)
        {
            return _db.Employees.FirstOrDefault(e => e.Id == id);
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = _db.Employees.Attach(employeeChanges);
            employee.State = EntityState.Modified;
            _db.SaveChanges();
            return employeeChanges;
        }
    }
}
