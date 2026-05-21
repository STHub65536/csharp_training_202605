using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Domains;

namespace Employee_Management_System.Applications.Services;
public interface IAdminService
{
    List<Admin> GetAdminList();
    void AddAdmin(Admin domain);
    bool IsUserAuthenticated(Admin domain);
    bool UpdateAdmin(int id, Admin admin);
    void DeleteAdmin(string id);
}