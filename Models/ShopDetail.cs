using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HyMall_App.Models
{
    public class ShopDetail
    {
        public int Id { get; set; }

        [Required]
        public string ShopName { get; set; }

        public string Directions { get; set; } // Google Maps link

        [DataType(DataType.Time)]
        public string OpeningTime { get; set; }

        [DataType(DataType.Time)]
        public string ClosingTime { get; set; }

        [Display(Name = "Days Open (Mon–Sun)")]
        public string DaysOpen { get; set; } // store as comma-separated list

        [Phone]
        public string ContactNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
