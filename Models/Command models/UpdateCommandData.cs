using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class UpdateCommandData
    {
        public int Id { get; set; }
        public string? Description { get; set; }

        public string? Category { get; set; }

        public int? Amount { get; set; }
    }
}
