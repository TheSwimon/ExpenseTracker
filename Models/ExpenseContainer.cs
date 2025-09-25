using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class ExpenseContainer
    {
        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
