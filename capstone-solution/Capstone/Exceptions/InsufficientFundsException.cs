using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Exceptions
{
    public class InsufficientFundsException : VendingMachineException
    {
        public InsufficientFundsException(string message)
            : base(message)
        {
        }
        
    }
}
