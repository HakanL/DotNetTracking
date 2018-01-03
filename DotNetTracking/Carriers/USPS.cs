using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public abstract class USPS : TrackingType
    {
        public USPS(string input)
            : base(input)
        {
        }

        public override string Carrier => "USPS";

        public static TrackingType Match(string input)
        {
            string fixedOutput;

            if (USPS91.IsInputValid(input, out fixedOutput))
                return new USPS91(fixedOutput);

            if (USPS20.IsInputValid(input))
                return new USPS20(input);

            if (USPS13.IsInputValid(input))
                return new USPS13(input);

            return null;
        }

        protected static bool IsValidCheckSum(string input)
        {
            var (sequence, checkDigit) = GetDigits(input);

            int total = sequence.Reverse().Select((x, i) => (i % 2 == 0) ? x * 3 : x).Sum();

            int check = total % 10;
            if (check > 0)
                check = (10 - check);

            return check == checkDigit;
        }
    }
}
