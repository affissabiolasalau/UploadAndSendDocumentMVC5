using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDocuments.Utilities
{
    public class Greetings
    {
        public static string GreetingByTime()
        {
            DateTime now = DateTime.Now;
            int hour = now.Hour;
            string greeting;
            if (hour < 12)
            {
                greeting = "Good Morning";
            }
            else if (hour < 16)
            {
                greeting = "Good Afternoon";
            }
            else
            {
                greeting = "Good Evening";
            }
            return greeting;
        }
    }
}
