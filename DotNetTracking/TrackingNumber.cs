using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haukcode.DotNetTracking
{
    public class TrackingNumber
    {
        private TrackingType[] trackingTypes;

        public string Original { get; private set; }

        public string Number { get; private set; }

        public TrackingNumber(string input)
        {
            Original = input;
            Number = Sanitize(input);
        }

        public static string Sanitize(string input)
        {
            return input.Trim().Replace(" ", "").ToUpper();
        }

        public TrackingType[] TrackingTypes
        {
            get
            {
                if (this.trackingTypes != null)
                    // Cached
                    return this.trackingTypes;

                this.trackingTypes = TrackingType.GetMatches(Number);

                return this.trackingTypes;
            }
        }
    }
}
