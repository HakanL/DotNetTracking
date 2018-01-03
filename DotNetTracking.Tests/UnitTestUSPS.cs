using System;
using System.Linq;
using Xunit;

namespace Haukcode.DotNetTracking.Tests
{
    public class UnitTestUSPS
    {
        [Theory]
        [InlineData("9101 1234 5678 9000 0000 13")]
        [InlineData("9400 1112 0108 0805 4830 16")]
        [InlineData("9505 5110 6960 5048 6006 24")]
        [InlineData("7196 9010 7560 0307 7385")]
        [InlineData("9361 2898 7870 0317 6337 95")]
        [InlineData("9212 3912 3456 7812 3456 70")]
        public void ShouldReturnUSPS91ForValid22DigitTrackingNumbers(string input)
        {
            TestHelper.TestTrackingNumber(input, i => new USPS91(i), USPS91.SearchPatterns);
        }

        [Theory]
        [InlineData("0307 1790 0005 2348 3741")]
        public void ShouldReturnUSPS20ForValidTrackingNumber(string input)
        {
            TestHelper.TestTrackingNumber(input, i => new USPS20(i), USPS20.SearchPattern);
        }

        [Theory]
        [InlineData("RB123456785US")]
        public void ShouldReturnUSPS13ForValidTrackingNumber(string input)
        {
            TestHelper.TestTrackingNumber(input, i => new USPS13(i), USPS13.SearchPattern);
        }

        [Theory]
        [InlineData("420221539101026837331000039521")]
        public void ShouldReturnUSPS91ForValid30DigitTrackingNumber(string input)
        {
            // Can't currently be searched for in strings
            TestHelper.TestTrackingNumber(input, i => new USPS91(i), null);
        }

