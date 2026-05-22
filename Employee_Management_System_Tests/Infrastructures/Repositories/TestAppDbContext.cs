using Microsoft.EntityFrameworkCore;
using Employee_Management_System.Infrastructures.Context;
using Employee_Management_System.Infrastructures.Entities;
using Employee_Management_System.Applications.Domains;

namespace WebApp_Exercise.Tests.TestDoubles;

internal sealed class TestAppDbContext : AppDbContext
{
    public TestAppDbContext()
        : base(new DbContextOptionsBuilder<AppDbContext>().Options)
    {
        Departments = new QueryableDbSet<DepartmentEntity>([]);
    }

    public int SaveChangesCallCount { get; private set; }

    public override int SaveChanges()
    {
        SaveChangesCallCount++;
        return 1;
    }
}
