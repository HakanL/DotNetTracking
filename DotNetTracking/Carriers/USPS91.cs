using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class USPS91 : USPS
    {
        public static readonly string[] SearchPatterns = new string[] {
            @"(\b(?:420\s*\d{5})?9\s*[1-5]\s*(?:(?:(?:[0-9]\s*){20}\b)|(?:(?:[0-9]\s*){24}\b)))",
            @"(\b([0-9]\s*){20}\b)"
        };
        public const string VerifyPattern = @"^(9[1-5](?:[0-9]{19}|[0-9]{23}))([0-9])$";

        public string ApplicationId { get; private set; }

        public string ServiceCode { get; private set; }

        public string SourceId { get; private set; }

        public string ChannelId { get; private set; }

        public string DeviceId { get; private set; }

        public string JulianDate { get; private set; }

        public string MailerId { get; private set; }

        public string PackageIdentifier { get; private set; }

        public char CheckDigit { get; private set; }

        public USPS91(string input)
            : base(input)
        {
            Decode();
        }

        private void Decode()
        {
            // https://ribbs.usps.gov/intelligentmail_package/documents/tech_guides/BarcodePackageIMSpec.pdf

            if (IsValid)
            {
                // Sometimes these numbers will appear without the leading 91, 93, or 94, though, so we need to account for that case

                string baseTrackingNumber = GetBaseTrackingNumber(Input);

                var enumerator = baseTrackingNumber.GetEnumerator();

                if ((Input.Length == 34 || Input.Length == 22) && baseTrackingNumber.Length == 22)
                {
                    ApplicationId = ReadFixedString(enumerator, 2);

                    switch (ApplicationId)
                    {
                        case "91":
                            ServiceCode = ReadFixedString(enumerator, 2);
                            MailerId = ReadFixedString(enumerator, 8);
                            PackageIdentifier = ReadFixedString(enumerator, 9);
                            break;

                        case "93":
                            // Format C05, C08
                            ServiceCode = ReadFixedString(enumerator, 3);
                            MailerId = ReadFixedString(enumerator, 6);
                            PackageIdentifier = ReadFixedString(enumerator, 10);
                            break;

                        case "94":
                            // Format N02, N03
                            ServiceCode = ReadFixedString(enumerator, 3);
                            SourceId = ReadFixedString(enumerator, 2);
                            MailerId = ReadFixedString(enumerator, 6);
                            PackageIdentifier = ReadFixedString(enumerator, 8);
                            break;

                        case "95":
                            // Format A01, P02
                            ServiceCode = ReadFixedString(enumerator, 3);
                            ChannelId = ReadFixedString(enumerator, 1);
                            DeviceId = ReadFixedString(enumerator, 6);
                            JulianDate = ReadFixedString(enumerator, 4);
                            PackageIdentifier = ReadFixedString(enumerator, 5);
                            break;

                        default:
                            // Format C01, C04
                            ServiceCode = ReadFixedString(enumerator, 3);
                            MailerId = ReadFixedString(enumerator, 9);
                            PackageIdentifier = ReadFixedString(enumerator, 7);
                            break;
                    }

                    CheckDigit = ReadChar(enumerator);

                    if (enumerator.MoveNext())
                        throw new IndexOutOfRangeException("Should not be any more characters");
                    return;
                }

                if ((Input.Length == 34 || Input.Length == 26) && baseTrackingNumber.Length == 26)
                {
                    ApplicationId = ReadFixedString(enumerator, 2);

                    switch (ApplicationId)
                    {
                        case "93":
                            // Format C06, C09
                            ServiceCode = ReadFixedString(enumerator, 3);
                            MailerId = ReadFixedString(enumerator, 6);
                            PackageIdentifier = ReadFixedString(enumerator, 14);
                            break;

                        default:
                            // Format C02, C10
                            ServiceCode = ReadFixedString(enumerator, 3);
                            MailerId = ReadFixedString(enumerator, 9);
                            PackageIdentifier = ReadFixedString(enumerator, 11);
                            break;
                    }

                    CheckDigit = ReadChar(enumerator);

                    if (enumerator.MoveNext())
                        throw new IndexOutOfRangeException("Should not be any more characters");
                    return;
                }

                if (Input.Length == 30 && baseTrackingNumber.Length == 22)
                {
                    ApplicationId = ReadFixedString(enumerator, 2);

                    switch (ApplicationId)
                    {
                        case "91":
                            ServiceCode = ReadFixedString(enumerator, 2);
                            MailerId = ReadFixedString(enumerator, 8);
                            PackageIdentifier = ReadFixedString(enumerator, 9);
                            break;

                        case "93":
                            // Format C07
                            ServiceCode = ReadFixedString(enumerator, 3);
                            MailerId = ReadFixedString(enumerator, 6);
                            PackageIdentifier = ReadFixedString(enumerator, 10);
                            break;

                        case "94":
                            // Format N01 (and N04, but we can't tell them apart)
                            ServiceCode = ReadFixedString(enumerator, 3);
                            SourceId = ReadFixedString(enumerator, 2);
                            MailerId = ReadFixedString(enumerator, 6);
                            PackageIdentifier = ReadFixedString(enumerator, 8);
                            break;

                        case "95":
                            // Format A02
                            ServiceCode = ReadFixedString(enumerator, 3);
                            ChannelId = ReadFixedString(enumerator, 1);
                            DeviceId = ReadFixedString(enumerator, 6);
                            JulianDate = ReadFixedString(enumerator, 4);
                            PackageIdentifier = ReadFixedString(enumerator, 5);
                            break;

                        default:
                            // Format C03
                            ServiceCode = ReadFixedString(enumerator, 3);
                            MailerId = ReadFixedString(enumerator, 9);
                            PackageIdentifier = ReadFixedString(enumerator, 7);
                            break;
                    }

                    CheckDigit = ReadChar(enumerator);

                    if (enumerator.MoveNext())
                        throw new IndexOutOfRangeException("Should not be any more characters");
                    return;
                }
            }
        }

        public override bool IsValid
        {
            get
            {
                bool result = IsInputValid(Input, out string fixedOutput);
                if (fixedOutput != Input)
                    SetInput(fixedOutput);

                return result;
            }
        }

        private static string GetBaseTrackingNumber(string input)
        {
            if (input.Length == 34 && Regex.Match(input, @"^(420\d{9})?9[1-5]").Success)
                return Regex.Replace(input, @"^420\d{9}", "");
            else if (Regex.Match(input, @"^(420\d{5})?9[1-5]").Success)
                return Regex.Replace(input, @"^420\d{5}", "");

            return input;
        }

        internal static bool IsInputValid(string input, out string fixedInput)
        {
            fixedInput = input;

            string baseTrackingNumber = GetBaseTrackingNumber(input);

            if (Regex.Match(baseTrackingNumber, VerifyPattern).Success)
            {
                return IsValidCheckSum(baseTrackingNumber);
            }
            else
            {
                if (!Regex.Match($"91{input}", VerifyPattern).Success)
                    return false;

                if (IsValidCheckSum($"91{input}"))
                {
                    fixedInput = $"91{input}";

                    return true;
                }
                else
                    return false;
            }
        }
    }
}
