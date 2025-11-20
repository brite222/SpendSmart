using System.ComponentModel.DataAnnotations;

namespace SpendSmart.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Value { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Color { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        [Required]
        public string? Size { get; set; } = string.Empty;

        [Required]
        public string SerialNumber { get; set; } = string.Empty;

        // NEW: Foreign Key
        public int UserId { get; set; }

        // NEW: Navigation Property
        public User? User { get; set; }
    }
}
