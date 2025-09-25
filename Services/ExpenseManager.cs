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

        public ExpenseManager()
        {
            _jsonDataHandler = new JsonDataHandler();
        }

        public void Add(string description, double amount)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Invalid argument: description of the Expense was not provided");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Invalid argument: amount must be greater than zero");
            }

            var expense = new Expense()
            {
                Description = description,
                Amount = amount,
                Id = GetUniqueId(),
                CreatedAt = DateTime.Now
            };

            _jsonDataHandler.ExpenseContainer.Expenses.Add(expense);
            _jsonDataHandler.SerializeJson();
        }

        public void Update(int id, string description, double amount)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Invalid argument: description of the Expense was not provided");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Invalid argument: amount must be greater than zero");
            }

            var expenses = _jsonDataHandler.ExpenseContainer.Expenses;
            var expense = expenses.FirstOrDefault(x => x.Id == id);

            if (expense == null)
            {
                throw new ArgumentException("No Expense found with the provided id");
            }

            expense.Description = description;
            expense.Amount = amount;

            _jsonDataHandler.SerializeJson();
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
            _jsonDataHandler.SerializeJson();


        }

        public List<Expense> GetAllExpenses()
        {
            return _jsonDataHandler.ExpenseContainer.Expenses;
        }

        public int GetUniqueId()
        {
            var expensesContainer = _jsonDataHandler.ExpenseContainer;

            var expenses = expensesContainer.Expenses;

            return expenses.Select(x => x.Id).DefaultIfEmpty(0).Max() + 1;
        }

        public int GetExpenseId()
        {
            var expensesContainer = _jsonDataHandler.ExpenseContainer;

            var expenses = expensesContainer.Expenses;

            return expenses[expenses.Count - 1].Id;
        }
    }
}
