using System;
using System.ComponentModel.DataAnnotations;

namespace HyMall_App.Models
{
    public class Promotion
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required, Display(Name = "Product(s) Involved")]
        public string ProductsInvolved { get; set; }

        [Required]
        public string DiscountType { get; set; } // Percentage / Flat Amount

        [Required]
        public string Occasion { get; set; } // Holiday, Birthday, Black Friday

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public string Status { get; set; } = "Pending"; // for admin approval

        public bool IsExpired => EndDate < DateTime.Now;
    }
}
