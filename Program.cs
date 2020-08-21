using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Globalization;

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

            TextReader reader;

            if (File.Exists("transactions.csv"))
            {
                // if file exists, Assign a streamreader to read from the file
                reader = new StreamReader("transactions.csv");
            }
            else
            {
                // if the file does not exist, read the data from an empty string instead
                reader = new StringReader("");
            }

            // Creates as stream reader to get information from the file
            var fileReader = new StreamReader("transactions.csv");

            // Create a CSV reader to parse the stream into CSV format
            var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);

            // Tell the CSV reader to interpret the first row as a header
            csvReader.Configuration.HasHeaderRecord = true;

            // Get the records from the CSV reader
            var transactions = csvReader.GetRecords<Transaction>().ToList();

            // Close the reader
            fileReader.Close();


            // {
            //   new Transaction
            //   {
            //     Description = "Paycheck",
            //     Amount = 2538.67,
            //     Type = "Deposit",
            //     Account = "Checking",
            //   },
            //   new Transaction
            //   {
            //     Description = "Savings Deposit",
            //     Amount = 500.00,
            //     Type = "Deposit",
            //     Account = "Savings",
            //   },
            //   new Transaction
            //   {
            //     Description = "Cable Bill",
            //     Amount = 175.35,
            //     Type = "Withdrawal",
            //     Account = "Checking",
            //   },
            //   new Transaction
            //   {
            //     Description = "IRA",
            //     Amount = 250,
            //     Type = "Withdrawal",
            //     Account = "Savings",
            //   },
            // };

            // Show Transactions
            // foreach (var transaction in transactions)
            // {
            //     Console.WriteLine(transaction.LineItem());
            // }

            // Start while loop to keep running application
            var quitApplication = "n";
            var screen = "checkingScreen";

            while (quitApplication != "y")
            {
                // Switch to control view
                switch (screen)
                {
                    case "checkingScreen":
                        // Create Checking View
                        Console.Clear();
                        Console.WriteLine("*** Checking ***\n");

                        // List checking transactions and calculate balance
                        var checkingAccount = new List<Transaction>();
                        var checkingBalance = 0.00;

                        foreach (var item in GetCheckingTransactions(transactions))
                        {
                            checkingAccount.Add(item);
                            Console.WriteLine(item.LineItem());

                            // Track Checking Account Balance
                            if (item.Type == "Deposit")
                            {
                                checkingBalance += item.Amount;
                            }
                            else
                            {
                                checkingBalance -= item.Amount;
                            }
                        }
                        Console.WriteLine($"** Checking Balance: ${checkingBalance} **");

                        // Prompt for action
                        Console.WriteLine("What would you like to do?");
                        Console.WriteLine("(d) Deposit to checking,");
                        Console.WriteLine("(w) Withdraw from checking,");
                        Console.WriteLine("(s) Switch to Savings Account");
                        Console.WriteLine("(q) Quit the program");

                        // Switch for checking options
                        var checkingOptions = Console.ReadLine();
                        switch (checkingOptions)
                        {
                            case "d":
                                // Checking Deposit Process
                                Console.WriteLine("* Enter a deposit *\n");

                                Console.Write("Description: ");

                                var checkingDepositDescription = Console.ReadLine();

                                Console.Write("Amount: ");

                                // Get Checking Deposit Amount and force positive value
                                var checkingDepositAmountString = Console.ReadLine();
                                var checkingDepositRawAmount = double.Parse(checkingDepositAmountString);
                                var checkingDepositPositiveAmount = Math.Abs(checkingDepositRawAmount);

                                var checkingDepositType = "Deposit";
                                var checkingDepositAccount = "Checking";

                                var checkingAccountDeposit = new Transaction()
                                {
                                    Description = checkingDepositDescription,
                                    Amount = checkingDepositPositiveAmount,
                                    Type = checkingDepositType,
                                    Account = checkingDepositAccount,
                                };

                                transactions.Add(checkingAccountDeposit);

                                // Create a stream for writing information into a file
                                var fileWriter = new StreamWriter("transactions.csv");

                                // Create an object that can write CSV to the filewriter
                                var csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);

                                // Ask our csvWriter to write out our list of numbers
                                csvWriter.WriteRecords(transactions);

                                // Tell the file we are done
                                fileWriter.Close();



                                break;
                            case "w":
                                // Checking Withdrawal Process
                                Console.WriteLine("* Enter a withdrawal *\n");

                                Console.Write("Description: ");

                                var checkingWithdrawalDescription = Console.ReadLine();

                                Console.Write("Amount: ");

                                // Get Checking Withdrawal amount and force positive value
                                var checkingWithdrawalAmountString = Console.ReadLine();
                                var checkingWithdrawalRawAmount = double.Parse(checkingWithdrawalAmountString);
                                var checkingWithdrawalPositiveAmount = Math.Abs(checkingWithdrawalRawAmount);

                                // Check for and prevent overdraw
                                if (checkingWithdrawalPositiveAmount > checkingBalance)
                                {
                                    checkingWithdrawalDescription = "TRANSACTION CANCELLED: OVERDRAW";
                                    checkingWithdrawalPositiveAmount = 0.00;
                                }

                                var checkingWithdrawalType = "Withdrawal";
                                var checkingWithdrawalAccount = "Checking";

                                var checkingAccountWithdrawal = new Transaction()
                                {
                                    Description = checkingWithdrawalDescription,
                                    Amount = checkingWithdrawalPositiveAmount,
                                    Type = checkingWithdrawalType,
                                    Account = checkingWithdrawalAccount,
                                };

                                transactions.Add(checkingAccountWithdrawal);
                                break;
                            case "s":
                                screen = "savingsScreen";
                                break;
                            case "q":
                                quitApplication = "y";
                                break;
                            default:
                                break;
                        }
                        break;

                    case "savingsScreen":
                        // Create Savings View
                        Console.Clear();
                        Console.WriteLine("*** Savings ***\n");

                        // List savings transactions and calculate balance
                        var savingsAccount = new List<Transaction>();
                        var savingsBalance = 0.00;

                        foreach (var item in GetSavingsTransactions(transactions))
                        {
                            savingsAccount.Add(item);
                            Console.WriteLine(item.LineItem());

                            // Track Savings Account Balance
                            if (item.Type == "Deposit")
                            {
                                savingsBalance += item.Amount;
                            }
                            else
                            {
                                savingsBalance -= item.Amount;
                            }
                        }
                        Console.WriteLine($"** Savings Balance: ${savingsBalance} **");

                        // Prompt for action
                        Console.WriteLine("What would you like to do?");
                        Console.WriteLine("(d) Deposit to savings,");
                        Console.WriteLine("(w) Withdraw from savings,");
                        Console.WriteLine("(c) Switch to Checking Account");
                        Console.WriteLine("(q) Quit the program");

                        // Switch for checking options
                        var savingsOptions = Console.ReadLine();
                        switch (savingsOptions)
                        {
                            case "d":
                                // Savings Deposit Process
                                Console.WriteLine("* Enter a deposit *\n");

                                Console.Write("Description: ");

                                var savingsDepositDescription = Console.ReadLine();

                                Console.Write("Amount: ");

                                // Get Savings Deposit Amount and force positive value
                                var savingsDepositAmountString = Console.ReadLine();
                                var savingsDepositRawAmount = double.Parse(savingsDepositAmountString);
                                var savingsDepositPositiveAmount = Math.Abs(savingsDepositRawAmount);

                                var savingsDepositType = "Deposit";
                                var savingsDepositAccount = "Savings";

                                var savingsAccountDeposit = new Transaction()
                                {
                                    Description = savingsDepositDescription,
                                    Amount = savingsDepositPositiveAmount,
                                    Type = savingsDepositType,
                                    Account = savingsDepositAccount,
                                };

                                transactions.Add(savingsAccountDeposit);
                                break;
                            case "w":
                                // Savings Withdrawal Process
                                Console.WriteLine("* Enter a withdrawal *\n");

                                Console.Write("Description: ");

                                var savingsWithdrawalDescription = Console.ReadLine();

                                Console.Write("Amount: ");

                                // Get Savings Withdrawal Amount and force positive value
                                var savingsWithdrawalAmountString = Console.ReadLine();
                                var savingsWithdrawalRawAmount = double.Parse(savingsWithdrawalAmountString);
                                var savingsWithdrawalPositiveAmount = Math.Abs(savingsWithdrawalRawAmount);

                                // Check for and prevent overdraw
                                if (savingsWithdrawalPositiveAmount > savingsBalance)
                                {
                                    savingsWithdrawalDescription = "TRANSACTION CANCELLED: OVERDRAW";
                                    savingsWithdrawalPositiveAmount = 0.00;
                                }

                                var savingsWithdrawalType = "Withdrawal";
                                var savingsWithdrawalAccount = "Savings";

                                var savingsAccountWithdrawal = new Transaction()
                                {
                                    Description = savingsWithdrawalDescription,
                                    Amount = savingsWithdrawalPositiveAmount,
                                    Type = savingsWithdrawalType,
                                    Account = savingsWithdrawalAccount,
                                };

                                transactions.Add(savingsAccountWithdrawal);
                                break;
                            case "c":
                                screen = "checkingScreen";
                                break;
                            case "q":
                                quitApplication = "y";
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;

                }



                // Create Savings View
                // Console.Clear();
                // Console.WriteLine("*** Savings ***\n");

                // var savingsAccount = new List<Transaction>();

                // foreach (var item in GetSavingsTransactions(transactions))
                // {
                //     savingsAccount.Add(item);
                //     Console.WriteLine(item.LineItem());
                // }
                // REMOVE ME: while loop test
                // Console.Write("Do you want to quit?");
                // quitApplication = Console.ReadLine();
            }

            // Say Goodbye
            Console.WriteLine("*** Goodbye ***");

            // PEDAC
            // TODO: Load or Create File



        }
    }
}
