using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class Amazon : TrackingType
    {
        public const string SearchPattern = @"\bTBA[0-9]{12}\b";
        public const string VerifyPattern = @"^TBA[0-9]{12}$";

        public Amazon(string input)
            : base(input)
        {
            Decode();
        }

        private void Decode()
        {
            if (IsInputValid(Input))
            {
                var enumerator = Input.GetEnumerator();

                // TBA
                ReadFixedString(enumerator, 3);

                TrackingIdentifier = ReadFixedString(enumerator, 12);

                if (enumerator.MoveNext())
                    throw new IndexOutOfRangeException("Should not be any more characters");
            }
        }

        public override string Carrier => "Amazon";

        public string TrackingIdentifier { get; private set; }

        public override bool IsValid => IsInputValid(Input);

        public override string TrackingURL => $"https://track.amazon.com/tracking/{Input}";

        private static bool IsInputValid(string input)
        {
            if (input.Length != 15)
                return false;

            if (!Regex.Match(input, VerifyPattern).Success)
                return false;

            return true;
        }

        public static TrackingType Match(string input)
        {
            if (IsInputValid(input))
                return new Amazon(input);

            return null;
        }
    }
}
