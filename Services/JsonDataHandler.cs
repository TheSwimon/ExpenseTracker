using ExpenseTracker.Models;
using System.Text.Json;

namespace ExpenseTracker.Services
{
    public class JsonDataHandler
    {
        public ExpenseContainer ExpenseContainer { get; set; }

        public JsonDataHandler()
        {
            ExpenseContainer = GetData();
        }


        public ExpenseContainer GetData()
        {
            string dataFilePath = GetPath();
            var expensesJson = File.ReadAllText(dataFilePath);


            var expenses = JsonSerializer.Deserialize<ExpenseContainer>(expensesJson);
            return expenses ?? new ExpenseContainer();
        }

        public ExpenseContainer SaveChanges()
        {
            string dataFilePath = GetPath();
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string expensesJson = JsonSerializer.Serialize(ExpenseContainer, options);
            File.WriteAllText(dataFilePath, expensesJson);
            return ExpenseContainer;
        }


        // retrieves json's file path and creates the file, if it doesn't exist before returning the path.
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
