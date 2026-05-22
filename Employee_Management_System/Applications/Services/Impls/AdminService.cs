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
        Admin? target = _repository.FindById(domain.UserId);
        if(target == null)
        {
            return false;
        }
        else
        {
            return target.UserName == domain.UserName? true : false;
        }
    }

    public void AddAdmin(Admin domain)
    {
        if(_repository.FindById(domain.UserId) != null)
        {
            _repository.Add(domain);
        }
    }

    public void UpdateAdmin(string id, Admin domain)
    {
        if(_repository.FindById(domain.UserId) != null)
        {
            _repository.UpdateById(id, domain);
        }
    }

    public void DeleteAdmin(string id)
    {
        if(_repository.FindById(id) != null)
        {
            _repository.DeleteById(id);
        }
    }
}