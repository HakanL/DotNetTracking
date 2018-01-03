using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

// Credit: https://github.com/jkeen/tracking_number

namespace Haukcode.DotNetTracking
{
    public abstract class TrackingType
    {
        private string input;

        public TrackingType(string input)
        {
            this.input = TrackingNumber.Sanitize(input);
        }

        public abstract string Carrier { get; }

        public abstract bool IsValid { get; }

        public string Input => this.input;

        protected void SetInput(string input)
        {
            this.input = input;
        }

        private static void Match(string input, ref List<TrackingType> list, Func<string, TrackingType> checker)
        {
            var result = checker(input);

            if (result != null)
                list.Add(result);
        }

        protected static IEnumerable<T> WithoutLast<T>(IEnumerable<T> source)
        {
            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    for (var value = e.Current; e.MoveNext(); value = e.Current)
                    {
                        yield return value;
                    }
                }
            }
        }

        public static (IEnumerable<int> Sequence, int CheckDigit) GetDigits(string input)
        {
            var all = input.ToCharArray().Select(x => int.Parse(x.ToString()));

            return (WithoutLast(all), all.Last());
        }

        public static (IEnumerable<char> Sequence, char CheckDigit) GetSequence(string input)
        {
            var all = input.ToCharArray();

            return (WithoutLast(all), all.Last());
        }

        public static TrackingType[] GetMatches(string input)
        {
            var list = new List<TrackingType>();

            Match(input, ref list, UPS.Match);
            Match(input, ref list, FedEx.Match);
            Match(input, ref list, DHL.Match);
            Match(input, ref list, OnTrac.Match);
            Match(input, ref list, USPS.Match);

            return list.ToArray();
        }

        public static IEnumerable<string> Search(string input, string searchPattern)
        {
            var match = Regex.Match(input, searchPattern);

            if (!match.Success)
                return new string[0];

            return ((ICollection<Capture>)match.Captures).Select(x => x.Value);
        }

        public static string ReadFixedString(IEnumerator<char> numerator, int count)
        {
            var sb = new StringBuilder(count);
            for (int i = 0; i < count; i++)
            {
                if (numerator.MoveNext())
                    sb.Append(numerator.Current);
                else
                    throw new IndexOutOfRangeException("No characters to read");
            }

            return sb.ToString();
        }

        public static char ReadChar(IEnumerator<char> numerator)
        {
            if (numerator.MoveNext())
                return numerator.Current;
            else
                throw new IndexOutOfRangeException("No characters to read");
        }
    }
}