        [Theory]
        [InlineData("92748931507708513018050063")]
        public void ShouldReturnUSPS91ForValid26DigitTrackingNumber(string input)
        {
            // Can't currently be searched for in strings
            TestHelper.TestTrackingNumber(input, i => new USPS91(i), null);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber22Digits()
        {
            var result = new USPS91("9101 1234 5678 9000 0000 13");

            Assert.Equal("91", result.ApplicationId);
            Assert.Equal("01", result.ServiceCode);
            Assert.Equal("12345678", result.MailerId);
            Assert.Equal("900000001", result.PackageIdentifier);
            Assert.Equal('3', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber34DigitsFormatC01()
        {
            var result = new USPS91("420 123456789 9212 3912 3456 7812 3456 70");

            Assert.Equal("92", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("912345678", result.MailerId);
            Assert.Equal("1234567", result.PackageIdentifier);
            Assert.Equal('0', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber34DigitsFormatC02()
        {
            var result = new USPS91("420 12345 9212 3912 3456 7812 3456 7890 12");

            Assert.Equal("92", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("912345678", result.MailerId);
            Assert.Equal("12345678901", result.PackageIdentifier);
            Assert.Equal('2', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber30DigitsFormatC03()
        {
            var result = new USPS91("420 12345 9212 3912 3456 7812 3456 70");

            Assert.Equal("92", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("912345678", result.MailerId);
            Assert.Equal("1234567", result.PackageIdentifier);
            Assert.Equal('0', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber22DigitsFormatC04()
        {
            var result = new USPS91("9212 3912 3456 7812 3456 70");

            Assert.Equal("92", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("912345678", result.MailerId);
            Assert.Equal("1234567", result.PackageIdentifier);
            Assert.Equal('0', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber34DigitsFormatC05()
        {
            var result = new USPS91("420 123456789 9312 3123 4561 2345 6789 06");

            Assert.Equal("93", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("123456", result.MailerId);
            Assert.Equal("1234567890", result.PackageIdentifier);
            Assert.Equal('6', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber34DigitsFormatC06()
        {
            var result = new USPS91("420 12345 9312 3123 4561 2345 6789 0123 44");

            Assert.Equal("93", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("123456", result.MailerId);
            Assert.Equal("12345678901234", result.PackageIdentifier);
            Assert.Equal('4', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber30DigitsFormatC07()
        {
            var result = new USPS91("420 12345 9312 3123 4561 2345 6789 06");

            Assert.Equal("93", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("123456", result.MailerId);
            Assert.Equal("1234567890", result.PackageIdentifier);
            Assert.Equal('6', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber22DigitsFormatC08()
        {
            var result = new USPS91("9312 3123 4561 2345 6789 06");

            Assert.Equal("93", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("123456", result.MailerId);
            Assert.Equal("1234567890", result.PackageIdentifier);
            Assert.Equal('6', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber26DigitsFormatC09()
        {
            var result = new USPS91("9312 3123 4561 2345 6789 0123 44");

            Assert.Equal("93", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("123456", result.MailerId);
            Assert.Equal("12345678901234", result.PackageIdentifier);
            Assert.Equal('4', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber26DigitsFormatC10()
        {
            var result = new USPS91("9212 3123 4567 8912 3456 7890 14");

            Assert.Equal("92", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("123456789", result.MailerId);
            Assert.Equal("12345678901", result.PackageIdentifier);
            Assert.Equal('4', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber30DigitsFormatN01()
        {
            var result = new USPS91("420 12345 9412 3121 2345 6123 4567 87");

            Assert.Equal("94", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("12", result.SourceId);
            Assert.Equal("123456", result.MailerId);
            Assert.Equal("12345678", result.PackageIdentifier);
            Assert.Equal('7', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber34DigitsFormatN02()
        {
            var result = new USPS91("420 123456789 9412 3121 2345 6123 4567 87");

            Assert.Equal("94", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("12", result.SourceId);
            Assert.Equal("123456", result.MailerId);
            Assert.Equal("12345678", result.PackageIdentifier);
            Assert.Equal('7', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber22DigitsFormatN03()
        {
            var result = new USPS91("9412 3121 2345 6123 4567 87");

            Assert.Equal("94", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("12", result.SourceId);
            Assert.Equal("123456", result.MailerId);
            Assert.Equal("12345678", result.PackageIdentifier);
            Assert.Equal('7', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber30DigitsFormatN04()
        {
            var result = new USPS91("420 12345 9412 3129 1234 5678 1234 58");

            Assert.Equal("94", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("12", result.SourceId);
            // Note that this gets detected as format N01 since there's no way to tell them apart
            Assert.Equal("912345", result.MailerId);
            Assert.Equal("67812345", result.PackageIdentifier);
            Assert.Equal('8', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber34DigitsFormatA01()
        {
            var result = new USPS91("420 123456789 9512 3112 3456 1234 1234 53");

            Assert.Equal("95", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("1", result.ChannelId);
            Assert.Equal("123456", result.DeviceId);
            Assert.Equal("1234", result.JulianDate);
            Assert.Equal("12345", result.PackageIdentifier);
            Assert.Equal('3', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber30DigitsFormatA02()
        {
            var result = new USPS91("420 12345 9512 3112 3456 1234 1234 53");

            Assert.Equal("95", result.ApplicationId);
            Assert.Equal("123", result.ServiceCode);
            Assert.Equal("1", result.ChannelId);
            Assert.Equal("123456", result.DeviceId);
            Assert.Equal("1234", result.JulianDate);
            Assert.Equal("12345", result.PackageIdentifier);
            Assert.Equal('3', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber26Digits()
        {
            var result = new USPS91("92748931507708513018050063");

            Assert.Equal("92", result.ApplicationId);
            Assert.Equal("748", result.ServiceCode);
            Assert.Equal("931507708", result.MailerId);
            Assert.Equal("51301805006", result.PackageIdentifier);
            Assert.Equal('3', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS91TrackingNumber30Digits()
        {
            var result = new USPS91("420221539101026837331000039521");

            Assert.Equal("91", result.ApplicationId);
            Assert.Equal("01", result.ServiceCode);
            Assert.Equal("02683733", result.MailerId);
            Assert.Equal("100003952", result.PackageIdentifier);
            Assert.Equal('1', result.CheckDigit);
        }

        [Fact]
        public void DissectUSPS20TrackingNumber()
        {
            var result = new USPS20("0307 1790 0005 2348 3741");

            Assert.Equal("03", result.ServiceCode);
            Assert.Equal("071790000", result.MailerId);
            Assert.Equal("52348374", result.PackageIdentifier);
            Assert.Equal('1', result.CheckDigit);
        }
    }
}
