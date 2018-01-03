using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class FedExGround : FedEx
    {
        public const string SearchPattern = @"(\b([0-9]\s*){15,15}\b)";
        public const string VerifyPattern = @"^([0-9]{15,15})$";

        public FedExGround(string input)
            : base(input)
        {
        }

        public override bool IsValid => IsInputValid(Input);

        internal static bool IsInputValid(string input)
        {
            if (input.Length != 15)
                return false;

            if (!Regex.Match(input, VerifyPattern).Success)
                return false;

            return IsValidCheckSum(input);
        }
    }
}
