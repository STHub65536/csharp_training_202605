
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Infrastructures.Adapters;
using Employee_Management_System.Infrastructures.Context;
using Employee_Management_System.Infrastructures.Repositories;
using Employee_Management_System.Exceptions;

namespace Employee_Management_System_Tests.Infrastructures.Repositories;

[DoNotParallelize]
[TestClass]
public class DepartmentRepositoryTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=employee_management;Username=postgres;Password=training;";

    private DepartmentRepository _repository = null!;
    private AppDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var adapter = new DepartmentEntityAdapter();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "init.sql");
        var sql = File.ReadAllText(path);
        _context.Database.ExecuteSqlRaw(sql);

        _repository = new DepartmentRepository(_context, adapter);
    }

    [TestMethod]
    public void FindAll_ReturnsAllDepartments()
    {
        var lists = _repository.FindAll();

        AreEqual(101, lists[0].DeptNo);
        AreEqual("総務部", lists[0].DeptName);
        AreEqual(102, lists[1].DeptNo);
        AreEqual("経理部", lists[1].DeptName);
        AreEqual(103, lists[2].DeptNo);
        AreEqual("人事部", lists[2].DeptName);
        AreEqual(104, lists[3].DeptNo);
        AreEqual("開発部", lists[3].DeptName);
        AreEqual(105, lists[4].DeptNo);
        AreEqual("営業部", lists[4].DeptName);
    }

    [TestMethod]
    public void FindByNumber_WhenNumberCorrect()
    {
        var actual = _repository.FindByNumber(101);

        IsNotNull(actual);
        AreEqual(101, actual.DeptNo);
        AreEqual("総務部", actual.DeptName);
    }

    [TestMethod]
    public void FindByNumber_WhenNumberNotFound()
    {
        var actual = _repository.FindByNumber(999);
        IsNull(actual);
    }

    [TestMethod]
    public void HasSameDeptName_WhenNameExists()
    {
        var actual = _repository.HasSameDeptName("総務部");
        IsTrue(actual);
    }

    [TestMethod]
    public void HasSameDeptName_WhenNameNotExists()
    {
        var actual = _repository.HasSameDeptName("情報システム部");
        IsFalse(actual);
    }

    [TestMethod]
    public void Add_WhenCorrect()
    {
        var beforeCount = _context.Departments.Count();

        var department = new Department(110, "検証部");

        _repository.Add(department);

        var afterCount = _context.Departments.Count();
        AreEqual(beforeCount + 1, afterCount);

        var created = _context.Departments
            .FirstOrDefault(i => i.DeptNo == 110);

        IsNotNull(created);
        AreEqual("検証部", created.DeptName);
    }

    [TestMethod]
    public void Add_WhenNumberIsIncorrect()
    {
        var department = new Department(1000, "検証部"); // 部署番号:4桁(最大3桁)

        var exception = Assert.ThrowsException<InternalException>(() => _repository.Add(department));
        Assert.IsInstanceOfType<DbUpdateException>(exception.InnerException);
    }

    [TestMethod]
    public void Add_WhenNameIsIncorrect()
    {
        var department = new Department(110, "ああああああああああああああああああああ部"); // 部署名:21文字(最大20文字)

        var exception = Assert.ThrowsException<InternalException>(() => _repository.Add(department));
        Assert.IsInstanceOfType<DbUpdateException>(exception.InnerException);
    }
}
