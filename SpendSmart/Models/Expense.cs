using System.ComponentModel.DataAnnotations;

namespace SpendSmart.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public decimal Value { get; set; }

        [Required]
        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        public string? Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public decimal Quantity { get; set; }

        [Required]
        public string? Size { get; set; } = string.Empty;

        [Required]
        public string SerialNumber { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Please select a user.")]
        public int UserId { get; set; }

        public User? User { get; set; }
    }
}
