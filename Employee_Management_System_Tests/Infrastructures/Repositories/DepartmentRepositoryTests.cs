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

    [TestMethod]
    public void Create_WithItemAndStock_AddsEntitiesAndSavesTwice()
    {
        using var context = CreateContext(Array.Empty<DepartmentEntity>());
        var repository = CreateRepository(context);
        var item = new Department(101,"総務部");
        var item2 = new Department(102,"情報システム部");
        repository.Add(item);
        repository.Add(item2);

        var savedDepartments = ((QueryableDbSet<DepartmentEntity>)context.Departments).Entities;
        Assert.AreEqual(2, savedDepartments.Count);

        var savedDepartment = savedDepartments[0];
        Assert.AreEqual(101, savedDepartment.DeptNo);
        Assert.AreEqual("総務部", savedDepartment.DeptName);

        var savedDepartment2 = savedDepartments[1];
        Assert.AreEqual(102, savedDepartment2.DeptNo);
        Assert.AreEqual("情報システム部", savedDepartment2.DeptName);

        Assert.AreEqual(2, ((TestAppDbContext)context).SaveChangesCallCount);
    }

    [TestMethod]
    public void Create_WhenDbSetThrows_WrapsExceptionInInternalException()
    {
        using var context = CreateContext(new ThrowingDbSet<DepartmentEntity>());
        var repository = CreateRepository(context);

        var exception = Assert.ThrowsException<InternalException>(
            () => repository.Add(new Department(101, "総務部")));

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

    private static TestAppDbContext CreateContext(QueryableDbSet<DepartmentEntity> departments)
    {
        return new TestAppDbContext
        {
            Departments = departments,
        };
    }

    private static TestAppDbContext CreateContext(ThrowingDbSet<DepartmentEntity> departments)
    {
        return new TestAppDbContext
        {
            Departments = departments,
        };
    }
}
