using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Adapters;
using Employee_Management_System.Applications.Domains;

namespace Employee_Management_System.Presentations.ViewModels.Adapters;

public class EmployeeViewModelAdapter : IConverter<Employee, EmployeeViewModel>, IRestorer<Employee, EmployeeViewModel>
{
    public EmployeeViewModel Convert(Employee domain)
    {
        return new EmployeeViewModel{
            EmpNo = domain.EmpNo,
            EmpName = domain.EmpName,
            Birthday = domain.Birthday,
            MailAddress = domain.MailAddress,
            DeptNo = domain.DeptNo,

            Dept = domain.Dept != null ? new Department(DeptNo: domain.Dept.DeptNo, DeptName: domain.Dept.DeptName):
                                         null
        };
    }

    public Employee Restore(EmployeeViewModel target)
    {
        int? deptNo = 0;
        if(target.ChangedDeptNo == 0)
        {
            target.ChangedDeptNo = null;
        }
        if(target.ChangedDeptNo == target.DeptNo)
        {
            deptNo = target.DeptNo;
        }
        else
        {
            deptNo = target.ChangedDeptNo;
        }
        return new Employee(
            EmpNo: target.EmpNo,
            EmpName: target.EmpName,
            Birthday: target.Birthday,
            MailAddress: target.MailAddress,
            DeptNo: deptNo
        );
    }
}