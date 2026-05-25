using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Applications.Repositories;
using Employee_Management_System.Infrastructures.Repositories;

namespace Employee_Management_System.Applications.Services.Impls;

public class DepartmentService : IDepartmentService
{
    private IDepartmentRepository _departmentRepository;
    private IEmployeeRepository _employeeRepository;

    public DepartmentService(IDepartmentRepository departmentRepository, IEmployeeRepository employeeRepository)
    {
        _departmentRepository = departmentRepository;
        _employeeRepository = employeeRepository;
    }

    public List<Department> GetDepartmentList()
    {
        return _departmentRepository.FindAll();
    }

    public Department? FindDepartment(int number)
    {
        return _departmentRepository.FindByNumber(number);
    }

    public bool IsDepartmentDifferent(Department domain)
    {
        return _departmentRepository.FindByNumber(domain.DeptNo) == null && !_departmentRepository.HasSameDeptName(domain.DeptName);
    }

    public void AddDepartment(Department domain)
    {
        if(_departmentRepository.FindByNumber(domain.DeptNo) == null && !_departmentRepository.HasSameDeptName(domain.DeptName))
        {
            _departmentRepository.Add(domain);
        }
    }

    public void UpdateDepartment(int no, Department domain)
    {
        //更新先の値に重複しているものがないか
        if(_departmentRepository.FindByNumber(domain.DeptNo) == null && !_departmentRepository.HasSameDeptName(domain.DeptName))
        {
            //元の部署番号(no)が存在するか
            if(_departmentRepository.FindByNumber(no) != null)
            {
                _departmentRepository.UpdateByNumber(no, domain);   
            }
        }
    }

    public void DeleteDepartment(int no)
    {
        //元の部署番号(no)が存在するか
        if(_departmentRepository.FindByNumber(no) != null)
        {
            _departmentRepository.DeleteByNumber(no);   
        }
    }

    public bool HasEmployees(int no)
    {
        List<Employee> domainList = _employeeRepository.FindAll();
        
        return domainList.Where(d => d.DeptNo == no).Count() >= 1;
    }
}