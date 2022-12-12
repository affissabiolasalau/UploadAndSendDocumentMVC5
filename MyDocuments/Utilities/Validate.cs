using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDocuments.Utilities
{
    public class Validate
    {
        public static bool Email(string email)
        {           
            try
            {
                if (email.Trim().EndsWith("."))
                {
                    return false;
                }
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
