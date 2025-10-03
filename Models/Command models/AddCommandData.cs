using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class AddCommandData
    {
        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public int Amount { get; set; } 
    }
}
