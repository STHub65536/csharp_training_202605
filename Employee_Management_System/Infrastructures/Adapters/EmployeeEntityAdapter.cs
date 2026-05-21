using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Adapters;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Infrastructures.Entities;

namespace Employee_Management_System.Infrastructures.Adapters;
public class EmployeeEntityAdapter : IConverter<Employee, EmployeeEntity>, IRestorer<Employee, EmployeeEntity>
{
    public EmployeeEntity Convert(Employee domain)
    {
        return new EmployeeEntity()
        {
            EmpNo = domain.EmpNo - 1000,
            EmpName = domain.EmpName,
            Birthday = domain.Birthday,
            MailAddress = domain.MailAddress,
            DeptNo = domain.DeptNo,
        };
    }

    public Employee Restore(EmployeeEntity target)
    {
        return new Employee(
            EmpNo: target.EmpNo + 1000,
            EmpName: target.EmpName,
            Birthday: target.Birthday,
            MailAddress: target.MailAddress,
            DeptNo: target.DeptNo,

            Dept: target.Dept != null ? new Department(DeptNo: target.Dept.DeptNo, DeptName: target.Dept.DeptName):
                                        null
        );   
    }
}