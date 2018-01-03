using System;
using System.Linq;
using Xunit;

namespace Haukcode.DotNetTracking.Tests
{
    public class UnitTestFedEx
    {
        [Theory]
        [InlineData("986578788855")]
        [InlineData("477179081230")]
        [InlineData("799531274483")]
        [InlineData("790535312317")]
        [InlineData("974367662710")]
        public void ShouldReturnFedExExpressForValidTrackingNumbers(string input)
        {
            TestHelper.TestTrackingNumber(input, i => new FedExExpress(i), FedExExpress.SearchPattern);
        }

        [Theory]
        [InlineData("9611020987654312345672")]
        public void ShouldReturnFedEx96ForValidTrackingNumber(string input)
        {
            TestHelper.TestTrackingNumber(input, i => new FedExGround96(i), FedExGround96.SearchPattern);
        }

        [Theory]
        [InlineData("0414 4176 0228 964")]
        [InlineData("5682 8361 0012 000")]
        [InlineData("5682 8361 0012 734")]
        public void ShouldReturnFedExGroundForValidTrackingNumber(string input)
        {
            TestHelper.TestTrackingNumber(input, i => new FedExGround(i), FedExGround.SearchPattern);
        }

        [Theory]
        [InlineData("00 0123 4500 0000 0027")]
        public void ShouldReturnFedExGround18ForValidTrackingNumber(string input)
        {
            TestHelper.TestTrackingNumber(input, i => new FedExGround18(i), FedExGround18.SearchPattern);
        }

        [Theory]
        [InlineData("61299998820821171811")]
        [InlineData("9261292700768711948021")]
        [InlineData("74890100141929418018")]
        public void ShouldReturnFedExSmartPostForValidTrackingNumber(string input)
        {
            TestHelper.TestTrackingNumber(input, i => new FedExSmartPost(i), FedExSmartPost.SearchPattern);
        }

        [Fact]
        public void DissectFedExGround18()
        {
            var result = new FedExGround18("00 0123 4500 0000 0027");

            Assert.Equal("00", result.ApplicationId);
            Assert.Equal("0", result.SerialContainer);
            Assert.Equal("0", result.ServiceCode);
            Assert.Equal("1234500", result.ShipperId);
            Assert.Equal("0000002", result.PackageIdentifier);
            Assert.Equal('7', result.CheckDigit);
        }

        [Fact]
        public void DissectFedExGround96()
        {
            var result = new FedExGround96("9611020987654312345672");

            Assert.Equal("96", result.ApplicationId);
            Assert.Equal("11", result.SerialContainer);
            Assert.Equal("020", result.ServiceCode);
            Assert.Equal("9876543", result.ShipperId);
            Assert.Equal("1234567", result.PackageIdentifier);
            Assert.Equal('2', result.CheckDigit);
        }
    }
}
