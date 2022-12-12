using System;
using System.ComponentModel.DataAnnotations;

namespace MyDocuments.Models
{
    public class TransactionsModel
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public string TransactionId { get; set; }
        public int Status { get; set; }
        public int FileCount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
