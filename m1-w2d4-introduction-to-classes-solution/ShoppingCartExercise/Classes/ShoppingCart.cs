using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechElevator.Classes
{
    public class ShoppingCart
    {

        public int TotalNumberOfItems { get; private set; }       
        public decimal TotalAmountOwed { get; private set; }
        

        /**
        * The average price of all items that have been added to the cart.  This should be equal to 
        * the totalAmountOwed divided by the totalNumberOfItems.
        * 
        * @return the average price of items added to the cart
        */
        public decimal GetAveragePricePerItem()
        {
            if (this.TotalNumberOfItems == 0)
            {
                return 0;
            }
            else
            {
                return this.TotalAmountOwed / this.TotalNumberOfItems;
            }
        }

        /**
        * Adds items to the cart.
        * 
        * @param numberOfItems the number of items being added to the cart
        * @param pricePerItem the price per item being added to the cart
        */
        public void AddItems(int numberOfItems, decimal pricePerItem)
        {
            this.TotalNumberOfItems += numberOfItems;
            this.TotalAmountOwed += (pricePerItem * numberOfItems);
        }

        /**
        * Removes all items from this cart.
        */
        public void Empty()
        {
            this.TotalNumberOfItems = 0;
            this.TotalAmountOwed = 0;
        }
    }
}
