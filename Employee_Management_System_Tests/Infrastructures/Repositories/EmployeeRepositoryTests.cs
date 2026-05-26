using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Exceptions;
using Employee_Management_System.Infrastructures.Adapters;
using Employee_Management_System.Infrastructures.Context;
using Employee_Management_System.Infrastructures.Repositories;
using Microsoft.AspNetCore.Http.Features;
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

        var normalPath = Path.Combine(AppContext.BaseDirectory, "sql", "normalInit.sql");
        var normalSql = File.ReadAllText(normalPath);
        _context.Database.ExecuteSqlRaw(normalSql);

        _repository = new EmployeeRepository(_context, adapter);
    }

    [TestMethod]
    public void FindAll_ReturnsAllEmployees()
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

    [TestMethod]
    public void FindAll_ReturnsEmptyList()
    {
        var adapter = new EmployeeEntityAdapter();
        
        var emptyPath = Path.Combine(AppContext.BaseDirectory, "sql", "employeeEmptyInit.sql");
        var emptySql = File.ReadAllText(emptyPath);
        _context.Database.ExecuteSqlRaw(emptySql);

        _repository = new EmployeeRepository(_context, adapter);

        var lists = _repository.FindAll();

        Assert.IsTrue(lists.Count() == 0);
    }

    [TestMethod]
    public void FindAll_WhenDbAccessError()
    {
        _context.Dispose();

        var exception = Assert.ThrowsException<InternalException>(() => _repository.FindAll());
        Assert.IsInstanceOfType<InternalException>(exception);
    }

    [TestMethod]
    public void FindByNumber_WhenNumberCorrect()
    {
        var actual = _repository.FindByNumber(1);

        IsNotNull(actual);
        AreEqual(1001, actual.EmpNo);
        AreEqual("田中太郎", actual.EmpName);
        AreEqual("2003-02-05", actual.Birthday.ToString("yyyy-MM-dd"));
        AreEqual("aaabbbccc1234@gmail.com", actual.MailAddress);
        AreEqual(101, actual.DeptNo);
    }

    [TestMethod]
    public void FindByNumber_WhenNumberNotFound()
    {
        var actual = _repository.FindByNumber(1100);
        IsNull(actual);
    }

    [TestMethod]
    public void FindByNumber_WhenDbAccessError()
    {
        _context.Dispose();

        var exception = Assert.ThrowsException<InternalException>(() => _repository.FindByNumber(1));
        Assert.IsInstanceOfType<InternalException>(exception);
    }

    [TestMethod]
    public void HasSameMailAddress_WhenMailAddressExists()
    {
        var actual = _repository.HasSameMailAddress("foo89732@ezweb.ne.jp");
        IsTrue(actual);
    }

    [TestMethod]
    public void HasSameMailAddress_WhenMailAddressNotExists()
    {
        var actual = _repository.HasSameMailAddress("hogehoge@example.com");
        IsFalse(actual);
    }

    [TestMethod]
    public void HasSameMailAddress_WhenDbAccessError()
    {
        _context.Dispose();

        var exception = Assert.ThrowsException<InternalException>(() => _repository.HasSameMailAddress("foo89732@ezweb.ne.jp"));
        Assert.IsInstanceOfType<InternalException>(exception);
    }

    [TestMethod]
    public void Add_WhenCorrect()
    {
        var beforeCount = _context.Employees.Count();

        var employee = new Employee("斎藤康太", new DateOnly(2013, 5, 1), "foofoo@ezweb.ne.jp", 104);

        _repository.Add(employee);

        var afterCount = _context.Employees.Count();
        AreEqual(beforeCount + 1, afterCount);

        var created = _context.Employees
            .FirstOrDefault(i => i.MailAddress == "foofoo@ezweb.ne.jp");

        IsNotNull(created);
        AreEqual("斎藤康太", created.EmpName);
        AreEqual("2013-05-01", created.Birthday.ToString("yyyy-MM-dd"));
        AreEqual("foofoo@ezweb.ne.jp", created.MailAddress);
        AreEqual(104, created.DeptNo);
    }

    [TestMethod]
    public void Add_WhenNameIsIncorrect()
    {
        var employee = new Employee("あああああああああああああああああああああ", new DateOnly(2000,1,1), "foo@gmail.com", 101); // 社員名:21文字(最大20文字)

        var exception = Assert.ThrowsException<InternalException>(() => _repository.Add(employee));
        Assert.IsInstanceOfType<InternalException>(exception);
    }

    [TestMethod]
    public void Add_WhenMailAddressIsIncorrect()
    {
        var employee = new Employee("田中次郎", new DateOnly(2000,1,1), "abcdefghijklmnopqrstuvwxyz123456789123456@gmail.com", null); // メールアドレス:51文字(50文字制限)

        var exception = Assert.ThrowsException<InternalException>(() => _repository.Add(employee));
        Assert.IsInstanceOfType<InternalException>(exception);
    }

    [TestMethod]
    public void UpdateByNumber_WhenTargetNotNull()
    {
        var employee = new Employee("田中次郎", new DateOnly(2000,1,1), "hogehogehoge@gmail.com", 101);

        _repository.UpdateByNumber(employee);

        var result = _repository.FindAll().Where(e => e.MailAddress.Equals("hogehogehoge@gmail.com")).FirstOrDefault();
        Assert.AreEqual("田中次郎", result.EmpName);
        Assert.AreEqual("2000-01-01", result.Birthday.ToString("yyyy-MM-dd"));
        Assert.AreEqual(101, result.DeptNo);
    }

    [TestMethod]
    public void UpdateNameByNumber_WhenTargetNull()
    {
        _context.Dispose();
        var employee = new Employee("田中次郎", new DateOnly(2000,1,1), "hogehogehoge@gmail.com", 101);

        var exception = Assert.ThrowsException<InternalException>(() => _repository.UpdateByNumber(employee));
        Assert.IsInstanceOfType<InternalException>(exception); 
    }
}