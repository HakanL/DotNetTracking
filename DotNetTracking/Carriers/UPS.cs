using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class UPS : TrackingType
    {
        public const string SearchPattern = @"(\b1\s*Z\s*(\w\s*){16,16}\b)";
        public const string VerifyPattern = @"^1Z(\w{15,15})(\w)$";

        public UPS(string input)
            : base(input)
        {
            Decode();
        }

        private void Decode()
        {
            if (IsInputValid(Input))
            {
                var enumerator = Input.GetEnumerator();

                // 1Z
                ReadFixedString(enumerator, 2);

                ShipperAccount = ReadFixedString(enumerator, 6);
                ServiceType = ReadFixedString(enumerator, 2);
                PackageIdentifier = ReadFixedString(enumerator, 7);
                CheckDigit = ReadChar(enumerator);

                if (enumerator.MoveNext())
                    throw new IndexOutOfRangeException("Should not be any more characters");
            }
        }

        public override string Carrier => "UPS";

        public string ShipperAccount { get; private set; }

        public string ServiceType { get; private set; }

        public string PackageIdentifier { get; private set; }

        public char CheckDigit { get; private set; }

        public string ServiceTypeDescription
        {
            get
            {
                switch (ServiceType)
                {
                    case "01":
                        return "UPS United States Next Day Air (Red)";

                    case "02":
                        return "UPS United States Second Day Air (Blue)";

                    case "03":
                        return "UPS United States Ground";

                    case "12":
                        return "UPS United States Third Day Select";

                    case "13":
                        return "UPS United States Next Day Air Saver (Red Saver)";

                    case "15":
                        return "UPS United States Next Day Air Early A.M.";

                    case "22":
                        return "UPS United States Ground - Returns Plus - Three Pickup Attempts";

                    case "32":
                        return "UPS United States Next Day Air Early A.M. - COD";

                    case "33":
                        return "UPS United States Next Day Air Early A.M. - Saturday Delivery, COD";

                    case "41":
                        return "UPS United States Next Day Air Early A.M. - Saturday Delivery";

                    case "42":
                        return "UPS United States Ground - Signature Required";

                    case "44":
                        return "UPS United States Next Day Air - Saturday Delivery";

                    case "66":
                        return "UPS United States Worldwide Express";

                    case "72":
                        return "UPS United States Ground - Collect on Delivery";

                    case "78":
                        return "UPS United States Ground - Returns Plus - One Pickup Attempt";

                    case "90":
                        return "UPS United States Ground - Returns - UPS Prints and Mails Label";

                    case "A0":
                        return "UPS United States Next Day Air Early A.M. - Adult Signature Required";

                    case "A1":
                        return "UPS United States Next Day Air Early A.M. - Saturday Delivery, Adult Signature Required";

                    case "A2":
                        return "UPS United States Next Day Air - Adult Signature Required";

                    case "A8":
                        return "UPS United States Ground - Adult Signature Required";

                    case "A9":
                        return "UPS United States Next Day Air Early A.M. - Adult Signature Required, COD";

                    case "AA":
                        return "UPS United States Next Day Air Early A.M. - Saturday Delivery, Adult Signature Required, COD";

                    default:
                        return $"UPS Unknown type {ServiceType}";
                }
            }
        }

        public override bool IsValid => IsInputValid(Input);

        public override string TrackingURL => $"https://wwwapps.ups.com/WebTracking/track?track=yes&trackNums={Input}";

        private static bool IsInputValid(string input)
        {
            if (input.Length != 18)
                return false;

            if (!Regex.Match(input, VerifyPattern).Success)
                return false;

            char checkDigit;
            var sequence = GetSequence(input, out checkDigit);

            int total = sequence
                .Skip(2)
                .Select(c => (c >= '0' && c <= '9') ? c - 48 : (((int)c - 3) % 10))
                .Select((x, i) => (i % 2 != 0) ? x * 2 : x)
                .Sum();

            int check = total % 10;
            if (check > 0)
                check = (10 - check);

            return check == int.Parse(checkDigit.ToString());
        }

        public static TrackingType Match(string input)
        {
            if (IsInputValid(input))
                return new UPS(input);

            return null;
        }
    }
}
