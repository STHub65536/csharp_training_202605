using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Exceptions;
using Employee_Management_System.Infrastructures.Adapters;
using Employee_Management_System.Infrastructures.Context;
using Employee_Management_System.Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Employee_Management_System_Tests.Infrastructures.Repositories;
[DoNotParallelize]
[TestClass]
public class EmployeeRepositoryTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=employee_management;Username=postgres;Password=training;";

    private EmployeeRepository _repository = null!;
    private AppDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var adapter = new EmployeeEntityAdapter();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "init.sql");
        var sql = File.ReadAllText(path);
        _context.Database.ExecuteSqlRaw(sql);

        _repository = new EmployeeRepository(_context, adapter);
    }

    [TestMethod]
    public void FindAll_ReturnsAllDepartments()
    {
        var lists = _repository.FindAll();

        AreEqual(1001, lists[0].EmpNo);
        AreEqual("田中太郎", lists[0].EmpName);
        AreEqual("2003-02-05", lists[0].Birthday.ToString("yyyy-MM-dd"));
        AreEqual("aaabbbccc1234@gmail.com", lists[0].MailAddress);
        AreEqual(101, lists[0].DeptNo);

        AreEqual(1002, lists[1].EmpNo);
        AreEqual("鈴木三郎", lists[1].EmpName);
        AreEqual("2002-03-06", lists[1].Birthday.ToString("yyyy-MM-dd"));
        AreEqual("hoge@example.com", lists[1].MailAddress);
        AreEqual(102, lists[1].DeptNo);

        AreEqual(1003, lists[2].EmpNo);
        AreEqual("佐藤花子", lists[2].EmpName);
        AreEqual("2001-04-07", lists[2].Birthday.ToString("yyyy-MM-dd"));
        AreEqual("foo89732@ezweb.ne.jp", lists[2].MailAddress);
        AreEqual(103, lists[2].DeptNo);

        AreEqual(1004, lists[3].EmpNo);
        AreEqual("中田彩子", lists[3].EmpName);
        AreEqual("2000-05-08", lists[3].Birthday.ToString("yyyy-MM-dd"));
        AreEqual("cccbbbaaa65536@gmail.com", lists[3].MailAddress);
        AreEqual(104, lists[3].DeptNo);

        AreEqual(1005, lists[4].EmpNo);
        AreEqual("加藤圭太", lists[4].EmpName);
        AreEqual("1999-06-09", lists[4].Birthday.ToString("yyyy-MM-dd"));
        AreEqual("fdssajfljfd21@example.com", lists[4].MailAddress);
        AreEqual(105, lists[4].DeptNo);

        AreEqual(1006, lists[5].EmpNo);
        AreEqual("松本良太", lists[5].EmpName);
        AreEqual("1998-07-10", lists[5].Birthday.ToString("yyyy-MM-dd"));
        AreEqual("woierufd234253@ezweb.ne.jp", lists[5].MailAddress);
        AreEqual(null, lists[5].DeptNo);
    }

    // [TestMethod]
    // public void FindByNumber_WhenNumberCorrect()
    // {
    //     var actual = _repository.FindByNumber(101);

    //     IsNotNull(actual);
    //     AreEqual(101, actual.DeptNo);
    //     AreEqual("総務部", actual.DeptName);
    // }

    // [TestMethod]
    // public void FindByNumber_WhenNumberNotFound()
    // {
    //     var actual = _repository.FindByNumber(999);
    //     IsNull(actual);
    // }

    // [TestMethod]
    // public void HasSameDeptName_WhenNameExists()
    // {
    //     var actual = _repository.HasSameDeptName("総務部");
    //     IsTrue(actual);
    // }

    // [TestMethod]
    // public void HasSameDeptName_WhenNameNotExists()
    // {
    //     var actual = _repository.HasSameDeptName("情報システム部");
    //     IsFalse(actual);
    // }

    // [TestMethod]
    // public void Add_WhenCorrect()
    // {
    //     var beforeCount = _context.Departments.Count();

    //     var department = new Department(110, "検証部");

    //     _repository.Add(department);

    //     var afterCount = _context.Departments.Count();
    //     AreEqual(beforeCount + 1, afterCount);

    //     var created = _context.Departments
    //         .FirstOrDefault(i => i.DeptNo == 110);

    //     IsNotNull(created);
    //     AreEqual("検証部", created.DeptName);
    // }

    // [TestMethod]
    // public void Add_WhenNumberIsIncorrect()
    // {
    //     var department = new Department(1000, "検証部"); //4桁(最大3桁)

    //     var exception = Assert.ThrowsException<InternalException>(() => _repository.Add(department));
    //     Assert.IsInstanceOfType<DbUpdateException>(exception.InnerException);
    // }

    // [TestMethod]
    // public void Add_WhenNameIsIncorrect()
    // {
    //     var department = new Department(110, "ああああああああああああああああああああ部"); // 21文字(20文字制限)

    //     var exception = Assert.ThrowsException<InternalException>(() => _repository.Add(department));
    //     Assert.IsInstanceOfType<DbUpdateException>(exception.InnerException);
    // }
}