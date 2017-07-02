using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    class String
    {
        public static string ToTitle(string text)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            text = text.ToLower();
            // Return char and concat substring.
            return char.ToUpper(text[0]) + text.Substring(1);
        }

    }
}
