using System;
using System.Collections.Generic;
using System.Linq;
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

    public Employee(int? EmpNo, string EmpName, DateOnly Birthday, string MailAddress, int? DeptNo, Department? Dept)
    {
        this.EmpNo = EmpNo;
        this.EmpName = EmpName;
        this.Birthday = Birthday;
        this.MailAddress = MailAddress;
        this.DeptNo = DeptNo;
        this.Dept = Dept;
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