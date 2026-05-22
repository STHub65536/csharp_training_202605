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
        try
        {
            List<Admin> domainList = _context.Admins.Select(a => _adapter.Restore(a))
                                                    .OrderBy(a => a.UserId)
                                                    .ToList();

            return domainList;
        }
        catch(Exception e)
        {
            throw new InternalException("すべての管理者を取得できませんでした。",e);
        }
    }

    public Admin? FindById(string id)
    {
        try
        {
            AdminEntity? entity = _context.Admins.Find(id);

            return entity != null? _adapter.Restore(entity) : null;
        }
        catch(Exception e)
        {
            throw new InternalException("指定した管理者を取得できませんでした。",e);
        }
    }

    public void Add(Admin domain)
    {
        try
        {
            AdminEntity entity = _adapter.Convert(domain);
            _context.Admins.Add(entity);

            _context.SaveChanges();
        }
        catch(Exception e)
        {
            throw new InternalException("管理者を取得できませんでした。",e);
        }
    }

    public void UpdateById(string id, Admin domain)
    {
        try
        {
            AdminEntity targetEntity = _context.Admins.Find(id)!;
            AdminEntity updateEntity = _adapter.Convert(domain);

            targetEntity.UserId = updateEntity.UserId;
            targetEntity.UserName = updateEntity.UserName;

            _context.SaveChanges();
        }
        catch(Exception e)
        {
            throw new InternalException("指定した管理者を更新できませんでした。",e);
        }
    }

    public void DeleteById(string id)
    {
        try
        {
            AdminEntity entity = _context.Admins.Find(id)!;
            
            _context.Admins.Remove(entity);

            _context.SaveChanges();
        }
        catch(Exception e)
        {
            throw new InternalException("指定した管理者を削除できませんでした。",e);
        }
    }
}