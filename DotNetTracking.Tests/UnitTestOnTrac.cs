using System;
using System.Linq;
using Xunit;

namespace Haukcode.DotNetTracking.Tests
{
    public class UnitTestOnTrac
    {
        [Theory]
        [InlineData("C11031500001879")]
        [InlineData("C10999911320231")]
        public void ShouldReturnOnTracForValidTrackingNumbers(string input)
        {
            TestHelper.TestTrackingNumber(input, i => new OnTrac(i), OnTrac.SearchPattern);
        }

        [Theory]
        [InlineData("C10999911320230")]
        public void ShouldReturnOnTracForInvalidTrackingNumber(string input)
        {
            var result = new OnTrac(input);
            Assert.False(result.IsValid);
        }
    }
}
