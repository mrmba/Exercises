using Capstone.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Inventory
{
    public class InventoryFileDAL
    {
        private string filepath;
        const int SlotId = 0;
        const int Product = 1;
        const int Cost = 2;
        const int InitialQuantity = 5;


        public InventoryFileDAL(string filepath)
        {
            this.filepath = filepath;
        }

        // Read the File Path and Create the Dictionary that represents the machine inventory
        public Dictionary<string, List<VendingMachineItem>> GetInventory()
        {
            List<string[]> fullList = new List<string[]>();
            Dictionary<string, List<VendingMachineItem>> stock = new Dictionary<string, List<VendingMachineItem>>();

            try
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] lineData = reader.ReadLine().Split('|');
                        fullList.Add(lineData);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("An error occurred reading the initial inventory file. " + ex.Message);
                throw ex;
            }

            foreach (string[] lineData in fullList)
            {
                stock.Add(lineData[SlotId], CreateInitialInventory(lineData));
            }

            return stock;
        }

        // Read each line data item and create a list representing the initial inventory
        private List<VendingMachineItem> CreateInitialInventory(string[] lineData)
        {
            // This little trick allows us to eliminate a lot of the duplicate
            // code that we would have spent writing a bunch of if/else if/else 
            // statements. We use the Reflection library to activate instances
            // just by knowing the Type.
            Dictionary<string, Type> slotTypes = new Dictionary<string, Type>()
            {
                {"A", typeof(ChipItem) },
                {"B", typeof(CandyItem) },
                {"C", typeof(BeverageItem) },
                {"D", typeof(GumItem) }
            };

            string firstCharacter = lineData[SlotId].Substring(0, 1);
            decimal cost = decimal.Parse(lineData[Cost]);
            string name = lineData[Product];

            Type typeToCreate = slotTypes[firstCharacter];
            List<VendingMachineItem> initialStock = new List<VendingMachineItem>();

            for (int i = 0; i < InitialQuantity; i++)
            {
                initialStock.Add((VendingMachineItem)Activator.CreateInstance(typeToCreate, name, cost));    
            }

            return initialStock;
        }        
    }
}
