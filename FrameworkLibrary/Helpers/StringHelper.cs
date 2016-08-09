using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace FrameworkLibrary
{
    public class StringHelper
    {
        public static string StripHtmlTags(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        public static string StripTags(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        public static T JsonToObject<T>(string to)
        {
            if (to.StartsWith("{") || to.StartsWith("["))
                return JsonConvert.DeserializeObject<T>(to);

            return default(T);
        }

        public static string ObjectToJson(object to, long depth=1)
        {
            return JsonConvert.SerializeObject(to);            
        }

        public static bool ContainsWord(string inputString, string term)
        {
            return Regex.IsMatch(inputString, @"\b" + Regex.Escape(term) + @"\b", RegexOptions.IgnoreCase);
        }

        public static string StripExtraLines(string text)
        {
            text = Regex.Replace(text, @"^\s*$\n", string.Empty, RegexOptions.Multiline);
            return text;
        }

        public static string StripExtraSpaces(string text)
        {
            text = Regex.Replace(text, @"\s+", " ");
            return text;
        }

        public static string StripExtraSpacesBetweenMarkup(string text)
        {
            Regex REGEX_FOR_TAGS = new Regex(@">s+<", RegexOptions.Compiled);
            Regex REGEX_FOR_BREAKS = new Regex(@">[\s]*<", RegexOptions.Compiled);

            text = REGEX_FOR_TAGS.Replace(text, "> <");
            text = REGEX_FOR_BREAKS.Replace(text, "><");

            return text;
        }

        public static string Replace(string inString, string oldStringToReplace, string newStringToReplaceWith)
        {
            var regex = new Regex(Regex.Escape(oldStringToReplace) + "(?![^<]*</a>)", RegexOptions.IgnoreCase);
            return regex.Replace(inString, newStringToReplaceWith);
        }

        public static string FormatOnlyDate(DateTime dateTime)
        {
            return dateTime.ToString("MMMM dd, yyyy");
        }

        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("MMM dd, yyyy h:mmtt");
        }

        public static string FormatOnlyTime(DateTime dateTime)
        {
            return dateTime.ToString("h:mmtt");
        }

        public static string StripTag(string text, string tagName)
        {
            return Regex.Replace(text, @"<.*" + tagName + ".*>", string.Empty);
        }

        public static string RemoveSpecialChars(string text)
        {
            return Regex.Replace(text, "[^\\w\\.@^\\s]", "");
        }

        public static string JavascriptStringEncode(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                return message;
            }

            StringBuilder builder = new StringBuilder();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(message, builder);
            return builder.ToString(1, builder.Length - 2); // remove first + last quote
        }

        /*public static string IgnoreCaseReplace(string oldStr, string newStr)
        {
            return Regex.Replace(oldStr, "*", newStr, RegexOptions.IgnoreCase);
        }*/

        /// <summary>
        /// This method will strip any white space, conver to lower case, replace spaces with "-" and remove any duplicate or trailing dashes
        /// </summary>
        public static string CreateSlug(string Value)
        {
            string Slug = Value.Trim().ToLower();
            //Remove all extra whitespace and convert string to lowercase.
            Slug = Regex.Replace(Slug, @"[^\w\s]", "").Trim();
            Slug = Regex.Replace(Slug, "[^a-z0-9 -_]", "").Replace(" ", "-");
            //Remove all special characters and convert spaces to dashes.
            // Remove any occurances of double dashes.
            while (!(!Slug.Contains("--")))
            {
                Slug = Slug.Replace("--", "-");
            }
            return Slug;
        }

        /// <summary>
        /// Name: Compute the similarity between two words using Levenshtein distance algorithm
        /// Description:In order to check similarties between two strings many methods have been implemented.
        /// Levenshtein method is famous, and invented by Russian scientist called Vladimir Levenshtein,in 1965.
        /// By: Salem Al Shekaili
        ///
        /// Inputs:'For example If s = "sale" and t = "sale", then Levenshtein_distance(s,t) = 0, because no transformations are needed.
        /// The strings are already identical.
        /// 'If s is "sale" and t is "nale", Levenshtein_distance(s,t) = 1, because one substitution (change "s" to "n") is sufficient to transform s into t.
        /// 'read more information here http://www.merriampark.com/ld.htm
        /// 'and see http://www.let.rug.nl/~kleiweg/lev/ for demo
        ///
        ///This code is copyrighted and has// limited warranties.
        ///Please see http://www.Planet-Source-Code.com/vb/scripts/ShowCode.asp?txtCodeId=5858&lngWId=10//for details.
        public static int CalculateDistance(string s, string t)
        {
            int i = 0; // iterates through s
            int j = 0;// iterates through t
            string s_i = null; // ith character of s
            string t_j = null;  // jth character of t
            int cost = 0;// cost

            // Step 1
            int n = s.Length; //length of s
            int m = t.Length; //length of t

            if (n == 0)
            {
                return m;
            }
            if (m == 0)
            {
                return n;
            }
            int[,] d = new int[n + 1, m + 1];
            // Step 2
            for (i = 0; i <= n; i++)
            {
                d[i, 0] = i;
            }
            for (j = 0; j <= m; j++)
            {
                d[0, j] = j;
            }
            // Step 3
            for (i = 1; i <= n; i++)
            {
                s_i = s.Substring(i - 1, 1);
                // Step 4
                for (j = 1; j <= m; j++)
                {
                    t_j = t.Substring(j - 1, 1);
                    // Step 5
                    if (s_i == t_j)
                    {
                        cost = 0;
                    }
                    else
                    {
                        cost = 1;
                    }
                    // Step 6
                    d[i, j] = System.Math.Min(System.Math.Min((d[(i - 1), j] + 1), (d[i, (j - 1)] + 1)), (d[(i - 1), (j - 1)] + cost));
                }
            }
            // Step 7
            return d[n, m];
        }

        public static string[] SplitByString(string testString, string split)
        {
            int offset = 0;
            int index = 0;
            int[] offsets = new int[testString.Length + 1];

            while (index < testString.Length)
            {
                int indexOf = testString.IndexOf(split, index);
                if (indexOf != -1)
                {
                    offsets[offset++] = indexOf;
                    index = (indexOf + split.Length);
                }
                else
                {
                    index = testString.Length;
                }
            }

            string[] final = new string[offset + 1];
            if (offset == 0)
            {
                final[0] = testString;
            }
            else
            {
                offset--;
                final[0] = testString.Substring(0, offsets[0]);
                for (int i = 0; i < offset; i++)
                {
                    final[i + 1] = testString.Substring(offsets[i] + split.Length, offsets[i + 1] - offsets[i] - split.Length);
                }
                final[offset + 1] = testString.Substring(offsets[offset] + split.Length);
            }
            return final;
        }

        public static string Encrypt(string password)
        {
            var data = Encoding.ASCII.GetBytes(password);

            var sha1 = new SHA1CryptoServiceProvider();
            var sha1data = sha1.ComputeHash(data);

            return Encoding.ASCII.GetString(sha1data);
        }
    }
}