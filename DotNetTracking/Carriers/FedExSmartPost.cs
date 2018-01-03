using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class FedExSmartPost : FedEx
    {
        public const string SearchPattern = @"(\b(?:9\s*2\s*)?([0-9]\s*){20}\b)";
        public const string VerifyPattern = @"^((?:92)?[0-9]{5}[0-9]{14})([0-9])$";

        public FedExSmartPost(string input)
            : base(input)
        {
        }

        public override bool IsValid => IsInputValid(Input);

        internal static bool IsInputValid(string input)
        {
            // http://stackoverflow.com/questions/15744704/how-to-calculate-a-fedex-smartpost-tracking-number-check-digit

            var match = Regex.Match(input, VerifyPattern);
            if (!match.Success)
                return false;

            if (input.StartsWith("92"))
                return IsValidCheckSum(input);
            else
                return IsValidCheckSum("92" + input);
        }
    }
}
