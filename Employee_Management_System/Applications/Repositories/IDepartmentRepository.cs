using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;

namespace Employee_Management_System.Applications.Repositories;
public interface IDepartmentRepository
{
    List<Department> FindAll();

    Department? FindByNumber(int number);

    void Add(Department domain);

    void UpdateByNumber(int number, Department domain);

    void DeleteByNumber(int number);
}