using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Applications.Repositories;
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
        List<Department> entityList = _context.Departments.Select(d => _adapter.Restore(d))
                                                          .OrderBy(d => d.DeptNo)
                                                          .ToList();

        return entityList;
    }

    public Department? FindByNumber(int number)
    {
        DepartmentEntity? entity = _context.Departments.Find(number);
        
        return entity != null ? _adapter.Restore(entity) : null;
    }

    public bool HasSameDeptName(string deptName)
    {
        DepartmentEntity? entity = _context.Departments.Where(d => d.DeptName == deptName)
                                                       .FirstOrDefault();
        return entity == null? false : true;
    }

    public void Add(Department domain)
    {
        DepartmentEntity entity = _adapter.Convert(domain);
        _context.Departments.Add(entity);

        _context.SaveChanges();
    }

    public void UpdateByNumber(int number, Department domain)
    {
        DepartmentEntity targetEntity = _context.Departments.Find(number)!;
        DepartmentEntity updateEntity = _adapter.Convert(domain);

        targetEntity.DeptNo = updateEntity.DeptNo;
        targetEntity.DeptName = updateEntity.DeptName;

        _context.SaveChanges();
    }

    public void DeleteByNumber(int number)
    {
        DepartmentEntity entity = _context.Departments.Find(number)!;
        
        _context.Departments.Remove(entity);

        _context.SaveChanges();
    }
}