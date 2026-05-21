using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Applications.Repositories;

namespace Employee_Management_System.Applications.Services.Impls;

public class AdminService : IAdminService
{
    private IAdminRepository _repository;

    public AdminService(IAdminRepository repository)
    {
        _repository = repository;
    }

    public List<Admin> GetAdminList()
    {
        return _repository.FindAll();
    }

    public bool IsUserAuthenticated(Admin domain)
    {
        throw new NotImplementedException();
    }

    public void AddAdmin(Admin domain)
    {
        throw new NotImplementedException();
    }

    public bool UpdateAdmin(int id, Admin admin)
    {
        throw new NotImplementedException();
    }

    public void DeleteAdmin(string id)
    {
        throw new NotImplementedException();
    }
}