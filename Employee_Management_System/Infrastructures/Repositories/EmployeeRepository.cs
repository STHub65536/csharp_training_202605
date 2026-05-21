using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Applications.Repositories;
using Employee_Management_System.Infrastructures.Adapters;
using Employee_Management_System.Infrastructures.Context;
using Employee_Management_System.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Infrastructures.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private AppDbContext _context;
    private EmployeeEntityAdapter _adapter;

    public EmployeeRepository(AppDbContext context, EmployeeEntityAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }
    
    public List<Employee> FindAll()
    {
        List<EmployeeEntity> entityList = _context.Employees.Include(e => e.Dept)
                                                            .OrderBy(e => e.EmpNo)
                                                            .ToList();
        List<Employee> domainList = new List<Employee>();
        foreach(EmployeeEntity e in entityList)
        {
            domainList.Add(_adapter.Restore(e));
        }

        return domainList;
    }

    public Employee? FindByNumber(int number)
    {
        EmployeeEntity? entity = _context.Employees.Find(number);
        
        return entity != null ? _adapter.Restore(entity) : null;
    }

    public bool HasSameMailAddress(string mailAddress)
    {
        EmployeeEntity? entity = _context.Employees.Where(e => e.MailAddress == mailAddress)
                                                   .FirstOrDefault();
        
        return entity == null? false: true;
    }

    public void Add(Employee domain)
    {
        EmployeeEntity target = _adapter.Convert(domain);
        _context.Employees.Add(target);

        _context.SaveChanges();
    }

    public void UpdateByNumber(int number, Employee domain)
    {
        EmployeeEntity targetEntity = _context.Employees.Find(number)!;
        EmployeeEntity updateEntity = _adapter.Convert(domain);

        targetEntity.EmpNo = updateEntity.EmpNo;
        targetEntity.EmpName = updateEntity.EmpName;
        targetEntity.Birthday = updateEntity.Birthday;
        targetEntity.MailAddress = updateEntity.MailAddress;
        targetEntity.DeptNo = updateEntity.DeptNo;

        _context.SaveChanges();
    }

    public void DeleteByNumber(int number)
    {
        EmployeeEntity? entity = _context.Employees.Find(number)!;
        _context.Employees.Remove(entity);

        _context.SaveChanges();
    }
}