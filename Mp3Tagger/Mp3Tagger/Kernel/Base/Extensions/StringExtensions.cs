using System;
using System.Linq;
using System.Text;

namespace Mp3Tagger.Kernel.Base.Extensions
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

        public static string AllWordsUpper(this string data)
        {
            string lookup = " \r\n\t";
            StringBuilder sb = new StringBuilder(data);

            if (sb.Length > 0 && char.IsLetter(sb[0]))
                sb[0] = char.ToUpper(sb[0]);

            for (int i = 1; i < sb.Length; i++)
            {
                char ch = sb[i];
                if (lookup.Contains(sb[i - 1]) && char.IsLetter(ch))
                    sb[i] = char.ToUpper(ch);
            }
            return sb.ToString();
        }

        public static string FirstWordUpper(this string data)
        {
            string lookup = " \r\n\t";
            StringBuilder sb = new StringBuilder(data);

            if (sb.Length > 0 && char.IsLetter(sb[0]))
                sb[0] = char.ToUpper(sb[0]);

            for (int i = 1; i < sb.Length; i++)
            {
                char ch = sb[i];
                if (lookup.Contains(sb[i - 1]) && char.IsLetter(ch))
                    sb[i] = char.ToLower(ch);
            }
            return sb.ToString();
        }
    }
}
