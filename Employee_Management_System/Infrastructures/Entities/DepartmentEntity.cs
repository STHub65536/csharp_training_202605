using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Management_System.Infrastructures.Entities;
public class DepartmentEntity
{
    public int DeptNo { get; set; }
    public string DeptName { get; set; }

    public List<EmployeeEntity> Employees { get; set; }
}