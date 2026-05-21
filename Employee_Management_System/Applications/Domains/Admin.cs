using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Management_System.Applications.Domains;
public class Admin
{
    public string UserId { get; }
    public string Password { get; }
    public string UserName { get; }

    public Admin(string UserId, string Password, string UserName)
    {
        this.UserId = UserId;
        this.Password = Password;
        this.UserName = UserName;
    }

    public override bool Equals(object? obj)
    {
        if(ReferenceEquals(this, obj))
        {
            return true;
        }
        if(obj is not Admin other) {
            return false;
        }
        return UserId == other.UserId;
    }

    public override int GetHashCode() => UserId?.GetHashCode() ?? 0;
}