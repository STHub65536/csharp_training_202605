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

    public void UpdateDepartment(int number, Department domain)
    {

        //元の部署番号(no)が存在するか.
        if(_departmentRepository.FindByNumber(number) != null)
        {
            if(number == domain.DeptNo) // 部署名を変えるだけなので、外部キー制約にひっかからない.
            {
                _departmentRepository.UpdateNameByNumber(number, domain);   
            }
            else // 部署番号が変わるので、該当する社員の所属部署を一時的にNullにしてから部署更新を行い、更新後の所属部署を割り当てる.
            {
                /*
                    sameDepartmentList:更新する部署に所属している社員のリスト.
                    sameDepartmentNullList:社員の所属部署を一時的にNullにしたリスト.
                    sameDepartmentNewList:社員の所属部署に新たな所属部署を割り当てたリスト.
                */
                List<Employee> sameDepartmentList = _employeeRepository.FindAll().Where(e => e.DeptNo == number).ToList();
                if(sameDepartmentList.Count() == 0) // 誰も所属していない
                {
                    _departmentRepository.DeleteByNumber(number);
                    _departmentRepository.Add(domain);
                }
                else
                {
                    List<Employee> sameDepartmentNullList = new List<Employee>();
                    List<Employee> sameDepartmentNewList = new List<Employee>();
                    sameDepartmentList.ForEach(e => sameDepartmentNullList.Add(new Employee(e.EmpNo - 1000, e.EmpName, e.Birthday, e.MailAddress, null)));
                    sameDepartmentList.ForEach(e => sameDepartmentNewList.Add(new Employee(e.EmpNo - 1000, e.EmpName, e.Birthday, e.MailAddress, domain.DeptNo)));

                    foreach(Employee emp in sameDepartmentNullList)
                    {
                        _employeeRepository.UpdateByNumber(emp);
                    }
                    _departmentRepository.DeleteByNumber(number);
                    _departmentRepository.Add(domain);
                    foreach(Employee emp in sameDepartmentNewList)
                    {
                        _employeeRepository.UpdateByNumber(emp);
                    }
                } 
            }
        }
    }

    public void DeleteDepartment(int number)
    {
        //元の部署番号(no)が存在するか
        if(_departmentRepository.FindByNumber(number) != null)
        {
            _departmentRepository.DeleteByNumber(number);   
        }
    }

    public bool HasEmployees(int number)
    {
        List<Employee> domainList = _employeeRepository.FindAll();
        
        return domainList.Where(d => d.DeptNo == number).Count() >= 1;
    }
}