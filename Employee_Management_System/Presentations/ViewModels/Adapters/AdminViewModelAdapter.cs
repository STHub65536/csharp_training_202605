using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Adapters;
using Employee_Management_System.Applications.Domains;

namespace Employee_Management_System.Presentations.ViewModels.Adapters;

public class AdminViewModelAdapter : IConverter<Admin, AdminViewModel>, IRestorer<Admin, AdminViewModel>
{
    public AdminViewModel Convert(Admin domain)
    {
        return new AdminViewModel()
        {
            UserId = domain.UserId,
            Password = domain.Password,
            UserName = domain.UserName
        };
    }

    public Admin Restore(AdminViewModel target)
    {
        return new Admin(
            UserId: target.UserId,
            Password: target.Password,
            UserName: target.UserName
        );
    }
}