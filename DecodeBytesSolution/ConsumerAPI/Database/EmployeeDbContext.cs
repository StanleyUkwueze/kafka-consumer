﻿using ConsumerAPI;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApplication.Database
{
    public class EmployeeDbContext: DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options): base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeReport> Reports { get; set; }

    }
}
