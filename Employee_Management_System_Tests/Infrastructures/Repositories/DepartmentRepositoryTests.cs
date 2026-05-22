using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Exceptions;
using Employee_Management_System.Infrastructures.Adapters;
using Employee_Management_System.Infrastructures.Context;
using Employee_Management_System.Infrastructures.Entities;
using Employee_Management_System.Infrastructures.Repositories;
using WebApp_Exercise.Tests.TestDoubles;

namespace Employee_Management_System_Tests.Infrastructures.Repositories;

[TestClass]
public sealed class DepartmentRepositoryTests
{
    [TestMethod]
    public void FindAll_ReturnsAllCategories()
    {
        using var context = CreateContext(
        [
            new DepartmentEntity { DeptNo = 101, DeptName = "総務部" },
            new DepartmentEntity { DeptNo = 102, DeptName = "情報システム部" },
        ]);
        var repository = CreateRepository(context);

        var departments = repository.FindAll();

        Assert.AreEqual(2, departments.Count);
        AssertCategory(departments[0], 101, "総務部");
        AssertCategory(departments[1], 102, "情報システム部");
    }

    [TestMethod]
    public void FindById_WhenCategoryExists_ReturnsCategory()
    {
        using var context = CreateContext(
        [
            new DepartmentEntity { DeptNo = 101, DeptName = "総務部" },
            new DepartmentEntity { DeptNo = 102, DeptName = "情報システム部" },
        ]);
        var repository = CreateRepository(context);

        var department = repository.FindByNumber(102);

        Assert.IsNotNull(department);
        AssertCategory(department, 102, "情報システム部");
    }

    [TestMethod]
    public void FindById_WhenCategoryDoesNotExist_ReturnsNull()
    {
        using var context = CreateContext(
        [
            new DepartmentEntity { DeptNo = 101, DeptName = "総務部" },
        ]);
        var repository = CreateRepository(context);

        var department = repository.FindByNumber(102);

        Assert.IsNull(department);
    }

    [TestMethod]
    public void FindAll_WhenDbSetThrows_WrapsExceptionInInternalException()
    {
        using var context = CreateContext(new ThrowingDbSet<DepartmentEntity>());
        var repository = CreateRepository(context);

        var exception = Assert.ThrowsException<InternalException>(() => repository.FindAll());

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    private static DepartmentRepository CreateRepository(AppDbContext context)
    {
        return new DepartmentRepository(context, new DepartmentEntityAdapter());
    }

    private static AppDbContext CreateContext(IEnumerable<DepartmentEntity> entities)
    {
        return CreateContext(new QueryableDbSet<DepartmentEntity>(entities));
    }

    private static AppDbContext CreateContext(DbSet<DepartmentEntity> departments)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        return new AppDbContext(options)
        {
            Departments = departments,
        };
    }

    private static void AssertCategory(Department department, int id, string name)
    {
        Assert.AreEqual(id, department.DeptNo);
        Assert.AreEqual(name, department.DeptName);
    }
}
