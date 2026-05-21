using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;

namespace Employee_Management_System.Applications.Repositories;
public interface IAdminRepository
{
    List<Admin> FindAll();

    Admin? FindById(string id);

    void Add(Admin domain);

    void UpdateById(string id, Admin domain);

    void DeleteById(string id);
}