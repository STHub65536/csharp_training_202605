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

public class AdminRepository : IAdminRepository
{
    private AppDbContext _context;
    private AdminEntityAdapter _adapter;

    public AdminRepository(AppDbContext context, AdminEntityAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }
    
    public List<Admin> FindAll()
    {
        List<Admin> domainList = _context.Admins.Select(a => _adapter.Restore(a))
                                                .OrderBy(a => a.UserId)
                                                .ToList();

        return domainList;
    }

    public Admin? FindById(string id)
    {
        AdminEntity? entity = _context.Admins.Find(id);

        return entity != null? _adapter.Restore(entity) : null;
    }

    public void Add(Admin domain)
    {
        AdminEntity entity = _adapter.Convert(domain);
        _context.Admins.Add(entity);

        _context.SaveChanges();
    }

    public void UpdateById(string id, Admin domain)
    {
        AdminEntity targetEntity = _context.Admins.Find(id)!;
        AdminEntity updateEntity = _adapter.Convert(domain);

        targetEntity.UserId = updateEntity.UserId;
        targetEntity.UserName = updateEntity.UserName;

        _context.SaveChanges();
    }

    public void DeleteById(string id)
    {
        AdminEntity entity = _context.Admins.Find(id)!;
        
        _context.Admins.Remove(entity);

        _context.SaveChanges();
    }
}