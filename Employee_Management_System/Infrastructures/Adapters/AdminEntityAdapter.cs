using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Adapters;
using Employee_Management_System.Applications.Domains;
using Employee_Management_System.Infrastructures.Entities;

namespace Employee_Management_System.Infrastructures.Adapters;
public class AdminEntityAdapter : IConverter<Admin, AdminEntity>, IRestorer<Admin, AdminEntity>
{
    public AdminEntity Convert(Admin domain)
    {
        return new AdminEntity()
        {
            UserId = domain.UserId,
            Password = domain.Password,
            UserName = domain.UserName
        };
    }

    public Admin Restore(AdminEntity target)
    {
        return new Admin(
            UserId :  target.UserId,
            Password : target.Password,
            UserName : target.UserName
        );
    }
}