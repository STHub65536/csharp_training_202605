using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Applications.Repositories;

namespace Employee_Management_System.Applications.Services.Impls;

public class EmployeeService : IEmployeeService
{
    private IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public List<Employee> GetEmployeeList()
    {
        return _repository.FindAll();
    }

    public Employee? FindEmployee(int number)
    {
        return _repository.FindByNumber(number);
    }

    public bool IsEmployeeDifferent(Employee domain)
    {
        return !_repository.HasSameMailAddress(domain.MailAddress);
    }

    public void AddEmployee(Employee domain)
    {
        if(!_repository.HasSameMailAddress(domain.MailAddress))
        {
            
            _repository.Add(domain);
        }
    }

    public void UpdateEmployee(Employee domain)
    {
        _repository.UpdateByNumber(domain);   
    }

    public void DeleteEmployee(int number)
    {
        _repository.DeleteByNumber(number - 1000);
    }
}