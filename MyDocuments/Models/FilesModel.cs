using System;
using System.ComponentModel.DataAnnotations;

namespace MyDocuments.Models
{
    public class FilesModel
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public string TransactionReference { get; set; }
        public int Status { get; set; }
        public string FileTitle { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
       
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
