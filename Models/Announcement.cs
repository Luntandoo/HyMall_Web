using System;
using System.ComponentModel.DataAnnotations;

namespace HyMall_App.Models
{
    public class Announcement
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Display(Name = "Please note this message will be sent to the HyMall admin.")]
        public string Note { get; set; } = "Sent to admin";

        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        public string Status { get; set; } = "Pending"; // Admin Approval
    }
}
