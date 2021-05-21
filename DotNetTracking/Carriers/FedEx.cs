using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public abstract class FedEx : TrackingType
    {
        public override string Carrier => "FedEx";

        public FedEx(string input)
            : base(input)
        {
        }

        public override string TrackingURL => $"https://www.fedex.com/apps/fedextrack/?tracknumbers={Input}";

        public static TrackingType Match(string input)
        {
            if (FedExExpress.IsInputValid(input))
                return new FedExExpress(input);

            if (FedExGround.IsInputValid(input))
                return new FedExGround(input);

            if (FedExGround18.IsInputValid(input))
                return new FedExGround18(input);

            if (FedExGround96.IsInputValid(input))
                return new FedExGround96(input);

            if (FedExSmartPost.IsInputValid(input))
                return new FedExSmartPost(input);

            return null;
        }

        protected static bool IsValidCheckSum(string input)
        {
            int checkDigit;
            var sequence = GetDigits(input, out checkDigit);

            int total = sequence.Reverse().Select((x, i) => (i % 2 == 0) ? x * 3 : x).Sum();

            int check = total % 10;
            if (check > 0)
                check = (10 - check);

            return check == checkDigit;
        }
    }
}
