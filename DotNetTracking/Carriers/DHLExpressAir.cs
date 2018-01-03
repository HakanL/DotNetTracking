using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class DHLExpressAir : DHL
    {
        public const string SearchPattern = @"(\b([0-9]\s*){11,11}\b)";
        public const string VerifyPattern = @"^([0-9]{10,10})([0-9])$";

        public DHLExpressAir(string input)
            : base(input)
        {
        }

        public override bool IsValid => IsInputValid(Input);

        internal static bool IsInputValid(string input)
        {
            if (input.Length != 11)
                return false;

            if (!Regex.Match(input, VerifyPattern).Success)
                return false;

            return IsValidCheckSum(input);
        }
    }
}
