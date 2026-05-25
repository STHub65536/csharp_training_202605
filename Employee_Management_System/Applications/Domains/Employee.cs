using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Employee_Management_System.Applications.Domains;
public class Employee
{
    public int? EmpNo { get; }
    public string EmpName { get; }
    public DateOnly Birthday { get; }
    public string MailAddress { get; }
    public int? DeptNo { get; }
    public Department? Dept { get; }

    public static readonly int MAX_AGE = 100;
    public static readonly int MIN_AGE = 15;

    public Employee(int? EmpNo, string EmpName, DateOnly Birthday, string MailAddress, int? DeptNo, Department? Dept)
    {
        this.EmpNo = EmpNo;
        this.EmpName = EmpName;
        this.Birthday = Birthday;
        this.MailAddress = MailAddress;
        this.DeptNo = DeptNo;
        this.Dept = Dept;
    }

    public Employee(int? EmpNo, string EmpName, DateOnly Birthday, string MailAddress, int? DeptNo)
    {
        this.EmpNo = EmpNo;
        this.EmpName = EmpName;
        this.Birthday = Birthday;
        this.MailAddress = MailAddress;
        this.DeptNo = DeptNo;
    }

    public Employee(string EmpName, DateOnly Birthday, string MailAddress, int? DeptNo)
    {
        this.EmpNo = EmpNo;
        this.EmpName = EmpName;
        this.Birthday = Birthday;
        this.MailAddress = MailAddress;
        this.DeptNo = DeptNo;
    }

    public bool CheckCorrectAge()
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

        int age = currentDate.Year - Birthday.Year;
        if(Birthday.Month > currentDate.Month || (Birthday.Month == currentDate.Month && Birthday.Day > currentDate.Day))
        {
            age--;
        }

        if(MIN_AGE <= age && age <= MAX_AGE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool Equals(object? obj)
    {
        if(ReferenceEquals(this, obj))
        {
            return true;
        }
        if(obj is not Employee other) {
            return false;
        }
        return EmpNo == other.EmpNo;
    }

    public override int GetHashCode() => EmpNo?.GetHashCode() ?? 0;
}