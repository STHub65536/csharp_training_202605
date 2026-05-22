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
    private IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public List<Department> GetDepartmentList()
    {
        return _repository.FindAll();
    }

    public Department? FindDepartment(int number)
    {
        return _repository.FindByNumber(number);
    }

    public void AddDepartment(Department domain)
    {
        if(_repository.FindByNumber(domain.DeptNo) == null && !_repository.HasSameDeptName(domain.DeptName))
        {
            _repository.Add(domain);
        }
    }

    public void UpdateDepartment(int no, Department domain)
    {
        //更新先の値に重複しているものがないか
        if(_repository.FindByNumber(domain.DeptNo) == null && !_repository.HasSameDeptName(domain.DeptName))
        {
            //元の部署番号(no)が存在するか
            if(_repository.FindByNumber(no) != null)
            {
                _repository.UpdateByNumber(no, domain);   
            }
        }
    }

    public void DeleteDepartment(int no)
    {
        //元の部署番号(no)が存在するか
        if(_repository.FindByNumber(no) != null)
        {
            _repository.DeleteByNumber(no);   
        }
    }
}