using Capstone.Exceptions;
using Capstone.Inventory;
using Capstone.TransactionLogging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        //Private variables
        private InventoryFileDAL inventorySource = new InventoryFileDAL("vendingmachine.csv");
        private TransactionFileLog transactionLogger = new TransactionFileLog("transactionlog.txt");
        private Dictionary<string, List<VendingMachineItem>> inventory;
        private decimal currentBalance;
        
        public VendingMachine()
        {
            this.inventory = inventorySource.GetInventory();
        }

        /// <summary>
        /// Constructor for new vending machine. Inventory is automatically stocked when creating the machine.
        /// </summary>
        public VendingMachine(Dictionary<string, List<VendingMachineItem>> inventory)
        {
            this.inventory = inventory;
        }

        /// <summary>
        /// Reflects the Current Balance (in dollars and cents) for the Vending Machine
        /// </summary>
        public decimal CurrentBalance
        {
            get { return this.currentBalance; }
        }

        /// <summary>
        /// Returns all of the available slots in the inventory.
        /// </summary>
        public string[] Slots
        {
            get { return inventory.Keys.ToArray(); }
        }


        /// <summary>
        /// Purchases the item at Slot Id. Ensures the slot exists, quantity is available, and a sufficient balance is provided.
        /// </summary>
        /// <param name="slotID"></param>
        /// <returns></returns>
        public VendingMachineItem Purchase(string slotID)
        {
            if (!inventory.ContainsKey(slotID))
            {
                throw new InvalidSlotIDException($"{slotID} is not a valid slot.");
            }

            if (inventory[slotID].Count == 0)
            {
                throw new OutOfStockException("The quantity in stock is 0.");
            }

            VendingMachineItem nextItem = inventory[slotID][0];
            if (currentBalance < nextItem.Price)
            {
                decimal difference = CurrentBalance - nextItem.Price;
                throw new InsufficientFundsException($"Balance is insufficient to purchase this item. You are short by {difference.ToString("C")}.");
            }

            decimal initialBalance = CurrentBalance;

            inventory[slotID].RemoveAt(0);            
            currentBalance -= nextItem.Price;

            transactionLogger.RecordPurchase(nextItem.ItemName, slotID, currentBalance + nextItem.Price, currentBalance);            

            return nextItem;
        }



        /// <summary>
        /// Returns a vending machine item at a given slot.
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        /// <remarks>Does not return a reference to the item in inventory.</remarks>
        public VendingMachineItem GetItemAtSlot(string slotId)
        {
            if (!inventory.ContainsKey(slotId))
            {
                throw new InvalidSlotIDException($"{slotId} is not a valid slot");
            }

            VendingMachineItem nextItem = (inventory[slotId].Count == 0) ? null : inventory[slotId][0];
            return nextItem;            
        }

        /// <summary>
        /// Returns the quantity remaining for a given slot
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        public int GetQuantityRemaining(string slotId)
        {
            if (!inventory.ContainsKey(slotId))
            {
                throw new InvalidSlotIDException($"{slotId} is not a valid slot");                
            }

            return inventory[slotId].Count;
        }


        /// <summary>
        /// Feeds money into the vending machine.
        /// </summary>        
        public void FeedMoney(int dollars)
        {
            int[] acceptableDollars = { 1, 2, 5, 10 };

            if (!acceptableDollars.Contains(dollars))
            {
                throw new VendingMachineException("Invalid bill inserted. Only provide $1, $2, $5, or $10");
            }

            currentBalance += dollars;

            transactionLogger.RecordDeposit(dollars, currentBalance);
        }

        /// <summary>
        /// Called by the user interface to return the Change.
        /// </summary>        
        public Change ReturnChange()
        {
            transactionLogger.RecordCompleteTransaction(currentBalance);

            Change change = new Change(currentBalance);
            currentBalance = 0;
            return change;
        }        
    }
}
