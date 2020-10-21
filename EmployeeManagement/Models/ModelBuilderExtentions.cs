using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtentions
    {
        public static void SeedInitialData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(InitialData.GetInitialEmployees());
        }
    }
}
