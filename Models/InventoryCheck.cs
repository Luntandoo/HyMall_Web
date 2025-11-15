
using System;
using System.ComponentModel.DataAnnotations;

namespace HyMall_App.Models
{
    public class InventoryCheck
    {
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public int CurrentQuantity { get; set; }

        public int? RestockedQuantity { get; set; }

        [Display(Name = "Notes (e.g., last restock date and details)")]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
