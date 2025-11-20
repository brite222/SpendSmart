using System.Collections.Generic;

namespace SpendSmart.Models
{
    public class User
    {
        public int Id { get; set; }

        // Required properties
        public required string Name { get; set; }
        public int Age { get; set; }
        public required string Address { get; set; }
        public required string Profession { get; set; }

        // One-to-many relationship
        public List<Expense> Expenses { get; set; } = new();
    }
}
