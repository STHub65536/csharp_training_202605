using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;

namespace Employee_Management_System.Applications.Services;
public interface IEmployeeService
{
    List<Employee> GetEmployeeList();
    Employee? FindEmployee(int number);
    bool IsEmployeeDifferent(Employee domain);
    void AddEmployee(Employee domain);
    void UpdateEmployee(Employee domain);
    void DeleteEmployee(int number);
}