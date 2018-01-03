using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public abstract class DHL : TrackingType
    {
        public DHL(string input)
            : base(input)
        {
        }

        public override string Carrier => "DHL";

        protected static bool IsValidCheckSum(string input)
        {
            // Standard mod 7 check
            var (sequence, checkDigit) = GetDigits(input);

            long value = long.Parse(string.Join("", sequence));

            return (value % 7) == checkDigit;
        }

        public static TrackingType Match(string input)
        {
            if (DHLExpressAir.IsInputValid(input))
                return new DHLExpressAir(input);

            if (DHLExpress.IsInputValid(input))
                return new DHLExpress(input);

            return null;
        }
    }
}
