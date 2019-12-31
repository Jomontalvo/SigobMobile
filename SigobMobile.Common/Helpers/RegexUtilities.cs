namespace SigobMobile.Common.Helpers
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Utility to evaluate regular expressions.
    /// </summary>
    public static class RegexUtilities
    {
        /// <summary>
        /// Validate email format
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                var validMail = new MailAddress(email);
                return true;
            }
            catch(FormatException){ return false; }
        }

        /// <summary>
        /// Get initials form Officee Name
        /// </summary>
        /// <param name="nameSeparated"></param>
        /// <returns></returns>
        public static string ExtractInitialsFromName(string nameSeparated)
        {
            string initials;
            try
            {
                string[] nameArray = nameSeparated.Split(",");
                nameArray = nameArray.Reverse().ToArray();
                string name = string.Join(" ", nameArray);
                name = name.Trim();
                // first remove all: punctuation, separator chars, control chars, and numbers (unicode style regexes)
                initials = Regex.Replace(name, @"[\p{P}\p{S}\p{C}\p{N}]+", "");

                // Replacing all possible whitespace/separator characters (unicode style), with a single, regular ascii space.
                initials = Regex.Replace(initials, @"\p{Z}+", " ");

                // Remove all Sr, Jr, I, II, III, IV, V, VI, VII, VIII, IX at the end of names
                initials = Regex.Replace(initials.Trim(), @"\s+(?:[JS]R|I{1,3}|I[VX]|VI{0,3})$", "", RegexOptions.IgnoreCase);

                // Extract up to 2 initials from the remaining cleaned name.
                initials = Regex.Replace(initials, @"^(\p{L})[^\s]*(?:\s+(?:\p{L}+\s+(?=\p{L}))?(?:(\p{L})\p{L}*)?)?$", "$1$2").Trim();

                if (initials.Length > 2)
                {
                    // Worst case scenario, everything failed, just grab the first two letters of what we have left.
                    initials = initials.Substring(0, 2);
                }
            }
            catch (Exception) { initials = nameSeparated.Substring(0, 1); }
            return initials.ToUpperInvariant();
        }
    }
}
