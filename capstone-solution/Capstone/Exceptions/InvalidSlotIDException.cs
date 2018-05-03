using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Exceptions
{
    public class InvalidSlotIDException : VendingMachineException
    {
        public InvalidSlotIDException(string message)
            :base(message)
        {
        }
    }
}
