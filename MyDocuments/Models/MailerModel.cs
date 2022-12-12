using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDocuments.Models
{
    public class MailerModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
