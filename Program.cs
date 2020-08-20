using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstBankOfSuncoast
{
    // Create Transaction Class
    class Transaction
    {
        // String: Description
        public string Description { get; set; }
        // Int: Amount
        public double Amount { get; set; }
        // Type: String (Deposit or Withdrawal)
        public string Type { get; set; }
        // Account: String (Checking or Savings)
        public string Account { get; set; }
        // 
        public string LineItem()
        {
            var lineItem = $"{Description} {Type} {Account} {Amount}";
            return lineItem;
        }
    }
    class Program
    {
        // Checking Transaction List
        public static IEnumerable<Transaction> GetCheckingTransactions(List<Transaction> transactions)
        {
            var checkingTransactions = transactions.Where(transaction => transaction.Account == "Checking");
            return checkingTransactions;
        }

        // Savings Transaction List
        public static IEnumerable<Transaction> GetSavingsTransactions(List<Transaction> transactions)
        {
            var savingsTransactions = transactions.Where(transaction => transaction.Account == "Savings");
            return savingsTransactions;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the First Bank of Suncoast");
            // TODO: Test Transactions

            var transactions = new List<Transaction>()
            {
              new Transaction
              {
                Description = "Paycheck",
                Amount = 2538.67,
                Type = "Deposit",
                Account = "Checking",
              },
              new Transaction
              {
                Description = "Savings Deposit",
                Amount = 500.00,
                Type = "Deposit",
                Account = "Savings",
              },
              new Transaction
              {
                Description = "Cable Bill",
                Amount = 175.35,
                Type = "Withdrawal",
                Account = "Checking",
              },
              new Transaction
              {
                Description = "IRA",
                Amount = 250,
                Type = "Withdrawal",
                Account = "Savings",
              },
            };

            // Show Transactions
            // foreach (var transaction in transactions)
            // {
            //     Console.WriteLine(transaction.LineItem());
            // }

            // Create Checking View
            Console.Clear();
            Console.WriteLine("*** Checking ***\n");

            var checkingAccount = new List<Transaction>();

            foreach (var item in GetCheckingTransactions(transactions))
            {
                checkingAccount.Add(item);
                Console.WriteLine(item.LineItem());
            }

            // Create Savings View
            // Console.Clear();
            Console.WriteLine("*** Savings ***\n");

            var savingsAccount = new List<Transaction>();

            foreach (var item in GetSavingsTransactions(transactions))
            {
                savingsAccount.Add(item);
                Console.WriteLine(item.LineItem());
            }



            // PEDAC
            // TODO: Load or Create File
            // TODO: Create Checking View
            // TODO: Create Savings View
            // TODO: Create Transactions List
        }
    }
}
