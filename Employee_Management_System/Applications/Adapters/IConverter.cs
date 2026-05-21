using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Management_System.Applications.Adapters;
public interface IConverter<TDomain, TTarget>
{
    TTarget Convert(TDomain domain);
}