using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class USPS20 : USPS
    {
        public const string SearchPattern = @"(\b([0-9]\s*){20,20}\b)";
        public const string VerifyPattern = @"^([0-9]{2,2})([0-9]{9,9})([0-9]{8,8})([0-9])$";

        public string ServiceCode { get; private set; }

        public string MailerId { get; private set; }

        public string PackageIdentifier { get; private set; }

        public char CheckDigit { get; private set; }

        public USPS20(string input)
            : base(input)
        {
            Decode();
        }

        private void Decode()
        {
            if (IsValid)
            {
                // http://www.usps.com/cpim/ftp/pubs/pub109.pdf (Publication 109. Extra Services Technical Guide, pg. 19)
                // http://www.usps.com/cpim/ftp/pubs/pub91.pdf (Publication 91. Confirmation Services Technical Guide pg. 38)

                ServiceCode = Input.Substring(0, 2);
                MailerId = Input.Substring(2, 9);
                PackageIdentifier = Input.Substring(11, 8);
                CheckDigit = Input[19];
            }
        }

        public string ServiceCodeDescription
        {
            get
            {
                switch (ServiceCode)
                {
                    case "71":
                        return "Certified Mail";

                    case "73":
                        return "Insured Mail";

                    case "77":
                        return "Registered Mail";

                    case "81":
                        return "Return Receipt for Merchandise";

                    default:
                        return $"USPS Unknown type {ServiceCode}";
                }
            }
        }

        public override bool IsValid => IsInputValid(Input);

        internal static bool IsInputValid(string input)
        {
            if (input.Length != 20)
                return false;

            if (!Regex.Match(input, VerifyPattern).Success)
                return false;

            return IsValidCheckSum(input);
        }
    }
}
