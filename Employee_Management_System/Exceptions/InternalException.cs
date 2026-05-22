using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Management_System.Exceptions;
public class InternalException : Exception
{
    public InternalException(string message) 
    : base(message) { }
    public InternalException(string message, Exception innerException) 
    : base(message, innerException) { }
}