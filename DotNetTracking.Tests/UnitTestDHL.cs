using System;
using System.Linq;
using Xunit;

namespace Haukcode.DotNetTracking.Tests
{
    public class UnitTestDHL
    {
        [Theory]
        [InlineData("73891051146")]
        public void ShouldReturnDHLExpressAirForValidTrackingNumber(string input)
        {
            TestHelper.TestTrackingNumber(input, i => new DHLExpressAir(i), DHLExpressAir.SearchPattern);
        }

        [Theory]
        [InlineData("3318810025")]
        [InlineData("8487135506")]
        [InlineData("3318810036")]
        [InlineData("3318810014")]
        public void ShouldReturnDHLExpressForValidTrackingNumbers(string input)
        {
            TestHelper.TestTrackingNumber(input, i => new DHLExpress(i), DHLExpress.SearchPattern);
        }
    }
}
