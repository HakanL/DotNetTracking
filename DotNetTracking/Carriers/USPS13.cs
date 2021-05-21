using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class USPS13 : USPS
    {
        public const string SearchPattern = @"(\b([A-Z]\s*){2,2}([0-9]\s*){9,9}([A-Z]\s*){2,2}\b)";
        public const string VerifyPattern = @"^([A-Z]{2,2})([0-9]{9,9})([A-Z]{2,2})$";

        public USPS13(string input)
            : base(input)
        {
        }

        public override bool IsValid => IsInputValid(Input);

        internal static bool IsInputValid(string input)
        {
            if (input.Length != 13)
                return false;

            if (!Regex.Match(input, VerifyPattern).Success)
                return false;

            var numericSequence = Regex.Match(input, @"[0-9]+");
            if (!numericSequence.Success)
                return false;

            int checkDigit;
            var sequence = GetDigits(numericSequence.Value, out checkDigit);

            var data = sequence.Zip(new int[] { 8, 6, 4, 2, 3, 5, 9, 7 }, (a, b) => a * b);
            int total = data.Sum();

            int remainder = total % 11;
            int check;
            switch (remainder)
            {
                case 1:
                    check = 0;
                    break;

                case 0:
                    check = 5;
                    break;

                default:
                    check = 11 - remainder;
                    break;
            }

            return check == checkDigit;
        }
    }
}
