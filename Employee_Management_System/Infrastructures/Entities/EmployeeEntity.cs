using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Management_System.Infrastructures.Entities;
public class EmployeeEntity
{
    public int? EmpNo { get; set; }
    public string EmpName { get; set; }
    public DateOnly Birthday { get; set; }
    public string MailAddress { get; set; }
    public int? DeptNo { get; set; }

    public DepartmentEntity? Dept { get; set; }
}