using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Adapters;
using Employee_Management_System.Applications.Domains;

namespace Employee_Management_System.Presentations.ViewModels.Adapters;

public class DepartmentViewModelAdapter : IConverter<Department, DepartmentViewModel>, IRestorer<Department, DepartmentViewModel>
{
    public DepartmentViewModel Convert(Department domain)
    {
        return new DepartmentViewModel()
        {
            DeptNo = domain.DeptNo,
            DeptName = domain.DeptName  
        };
    }

    public Department Restore(DepartmentViewModel target)
    {
        return new Department(
            DeptNo: target.DeptNo,
            DeptName: target.DeptName
        );
    }
}