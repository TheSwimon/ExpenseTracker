using ExpenseTracker.Models;
using ExpenseTracker.Services;
using System.Reflection.Metadata;
using System.Text.Json;




//JsonDataHandler handler = new JsonDataHandler();
//List<Expense> expenses = handler.ExpenseContainer.Expenses;
//ExpenseManager manager = new ExpenseManager();


CommandHandler commandHandler = new CommandHandler();
commandHandler.ExecuteCommand(args);


