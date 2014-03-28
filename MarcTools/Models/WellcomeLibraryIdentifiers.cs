using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MarcTools.Models
{
    /// <summary>
    /// COntains utility methods for identifying and normalising b numbers
    /// and other identifiers in future.
    /// Ultimately an identifier could be process and cause DIFFERENT dip providers to be invoked.
    /// </summary>
    public static class WellcomeLibraryIdentifiers
    {
        private static readonly Regex VariantsOf7DigitBNumber = new Regex(@".?b?([0-9]{7})(\w?)");

        /// <summary>
        /// /player/b12345678
        /// /player/b1234567
        /// /player/b1234567a
        /// /player/.b12345678
        /// /player/.b1234567
        /// /player/.b1234567a
        /// 
        /// These variants (with and without checksum) are all valid, but we should redirect to the first one to normalise
        /// references to b Numbers, so we don't end up with lots of variations.
        /// 
        /// From the Encore docs (Page # 105781):
        /// 
        /// Record numbers are seven or eight digits long, including a modulus 11 check digit in the last position. 
        /// For example, the number .b10243641 is interpreted as follows:
        ///         .b10243641
        ///         |||      |
        ///         |||      `--- Check digit
        ///         ||`--- Record number
        ///         |`--- Record type (bibliographic)
        ///         `--- Record number prefix
        /// 
        /// When searching for a record number, if you don't know the check digit, you can substitute 
        /// the character a for the check digit. For example, you could enter the record 
        /// number above as.b1024364a.
        /// </summary>
        public static string GetNormalisedBNumber(string bNumber, bool errorOnInvalidChecksum)
        {
            string normalisedBNumber = null;
            var m = VariantsOf7DigitBNumber.Match(bNumber);
            if (m.Success && m.Groups[1].Success)
            {
                string recordNumber = m.Groups[1].Value;
                char expectedCheckDigit = GetExpectedBNumberCheckDigit(recordNumber);
                normalisedBNumber = "b" + recordNumber + expectedCheckDigit;
                if (m.Groups[2].Success && !String.IsNullOrWhiteSpace(m.Groups[2].Value))
                {
                    char suppliedCheckDigit = m.Groups[2].Value[0];
                    // we could throw an exception if a checksum digit was supplied that DOES NOT MATCH
                    if (errorOnInvalidChecksum && suppliedCheckDigit != 'a' && suppliedCheckDigit != expectedCheckDigit)
                    {
                        throw new ArgumentException(
                            "Supplied check digit '" + suppliedCheckDigit
                            + "' does not match expected check digit '" + expectedCheckDigit + "'",
                            "bNumber");
                    }
                }
            }
            return normalisedBNumber;
        }

        /// <summary>
        /// Check Digits
        /// Check digits may be any one of 11 possible digits (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, or x).
        /// The check digit is calculated as follows:
        /// 1.	Multiply the rightmost digit of the record number by 2, the next digit to the left by 3, 
        /// the next by 4, etc., and total the products. For example:
        ///         1 0 2 4 3 6 4
        ///         | | | | | | |
        ///         | | | | | | 4 * 2 =  8
        ///         | | | | | 6 * 3   = 18
        ///         | | | | 3 * 4     = 12
        ///         | | | 4 * 5       = 20
        ///         | | 2 * 6         = 12
        ///         | 0 * 7           =  0
        ///         1 * 8             =  8
        ///                             78
        /// 2.	Divide the total by 11 and retain the remainder (for example, 78 / 11 is 7, with a 
        /// remainder of 1). The remainder after the division is the check digit. If the remainder 
        /// is 10, the letter x is used as the check digit.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static char GetExpectedBNumberCheckDigit(string s)
        {
            if (s.Length != 7)
            {
                throw new ArgumentException("string must be 7 characters", "s");
            }
            int total = 0;
            int multiplier = 2;
            for (int i = 6; i >= 0; i--)
            {
                int digit = int.Parse(s[i].ToString(CultureInfo.InvariantCulture));
                total += digit * multiplier;
                multiplier++;
            }
            int remainder = total % 11;
            if (remainder == 10)
            {
                return 'x';
            }
            return remainder.ToString(CultureInfo.InvariantCulture)[0];
        }

        public static string ToBNumber(this int shortbNumber)
        {
            var asString = shortbNumber.ToString(CultureInfo.InvariantCulture);
            return String.Format("b{0}{1}", shortbNumber, GetExpectedBNumberCheckDigit(asString));
        }

        public static string ToPatronId(this int shortPatronId)
        {
            var asString = shortPatronId.ToString(CultureInfo.InvariantCulture);
            return String.Format(".p{0}{1}", shortPatronId, GetExpectedBNumberCheckDigit(asString));
        }

        public static bool IsBNumber(this string s)
        {
            if (String.IsNullOrWhiteSpace(s))
                return false;

            // TODO: should this allow 
            return Regex.IsMatch(s, "\\Ab[0-9x]{7,9}\\z", RegexOptions.IgnoreCase);
        }

        public static int ToShortBNumber(this string bNumber)
        {
            return GetShortBNumber(bNumber);
        }

        public static int GetShortBNumber(string bNumber)
        {
            if (!bNumber.IsBNumber())
                return 0;

            int b;
            if (bNumber.StartsWith("b"))
            {
                bNumber = bNumber.Substring(1);
            }
            Int32.TryParse(bNumber.Substring(0, 7), out b);
            return b;
        }
    }
}