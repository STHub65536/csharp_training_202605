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

namespace Employee_Management_System.Infrastructures.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private AppDbContext _context;
    private DepartmentEntityAdapter _adapter;

    public DepartmentRepository(AppDbContext context, DepartmentEntityAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }
    
    public List<Department> FindAll()
    {
        try
        {
            List<Department> entityList = _context.Departments.Select(d => _adapter.Restore(d))
                                                          .OrderBy(d => d.DeptNo)
                                                          .ToList();
            return entityList;
        }
        catch(Exception e)
        {
            throw new InternalException("すべての部署を取得できませんでした。",e);
        }
    }

    public Department? FindByNumber(int number)
    {
        try
        {
            DepartmentEntity? entity = _context.Departments.Where(d => d.DeptNo == number).FirstOrDefault();

            return entity != null ? _adapter.Restore(entity) : null;
        }
        catch(Exception e)
        {
            throw new InternalException("指定した部署を取得できませんでした。",e);
        }
    }

    public bool HasSameDeptName(string deptName)
    {
        try
        {
            DepartmentEntity? entity = _context.Departments.Where(d => d.DeptName == deptName)
                                                        .FirstOrDefault();
            return entity == null? false : true;
        }
        catch(Exception e)
        {
            throw new InternalException("名前での検索時にエラーが発生しました。",e);
        }
    }

    public void Add(Department domain)
    {
        try
        {
            DepartmentEntity entity = _adapter.Convert(domain);
            _context.Departments.Add(entity);
            _context.SaveChanges();
        }
        catch(Exception e)
        {
            throw new InternalException("部署を追加できませんでした。",e);
        }
    }

    public void UpdateByNumber(int number, Department domain)
    {
        try
        {
            DepartmentEntity targetEntity = _context.Departments.Where(d => d.DeptNo == number).FirstOrDefault()!;
            DepartmentEntity updateEntity = _adapter.Convert(domain);

            targetEntity.DeptNo = updateEntity.DeptNo;
            targetEntity.DeptName = updateEntity.DeptName;

            _context.SaveChanges();
        }
        catch(Exception e)
        {
            throw new InternalException("指定された部署を更新できませんでした。",e);
        }
        
    }

    public void DeleteByNumber(int number)
    {
        try
        {
            DepartmentEntity entity = _context.Departments.Where(d => d.DeptNo == number).FirstOrDefault()!;
            
            _context.Departments.Remove(entity);

            _context.SaveChanges();
        }
        catch(Exception e)
        {
            throw new InternalException("指定された部署を削除できませんでした。",e);
        }
    }
}