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
        public CommandHandler()
        {
            _expenseManager = new ExpenseManager();
            _handler = new Dictionary<string, Delegate>()
            {
                {"add", HandleAdd },
                {"update", HandleUpdate },
                {"delete", HandleDelete },
                {"list",  HandleList},
                {"summary", HandleSummary }

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



            if (_handler.TryGetValue(operation, out Delegate method))
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
            if (args.Length == 4 && args[0].ToLower() == "--description" && !String.IsNullOrWhiteSpace(args[1]) && args[2].ToLower() == "--amount" && double.TryParse(args[3], out double amount))
            {
                _expenseManager.Add(args[1], amount);

                int id = _expenseManager.GetExpenseId();
                Console.WriteLine($"Expense added successfully: {id}");
            }
            else
            {
                Console.WriteLine(@"Unrecognized command, write \help for instructions.");
            }
        }


        public void HandleUpdate(string[] args)
        {
            if (args.Length == 6 && args[0].ToLower() == "--id" && int.TryParse(args[1], out int id) && args[2].ToLower() == "--description" && !string.IsNullOrEmpty(args[3]) && args[4].ToLower() == "--amount" && double.TryParse(args[5], out double amount))
            {
                _expenseManager.Update(id, args[3], amount);
                Console.WriteLine($"Expense updated successfully: {id}");
            }
            else
            {
                Console.WriteLine(@"Unrecognized command, write \help for instructions.");
            }
        }

        public void HandleDelete(string[] args)
        {
            if (args.Length == 1 && int.TryParse(args[0], out int id))
            {
                _expenseManager.Delete(id);
                Console.WriteLine("Expense deleted successfully");
            }
            else
            {
                Console.WriteLine(@"Unrecognized command, write \help for instructions.");
            }
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
    }
}
