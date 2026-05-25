using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;

namespace Employee_Management_System.Applications.Services;
public interface IDepartmentService
{
    List<Department> GetDepartmentList();
    Department? FindDepartment(int number);
    bool IsDepartmentDifferent(Department domain);
    void AddDepartment(Department domain);
    void UpdateDepartment(int no, Department domain);
    void DeleteDepartment(int no);
    bool HasEmployees(int no);
}