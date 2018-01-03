using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class DHLExpress : DHL
    {
        public const string SearchPattern = @"(\b([0-9]\s*){10,10}\b)";
        public const string VerifyPattern = @"^([0-9]{9,9})([0-9])$";

        public DHLExpress(string input)
            : base(input)
        {
        }

        public override bool IsValid => IsInputValid(Input);

        internal static bool IsInputValid(string input)
        {
            if (input.Length != 10)
                return false;

            if (!Regex.Match(input, VerifyPattern).Success)
                return false;

            return IsValidCheckSum(input);
        }
    }
}
