using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public class CommandHandler
    {
        private readonly ExpenseManager _expenseManager;
        private readonly Dictionary<string, Delegate> _handler;
        private readonly CommandParser _parser;
        public CommandHandler()
        {
            _expenseManager = new ExpenseManager();
            _parser = new CommandParser();

            _handler = new Dictionary<string, Delegate>()
            {
                {"add", HandleAdd },
                {"update", HandleUpdate },
                {"delete", HandleDelete },
                {"list",  HandleList},
                {"summary", HandleSummary },
                {@"\help", LogHelp }

            };
        }

        public bool ExecuteCommand(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(@"You need to provide arguments, write \help for instructions.");
                return false;
            }

            string operation = args[0];
            args = args.Skip(1).ToArray();



            if (_handler.TryGetValue(operation, out Delegate? method))
            {
                try
                {
                    if (method is Action<string[]> argHandler)
                    {
                        argHandler(args);
                    }
                    else if (method is Action noArgHandler)
                    {
                        noArgHandler();
                    }
                    return true;
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

            Console.WriteLine(@"Unrecognized command, write \help for instructions.");
            return false;
        }

        public void HandleAdd(string[] args)
        {
            var addCommandData = _parser.ParseAdd(args, 6);

            _expenseManager.Add(addCommandData.Description, addCommandData.Category, addCommandData.Amount);
            int id = _expenseManager.GetLastExpenseId();
            Console.WriteLine($"Expense added successfully: {id}");
        }


        public void HandleUpdate(string[] args)
        {
            var updateCommandData = _parser.ParseUpdate(args);

            _expenseManager.Update(updateCommandData.Id, updateCommandData.Description, updateCommandData.Category, updateCommandData.Amount);
            Console.WriteLine($"Expense updated successfully: {5}");

        }

        public void HandleDelete(string[] args)
        {
            var deleteCommandData = _parser.ParseDelete(args, 2);

            _expenseManager.Delete(deleteCommandData.Id);
            Console.WriteLine("Expense deleted successfully");
        }

        public void HandleList()
        {
            List<Expense> expenses = _expenseManager.GetAllExpenses();

            if (expenses.Count == 0)
            {
                Console.WriteLine("You have no expenses");
            }
            else
            {
                Console.WriteLine();
                foreach (var expense in expenses)
                {
                    Console.WriteLine(expense.Description);
                    Console.WriteLine("$" + expense.Amount.ToString("N2"));
                    Console.WriteLine(expense.CreatedAt);
                    Console.WriteLine(expense.Id);
                    Console.WriteLine();
                }
            }
        }

        public void HandleSummary()
        {

            List<Expense> expenses = _expenseManager.GetAllExpenses();
            if (expenses.Count == 0)
            {
                Console.WriteLine("You have no expenses");
            }
            else
            {
                double total = 0;

                foreach (var expense in expenses)
                {
                    total += expense.Amount;
                }
                Console.WriteLine($"Your total expenses are ${total.ToString("N2")}");
            }
        }

        private void LogHelp()
        {
            Console.WriteLine("\nAvailable operations: Add, Update, Delete, List details of all expenses, List summary of all expenses.\r\n\r\nExample commands:\r\nAdd entry - \"ExpenseTracker add --description \"pizza\" --category \"groceries\" --amount 20\" \r\nUpdate entry - \"ExpenseTracker update --id 9 --description \"khinkali\" --amount 25\"\r\nDelete entry - \"ExpenseTracker delete --id 8\"\r\nDetails of all expenses - \"list\"\r\nsummary of all expenses - \"summary\"\r\n\r\nGuide: \r\norder of the key-value pairs in Add command doesn't matter and all properties must be present.\r\norder of the key-value pairs in Update command doesn't matter, you have to provide at least one value to update the property, the first key-value pair must be the ID of the record.");
        }
    }
}
