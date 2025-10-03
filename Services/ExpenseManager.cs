
using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public class ExpenseManager
    {
        private readonly JsonDataHandler _jsonDataHandler;
        private readonly List<string> _categories;

        public ExpenseManager()
        {
            _jsonDataHandler = new JsonDataHandler();
            _categories = ["Housing", "Transportation", "Groceries", "Health", "Insurance", "Utilities", "Debt", "Education", "Savings", "Entertainment", "Clothing", "Taxes", "Miscellaneous"];
        }

        public void Add(string description, string category, int amount)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Invalid argument: description of the Expense was not provided");
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Invalid argument: category of the expense was not provided");
            }

            if (!IsCategoryValid(category))
            {
                throw new ArgumentException("Invalid argument: category doesn't exist");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Invalid argument: amount must be greater than zero");
            }



            var expense = new Expense()
            {
                Description = description,
                Category = category,
                Amount = amount,
                Id = GetUniqueId(),
                CreatedAt = DateTime.Now
            };

            _jsonDataHandler.ExpenseContainer.Expenses.Add(expense);
            _jsonDataHandler.SaveChanges();
        }

        public void Update(int id, string? description, string? category, int? amount)
        {
            if (amount != null && amount <= 0)
            {
                throw new ArgumentException("Invalid argument: amount should be greater than zero");
            }

            if (string.IsNullOrEmpty(description) && string.IsNullOrEmpty(category) && amount == null)
            {
                throw new ArgumentException(@"Invalid command: write \help for instructions");
            }

            if (!string.IsNullOrEmpty(category) && !IsCategoryValid(category))
            {
                throw new ArgumentException("Invalid argument: category doesn't exist");
            }

            var expenses = _jsonDataHandler.ExpenseContainer.Expenses;
            var expense = expenses.FirstOrDefault(x => x.Id == id);

            if (expense == null)
            {
                throw new ArgumentException("No Expense found with the provided id");
            }

            expense.Description = description ?? expense.Description;
            expense.Category = category ?? expense.Category;
            expense.Amount = amount ?? expense.Amount;

            _jsonDataHandler.SaveChanges();
        }

        public void Delete(int id)
        {
            var expenses = _jsonDataHandler.ExpenseContainer.Expenses;
            var entry = expenses.FirstOrDefault(x => x.Id == id);

            if (entry == null)
            {
                throw new ArgumentException("No Expense found with the provided id");
            }

            expenses.Remove(entry);
            _jsonDataHandler.SaveChanges();


        }

        public List<Expense> GetAllExpenses()
        {
            return _jsonDataHandler.ExpenseContainer.Expenses;
        }

        public int GetLastExpenseId()
        {
            var expensesContainer = _jsonDataHandler.ExpenseContainer;

            var expenses = expensesContainer.Expenses;

            return expenses[expenses.Count - 1].Id;
        }


        //helper method
        private int GetUniqueId()
        {
            var expensesContainer = _jsonDataHandler.ExpenseContainer;

            var expenses = expensesContainer.Expenses;

            return expenses.Select(x => x.Id).DefaultIfEmpty(0).Max() + 1;
        }

        private bool IsCategoryValid(string category)
        {
            var isValid = _categories.Any(standardCategory => standardCategory.Equals(category, StringComparison.OrdinalIgnoreCase));
            return isValid;
        }
    }
}
