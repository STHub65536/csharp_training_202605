using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Adapters;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Infrastructures.Entities;

namespace Employee_Management_System.Infrastructures.Adapters;
public class DepartmentEntityAdapter : IConverter<Department, DepartmentEntity>, IRestorer<Department, DepartmentEntity>
{
    public DepartmentEntity Convert(Department domain)
    {
        return new DepartmentEntity()
        {
            DeptNo = domain.DeptNo,
            DeptName = domain.DeptName  
        };
    }

    public Department Restore(DepartmentEntity target)
    {
        return new Department(
            DeptNo: target.DeptNo,
            DeptName: target.DeptName
        );
    }
}