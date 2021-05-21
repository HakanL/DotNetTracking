using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Haukcode.DotNetTracking.Tests
{
    public static class TestHelper
    {
        private static string ChangeCheckDigit(string input)
        {
            char last;
            var seq = TrackingType.GetSequence(input, out last);
            int.TryParse(last.ToString(), out int checkDigit);
            char newCheckDigit = (checkDigit <= 2 ? checkDigit + 3 : checkDigit - 3).ToString()[0];

            return string.Join("", seq) + newCheckDigit;
        }

        public static void TestTrackingNumber(string input, Func<string, TrackingType> tester, params string[] searchPatterns)
        {
            var result = tester(input);
            Assert.True(result.IsValid);

            // Verify that we can resolve this tracking number
            var trackNumber = new TrackingNumber(input);
            Assert.Equal(input, trackNumber.Original);
            var matching = trackNumber.TrackingTypes.Where(x => x.Carrier == result.Carrier);
            Assert.Single(matching);
            Assert.Equal(result.GetType(), matching.First().GetType());

            // Check for invalid checksum digit
            string invalidInput = ChangeCheckDigit(input);

            var resultInvalid = tester(invalidInput);
            Assert.False(resultInvalid.IsValid);

            // Verify that we cannot resolve this tracking number
            trackNumber = new TrackingNumber(invalidInput);

            Assert.Equal(invalidInput, trackNumber.Original);
            Assert.Empty(trackNumber.TrackingTypes);

            if (searchPatterns != null && searchPatterns.Any())
            {
                var searchStrings = TestHelper.GetPossibleStrings(input);

                foreach (string searchString in searchStrings)
                {
                    var searchResult = new List<string>();
                    foreach (string searchPattern in searchPatterns)
                    {
                        var subResult = TrackingType.Search(searchString, searchPattern);
                        searchResult.AddRange(subResult);
                        if (subResult.Any())
                            break;
                    }

                    Assert.Single(searchResult);
                }
            }
        }

        public static IEnumerable<string> GetPossibleNumbers(string input)
        {
            var list = new List<string>();
            list.Add(input);
            list.Add(input.Replace(" ", ""));
            list.Add(string.Join(" ", input.ToCharArray()));
            list.Add(string.Join("  ", input.ToCharArray()));
            list.Add(input.Substring(0, input.Length / 2) + "  " + input.Substring(input.Length / 2));

            return list.Distinct();
        }

        public static IEnumerable<string> GetPossibleStrings(string input)
        {
            return GetPossibleNumbers(input).Select(x => GetSearchString(x));
        }

        public static string GetSearchString(string number)
        {
            return $"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor {number} ut labore et dolore magna aliqua.";
        }
    }
}
