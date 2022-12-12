using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDocuments.Models
{
    public class ErrorLogsModel
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
