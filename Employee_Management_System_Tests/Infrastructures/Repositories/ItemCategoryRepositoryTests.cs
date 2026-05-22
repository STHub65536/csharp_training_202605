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
public sealed class ItemCategoryRepositoryTests
{
    [TestMethod]
    public void FindAll_ReturnsAllCategories()
    {
        using var context = CreateContext(
        [
            new DepartmentEntity { DeptNo = 1, DeptName = "Books" },
            new DepartmentEntity { DeptNo = 2, DeptName = "Games" },
        ]);
        var repository = CreateRepository(context);

        var categories = repository.FindAll();

        Assert.AreEqual(2, categories.Count);
        AssertCategory(categories[0], 1, "Books");
        AssertCategory(categories[1], 2, "Games");
    }

    [TestMethod]
    public void FindById_WhenCategoryExists_ReturnsCategory()
    {
        using var context = CreateContext(
        [
            new DepartmentEntity { DeptNo = 1, DeptName = "Books" },
            new DepartmentEntity { DeptNo = 2, DeptName = "Games" },
        ]);
        var repository = CreateRepository(context);

        var category = repository.FindByNumber(2);

        Assert.IsNotNull(category);
        AssertCategory(category, 2, "Games");
    }

    [TestMethod]
    public void FindById_WhenCategoryDoesNotExist_ReturnsNull()
    {
        using var context = CreateContext(
        [
            new DepartmentEntity { DeptNo = 1, DeptName = "Books" },
        ]);
        var repository = CreateRepository(context);

        var category = repository.FindByNumber(2);

        Assert.IsNull(category);
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

    private static AppDbContext CreateContext(DbSet<DepartmentEntity> itemCategories)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        return new AppDbContext(options)
        {
            Departments = itemCategories,
        };
    }

    private static void AssertCategory(Department category, int id, string name)
    {
        Assert.AreEqual(id, category.DeptNo);
        Assert.AreEqual(name, category.DeptName);
    }
}
