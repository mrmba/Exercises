using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    public abstract class VendingMachineItem
    {
        // Private Variables
        private string itemName;
        private decimal priceInCents;
        
        // Properties
        public string ItemName
        {
            get { return itemName; }
        }

        public decimal Price
        {
            get { return priceInCents; }
        }        
        
        //Constructors
        public VendingMachineItem(string itemName, decimal price)
        {
            this.itemName = itemName;
            this.priceInCents = price;
        }

        public abstract string Consume();
    }
}
