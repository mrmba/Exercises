using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Capstone.TransactionLogging
{
    public class TransactionFileLog
    {
        private string filepath;

        public TransactionFileLog(string filepath)
        {
            this.filepath = filepath;
        }

        public void RecordDeposit(decimal depositAmount, decimal finalBalance)
        {
            WriteTransaction($"FEED MONEY: {depositAmount.ToString("C")}    {finalBalance.ToString("C")}");
        }

        public void RecordPurchase(string productName, string slotId, decimal initialBalance, decimal finalBalance)
        {
            WriteTransaction($"{productName.ToUpper()}  {slotId}    {initialBalance.ToString("C")}  {finalBalance.ToString("C")}");
        }

        public void RecordCompleteTransaction(decimal initialAmount)
        {
            WriteTransaction($"GIVE CHANGE: {initialAmount.ToString("C")}    $0.00");
        }

        private void WriteTransaction(string message)
        {
            string fullPath = Path.Combine(Environment.CurrentDirectory, filepath);

            try
            {
                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                    sw.Write(DateTime.UtcNow.ToLocalTime().ToString().PadRight(25));
                    sw.WriteLine(message);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("An error has occurred recording the transaction: " + ex.Message);
            }
        }        
    }
}
