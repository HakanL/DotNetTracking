using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class FedExGround96 : FedEx
    {
        public const string SearchPattern = @"(\b9\s*6\s*([0-9]\s*){20,20}\b)";
        public const string VerifyPattern = @"^96[0-9]{5,5}([0-9]{14,14})([0-9])$";

        public string ApplicationId { get; private set; }

        public string SerialContainer { get; private set; }

        public string ServiceCode { get; private set; }

        public string ShipperId { get; private set; }

        public string PackageIdentifier { get; private set; }

        public char CheckDigit { get; private set; }

        public FedExGround96(string input)
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
                SerialContainer = ReadFixedString(enumerator, 2);
                ServiceCode = ReadFixedString(enumerator, 3);
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
            // 22 numbers
            // http://fedex.com/us/solutions/ppe/FedEx_Ground_Label_Layout_Specification.pdf
            // 96 - UCC/EAN Application Identifier
            //
            // [0-9]{2,2} - SCNC
            // [0-9]{3,3} - Class Of Service
            // [0-9]{7,7} - RPS Shipper ID (used in calculation)
            // [0-9]{7,7} - Package Number (used in calculation)
            // [0-9]      - Check Digit

            if (input.Length != 22)
                return false;

            var match = Regex.Match(input, VerifyPattern);
            if (!match.Success)
                return false;

            return IsValidCheckSum(match.Groups[1].Value + match.Groups[2].Value);
        }
    }
}
