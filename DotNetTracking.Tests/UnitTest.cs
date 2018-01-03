using System;
using System.Linq;
using Xunit;

namespace Haukcode.DotNetTracking.Tests
{
    public class UnitTest
    {
        [Fact]
        public void TestTrackingNumber()
        {
            var result = new TrackingNumber(" 1Z E6A4850190733810 ");

            Assert.True(result.Original == " 1Z E6A4850190733810 ", "Not matching original");

            Assert.True(result.Number == "1ZE6A4850190733810", "Not matching sanitized");

            Assert.True(result.TrackingTypes.Length == 1, "Should only match one tracking type");

            Assert.True(result.TrackingTypes.First().Carrier == "UPS", "Not matching UPS");
        }

        [Fact]
        public void ShouldReturnUnknownWhenGivenInvalidNumber()
        {
            var result = new TrackingNumber("101");

            Assert.True(result.TrackingTypes.Length == 0, "Should return unknown tracking type");
        }

        [Fact]
        public void ShouldUpcaseAndRemoveSpacesFromTrackingNumber()
        {
            var result = new TrackingNumber("abc 123 def");

            Assert.True(result.Number == "ABC123DEF", "Should return upper case number without spaces");
        }

        [Fact]
        public void ShouldRemoveLeadingAndTrailingWhitespaceFromTrackingNnumber()
        {
            var result = new TrackingNumber("  ABC123 \n");

            Assert.True(result.Number == "ABC123", "Should trim result");
        }

        [Fact]
        public void ShouldReturnTwoTrackingNumbersWhenGivenStringWithTwo()
        {
            //TODO
            // Search:       s = TrackingNumber.search("Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, 1Z879E930346834440 nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute 9611020987654312345672 dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.")

            //assert_equal 2, s.size
            //TODO
        }

        [Fact]
        public void ShouldReturnTrackingNumbersWithoutTrailingWhitespace()
        {
            //TODO

            //search("hello 1Z879E930346834440\nbye")
            //assert_equal 1, s.size
            //assert_equal "1Z879E930346834440", s.first.tracking_number
        }
    }
}
