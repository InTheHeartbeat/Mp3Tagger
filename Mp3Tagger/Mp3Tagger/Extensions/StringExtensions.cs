using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Tagger.Extensions
{
    public static class StringExtensions
    {
        public static string AddSpaceBetwenUpperChars(this string data)
        {
            string result = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                if (Char.IsUpper(data, i))
                    result += " " + data[i];
                else
                    result += data[i];
            }
            return result;
        }
    }
}
