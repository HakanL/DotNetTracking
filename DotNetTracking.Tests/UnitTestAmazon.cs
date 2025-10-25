using System;
using System.Linq;
using Xunit;

namespace Haukcode.DotNetTracking.Tests
{
    public class UnitTestAmazon
    {
        [Theory]
        [InlineData("TBA325393026547")]
        public void ShouldReturnAmazonForValidTrackingNumbers(string input)
        {
            var result = new Amazon(input);
            Assert.True(result.IsValid);

            // Verify that we can resolve this tracking number
            var trackNumber = new TrackingNumber(input);
            Assert.Equal(input, trackNumber.Original);
            var matching = trackNumber.TrackingTypes.Where(x => x.Carrier == "Amazon");
            Assert.Single(matching);
            Assert.Equal(typeof(Amazon), matching.First().GetType());

            // Test search pattern - only test continuous string, not spaced versions
            string searchString = $"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor {input} ut labore et dolore magna aliqua.";
            var searchResult = TrackingType.Search(searchString, Amazon.SearchPattern);
            Assert.Single(searchResult);
            Assert.Equal(input, searchResult.First());
        }

        [Fact]
        public void DissectAmazonTrackingNumber()
        {
            var result = new Amazon("TBA325393026547");

            Assert.Equal("325393026547", result.TrackingIdentifier);
            Assert.Equal("Amazon", result.Carrier);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ShouldValidateTrackingURL()
        {
            var result = new Amazon("TBA325393026547");

            Assert.Equal("https://track.amazon.com/tracking/TBA325393026547", result.TrackingURL);
        }

        [Fact]
        public void ShouldRejectInvalidLength()
        {
            var result = new Amazon("TBA12345");

            Assert.False(result.IsValid);
        }

        [Fact]
        public void ShouldRejectInvalidPrefix()
        {
            var result = new Amazon("ABC325393026547");

            Assert.False(result.IsValid);
        }

        [Fact]
        public void ShouldRejectInvalidFormat()
        {
            var result = new Amazon("TBAABC393026547");

            Assert.False(result.IsValid);
        }
    }
}
