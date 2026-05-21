using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;

namespace Employee_Management_System.Applications.Repositories;
public interface IEmployeeRepository
{
    List<Employee> FindAll();

    Employee? FindByNumber(int number);

    bool HasSameMailAddress(string mailAddress);

    void Add(Employee domain);

    void UpdateByNumber(Employee domain);

    void DeleteByNumber(int number);
}