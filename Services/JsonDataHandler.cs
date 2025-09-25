using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public class JsonDataHandler
    {
        public ExpenseContainer ExpenseContainer { get; set; }

        public JsonDataHandler()
        {
            ExpenseContainer = DeserializeJson();
        }


        public ExpenseContainer DeserializeJson()
        {
            string dataFilePath = GetPath();
            var expensesJson = File.ReadAllText(dataFilePath);


            var expenses = JsonSerializer.Deserialize<ExpenseContainer>(expensesJson);
            return expenses ?? new ExpenseContainer();
        }

        public ExpenseContainer SerializeJson()
        {
            string dataFilePath = GetPath();
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string expensesJson = JsonSerializer.Serialize(ExpenseContainer, options);
            File.WriteAllText(dataFilePath, expensesJson);
            return ExpenseContainer;
        }

        private string GetPath()
        {
            var exeDir = AppContext.BaseDirectory;
            var dataDir = Path.Combine(exeDir, @"..\..\..", "Data");
            Directory.CreateDirectory(dataDir);
            var dataFilePath = Path.Combine(dataDir, "Expenses.json");

            if (!File.Exists(dataFilePath))
            {
                File.WriteAllText(dataFilePath, @"{""Expenses"": []}");
            }
            return dataFilePath;
        }
    }
}
