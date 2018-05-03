using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    public class ChipItem : VendingMachineItem
    {
        public ChipItem(string itemName, decimal priceInCents) : base(itemName, priceInCents)
        {
        }

        public override string Consume()
        {
            return "Crunch Crunch, Yum!";
        }
    }
}
