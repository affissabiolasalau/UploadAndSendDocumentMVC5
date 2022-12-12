using MyDocuments.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyDocuments.Services
{
    public class DocumentServices
    {
        public static bool DeleteFile(string fileCompletePath)
        {
            try
            {
                // Check if file exists with its full path    
                if (File.Exists(Path.Combine(fileCompletePath)))
                {
                    // If file found, delete it    
                    File.Delete(Path.Combine(fileCompletePath));
                    return true;
                }
                return false;
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
                return false;
            }
        }

        
    }
}
