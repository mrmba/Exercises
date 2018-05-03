using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Capstone.Exceptions;

namespace Capstone.Classes
{
    /// <summary>
    /// The VendingMachine CLI handles the entire user interface.
    /// </summary>
    public class VendingMachineCLI
    {
        private const string Option_DisplayVendingMachine = "1";
        private const string Option_DisplayPurchaseMenu = "2";
        private const string Option_InsertMoney = "1";
        private const string Option_MakeSelection = "2";
        private const string Option_ReturnChange = "3";
        private const string Option_ReturnToPreviousMenu = "r";
        private const string Option_Quit = "q";
        private VendingMachine vm;

        public VendingMachineCLI(VendingMachine vm)
        {
            this.vm = vm;
        }

        public void Run()
        {
            while (true)
            {
                PrintTitle();

                Console.WriteLine(" (1) Display vending machine items");
                Console.WriteLine(" (2) Purchase");
                Console.WriteLine(" (Q) Quit");
                Console.Write(" Please make a choice: ");

                string choice = Console.ReadLine().ToLower();

                Console.WriteLine();


                if (choice == Option_DisplayVendingMachine)
                {
                    DisplayInventory();
                    Console.ReadKey();
                }
                else if (choice == Option_DisplayPurchaseMenu)
                {
                    DisplayPurchaseMenu();
                }
                else if (choice == Option_Quit)
                {
                    return;
                }
                else
                {
                    DisplayInvalidOption();
                    Console.ReadKey();
                }

                Console.Clear();
            }
        }

        private void PrintTitle()
        {
            Console.Clear();

            Console.WriteLine("****************************************************************");
            Console.WriteLine("*                       VENDO-MATIC 5000                       *");
            Console.WriteLine("****************************************************************");

            Console.WriteLine();
        }

        private void DisplayPurchaseMenu()
        {

            while (true)
            {
                PrintTitle();

                Console.WriteLine(" (1) Insert money");
                Console.WriteLine(" (2) Make a selection");
                Console.WriteLine(" (3) Finish Transaction");
                Console.WriteLine(" (R) Return to Main Menu");
                Console.WriteLine();
                Console.WriteLine($" Current balance: {vm.CurrentBalance.ToString("C")}");
                Console.Write(" Please make a choice: ");                ;

                string choice = Console.ReadLine().ToLower();

                Console.WriteLine();

                if (choice == Option_InsertMoney)
                {
                    Console.Write(" How much money do you want to enter? ($1, $2, $5, $10): ");
                    int moneyIn = int.Parse(Console.ReadLine());

                    vm.FeedMoney(moneyIn);
                }
                else if (choice == Option_MakeSelection)
                {
                    PrintTitle();

                    Console.WriteLine($" Current balance: {vm.CurrentBalance.ToString("C")}");
                    DisplayInventory();

                    Console.WriteLine();

                    Console.Write(" Please select a slot id: ");
                    string slot = Console.ReadLine().ToUpper();

                    Console.WriteLine();

                    try
                    {
                        VendingMachineItem purchasedItem = vm.Purchase(slot);                      
                        Console.WriteLine(" Here are your " + purchasedItem.ItemName);
                        Console.WriteLine(" " + purchasedItem.Consume());
                    }
                    catch (VendingMachineException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        Console.WriteLine();
                        Console.WriteLine(" Thank you for using Vendo-Matic!");
                        Console.ReadKey();
                    }                                        
                }
                else if (choice == Option_ReturnChange)
                {
                    DisplayReturnedChange();
                    Console.ReadKey();
                }
                else if (choice == Option_ReturnToPreviousMenu)
                {
                    Console.WriteLine(" Returning to previous menu. ");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    DisplayInvalidOption();
                }                
            }
        }

        private void DisplayInvalidOption()
        {
            Console.WriteLine(" The option you selected is not a valid option.");
            Console.WriteLine();
        }

        private void DisplayReturnedChange()
        {
            Change change = vm.ReturnChange();

            Console.WriteLine($" Your change is: {change.TotalChange.ToString("C")}");
            Console.WriteLine($"  {change.Quarters.ToString().PadLeft(3)} quarters");
            Console.WriteLine($"  {change.Dimes.ToString().PadLeft(3)} dimes");
            Console.WriteLine($"  {change.Nickels.ToString().PadLeft(3)} nickels");
            Console.WriteLine();
        }

        private void DisplayInventory()
        {
            Console.Write(" Location".PadRight(11));
            Console.Write("Product Name".PadRight(40));
            Console.WriteLine("Purchase Price".PadRight(18));
            
            // Get All Known Slots
            string[] existingSlots = vm.Slots;

            // Loop Through Each Slot and Display Its Item Data
            foreach (string slotId in existingSlots)
            {
                Console.Write(" " + slotId.PadRight(10));

                VendingMachineItem availableItem = vm.GetItemAtSlot(slotId);

                if (availableItem == null)
                {
                    Console.WriteLine("* SOLD OUT *".PadRight(40));
                }
                else
                {
                    Console.Write(availableItem.ItemName.PadRight(40));
                    Console.Write((availableItem.Price.ToString("C").PadRight(18)));
                    Console.WriteLine($"In Stock ({vm.GetQuantityRemaining(slotId)})".PadRight(20));
                }
        
            }
        }
    }
}