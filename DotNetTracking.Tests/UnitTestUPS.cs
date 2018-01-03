using System;
using System.Linq;
using Xunit;

namespace Haukcode.DotNetTracking.Tests
{
    public class UnitTestUPS
    {
        [Theory]
        [InlineData("1Z5R89390357567127")]
        [InlineData("1Z879E930346834440")]
        [InlineData("1Z410E7W0392751591")]
        [InlineData("1Z8V92A70367203024")]
        public void ShouldReturnUPSForValidTrackingNumbers(string input)
        {
            TestHelper.TestTrackingNumber(input, i => new UPS(i), UPS.SearchPattern);
        }

        [Fact]
        public void DissectUPSTrackingNumber1()
        {
            var result = new UPS("1ZE6A4850190733810");

            Assert.Equal("01", result.ServiceType);
            Assert.Equal("UPS United States Next Day Air (Red)", result.ServiceTypeDescription);
            Assert.Equal("E6A485", result.ShipperAccount);
            Assert.Equal("9073381", result.PackageIdentifier);
            Assert.Equal('0', result.CheckDigit);
        }

        [Fact]
        public void DissectUPSTrackingNumber2()
        {
            var result = new UPS("1ZX7058Y0396489283");

            Assert.Equal("03", result.ServiceType);
            Assert.Equal("UPS United States Ground", result.ServiceTypeDescription);
            Assert.Equal("X7058Y", result.ShipperAccount);
            Assert.Equal("9648928", result.PackageIdentifier);
            Assert.Equal('3', result.CheckDigit);
        }
    }
}
