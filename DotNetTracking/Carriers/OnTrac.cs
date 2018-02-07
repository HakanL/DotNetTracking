using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class OnTrac : TrackingType
    {
        public const string SearchPattern = @"(\b(C\s*)([0-9]\s*){14,14}\b)";
        public const string VerifyPattern = @"^(C[0-9]{13,13})([0-9])$";

        public OnTrac(string input)
            : base(input)
        {
        }

        public override string Carrier => "OnTrac";

        public override string TrackingURL => $"http://www.ontrac.com/trackingres.asp?tracking_number={Input}";

        public override bool IsValid => IsInputValid(Input);

        private static bool IsInputValid(string input)
        {
            if (input.Length != 15)
                return false;

            if (!Regex.Match(input, VerifyPattern).Success)
                return false;

            var (sequence, checkDigit) = GetSequence(input);

            // Same check as for UPS

            int total = sequence
                .Select(c => (c >= '0' && c <= '9') ? c - 48 : (((int)c - 3) % 10))
                .Select((x, i) => (i % 2 != 0) ? x * 2 : x)
                .Sum();

            int check = total % 10;
            if (check > 0)
                check = (10 - check);

            return check == int.Parse(checkDigit.ToString());
        }

        public static TrackingType Match(string input)
        {
            if (IsInputValid(input))
                return new OnTrac(input);

            return null;
        }
    }
}
