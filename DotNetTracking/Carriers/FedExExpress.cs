using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class FedExExpress : FedEx
    {
        public const string SearchPattern = @"(\b([0-9]\s*){12,12}\b)";
        public const string VerifyPattern = @"^([0-9]{11,11})([0-9])$";

        public FedExExpress(string input)
            : base(input)
        {
        }

        public override bool IsValid => IsInputValid(Input);

        internal static bool IsInputValid(string input)
        {
            if (input.Length != 12)
                return false;

            var match = Regex.Match(input, VerifyPattern);

            if (!match.Success)
                return false;

            var digits = match.Groups[1].Value.ToCharArray().Select(c => int.Parse(c.ToString()));
            var data = digits.Zip(new int[] { 3, 1, 7, 3, 1, 7, 3, 1, 7, 3, 1 }, (a, b) => a * b);
            int total = data.Sum();

            return (total % 11 % 10) == int.Parse(match.Groups[2].Value);
        }
    }
}
