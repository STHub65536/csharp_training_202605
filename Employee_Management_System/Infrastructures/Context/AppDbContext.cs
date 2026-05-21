using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Infrastructures.Context.EntityTypeConfiguration;
using Employee_Management_System.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Infrastructures.Context;
public class AppDbContext : DbContext
{
    private DbSet<DepartmentEntity> Departments { get; set; }
    private DbSet<EmployeeEntity> Employees { get; set; }
    private DbSet<AdminEntity> Admins { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DepartmentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AdminEntityTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}