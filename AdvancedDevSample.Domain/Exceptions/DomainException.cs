using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) 
        { 
        
        }
    }
}
