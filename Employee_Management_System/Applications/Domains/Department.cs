using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Management_System.Applications.Domains;
public class Department
{
    public int DeptNo { get; set; }
    public string DeptName { get; set; }

    public Department(int DeptNo, string DeptName)
    {
        this.DeptNo = DeptNo;
        this.DeptName = DeptName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is not Department other) return false;
        return DeptNo == other.DeptNo;
    }
    public override int GetHashCode() => DeptNo.GetHashCode();

}