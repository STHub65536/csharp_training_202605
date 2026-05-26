using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Applications.Repositories;
using Employee_Management_System.Exceptions;
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
        try
        {
            List<Employee> domainList = _context.Employees.Include(e => e.Dept)
                                                                .OrderBy(e => e.EmpNo)
                                                                .ToList()
                                                                .Select(e => _adapter.Restore(e))
                                                                .ToList();
            return domainList;
        }
        catch(Exception e)
        {
            throw new InternalException("すべての社員を取得できませんでした。",e);
        }
    }

    public Employee? FindByNumber(int number)
    {
        try
        {
            EmployeeEntity? entity = _context.Employees.Where(d => d.EmpNo == number).FirstOrDefault()!;
            
            return entity != null ? _adapter.Restore(entity) : null;
        }
        catch(Exception e)
        {
            throw new InternalException("指定した社員を取得できませんでした。",e);
        }
    }

    public bool HasSameMailAddress(string mailAddress)
    {
        try
        {
            EmployeeEntity? entity = _context.Employees.Where(e => e.MailAddress == mailAddress)
                                                    .FirstOrDefault();
            
            return entity == null? false: true;
        }
        catch(Exception e)
        {
            throw new InternalException("メールアドレスでの検索時にエラーが発生しました。",e);
        }
    }

    public void Add(Employee domain)
    {
        try
        {
            EmployeeEntity target = _adapter.Convert(domain);
            _context.Employees.Add(target);

            _context.SaveChanges();
        }
        catch(Exception e)
        {
            throw new InternalException("社員を追加できませんでした。",e);
        }
    }

    public void UpdateByNumber(Employee domain)
    {
        try
        {
            EmployeeEntity targetEntity = _context.Employees.Where(e => e.EmpNo == (domain.EmpNo - 1000)).FirstOrDefault()!;
            EmployeeEntity updateEntity = _adapter.Convert(domain);

            targetEntity.EmpName = updateEntity.EmpName;
            targetEntity.Birthday = updateEntity.Birthday;
            targetEntity.MailAddress = updateEntity.MailAddress;
            targetEntity.DeptNo = updateEntity.DeptNo;

            _context.SaveChanges();
        }
        catch(Exception e)
        {
            throw new InternalException("指定した社員を更新できませんでした",e);
        }
    }

    public void DeleteByNumber(int number)
    {
        try
        {
            EmployeeEntity? entity = _context.Employees.Where(d => d.EmpNo == number).FirstOrDefault()!;
            _context.Employees.Remove(entity);

            _context.SaveChanges();
        }
        catch(Exception e)
        {
            throw new InternalException("指定した社員を削除できませんでした",e);
        }
    }
}