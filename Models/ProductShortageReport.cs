using System;
using System.ComponentModel.DataAnnotations;

namespace HyMall_App.Models
{
    public class ProductShortageReport
    {
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public int QuantityRemaining { get; set; }

        [Required]
        public string ShortageReason { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpectedRestockDate { get; set; }

        public DateTime ReportedOn { get; set; } = DateTime.Now;
    }
}
