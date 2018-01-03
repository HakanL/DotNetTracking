using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class FedExGround18 : FedEx
    {
        public const string SearchPattern = @"(\b([0-9]\s*){18,18}\b)";
        public const string VerifyPattern = @"^[0-9]{2,2}([0-9]{15,15})([0-9])$";

        public string ApplicationId { get; private set; }

        public string SerialContainer { get; private set; }

        public string ServiceCode { get; private set; }

        public string ShipperId { get; private set; }

        public string PackageIdentifier { get; private set; }

        public char CheckDigit { get; private set; }

        public FedExGround18(string input)
            : base(input)
        {
            Decode();
        }

        private void Decode()
        {
            if (IsInputValid(Input))
            {
                var enumerator = Input.GetEnumerator();

                ApplicationId = ReadFixedString(enumerator, 2);
                SerialContainer = ApplicationId.Substring(1);
                ServiceCode = ReadFixedString(enumerator, 1);
                ShipperId = ReadFixedString(enumerator, 7);
                PackageIdentifier = ReadFixedString(enumerator, 7);
                CheckDigit = ReadChar(enumerator);

                if (enumerator.MoveNext())
                    throw new IndexOutOfRangeException("Should not be any more characters");
            }
        }

        public override bool IsValid => IsInputValid(Input);

        internal static bool IsInputValid(string input)
        {
            if (input.Length != 18)
                return false;

            if (!Regex.Match(input, VerifyPattern).Success)
                return false;

            return IsValidCheckSum(input);
        }
    }
}
