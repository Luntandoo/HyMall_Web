using System;
using System.ComponentModel.DataAnnotations;

namespace HyMall_App.Models
{
    public class CustomerFeedback
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } // optional if anonymous

        [Required]
        public string ProductOrCategory { get; set; }

        [Required]
        public string FeedbackType { get; set; } // Complaint, Suggestion, Compliment

        [Required, DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Required]
        public string Severity { get; set; } // Low, Medium, High

        [Required]
        public string ResponseStatus { get; set; } // Pending, In Progress, Resolved

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
