using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Management_System.Presentations.ViewModels.ViewModelAttributes;

public class AgeAttribute : ValidationAttribute
{
    public int minAge {get; set; }
    public int maxAge {get; set; }

    public AgeAttribute(int minage, int maxAge)
    {
        this.minAge = minAge;
        this.maxAge = maxAge;
    }

    public override bool IsValid(object? value)
    {
        if(value != null)
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly birthDate = DateOnly.Parse(value.ToString());
            int age = currentDate.Year - birthDate.Year;
            if(currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
            {
                age--;
            }

            if(age >= minAge && age <= maxAge)
            {
                return true;
            }
            else{
                return false;
            }
        }
        return false;
    }
}