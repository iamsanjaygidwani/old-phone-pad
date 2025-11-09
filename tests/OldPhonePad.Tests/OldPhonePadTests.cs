using System;
using Xunit;
using OldPhonePad.Lib;
using Microsoft.Extensions.Logging.Abstractions;

namespace OldPhonePad.Tests
{
    public class OldPhonePadTests
    {
        private IOldPhonePadService CreateService()
        {
            var mapping = new DefaultKeyMapping();
            var tokenizer = new SimpleTokenizer();
            var resolver = new GroupResolver(mapping);
            var logger = new NullLogger<OldPhonePadService>();
            return new OldPhonePadService(tokenizer, resolver, logger);
        }

        [Theory]
        [InlineData("33#", "E")]
        [InlineData("227*#", "B")]
        [InlineData("4433555 555666#", "HELLO")]
        [InlineData("8 88777444666*664#", "TURING")]
        [InlineData("2#", "A")]
        [InlineData("222#", "C")]
        [InlineData("0#", " ")]
        public void Examples(string input, string expected)
        {
            var svc = CreateService();
            var result = svc.Decode(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BackspaceOnEmptyDoesNotThrow()
        {
            var svc = CreateService();
            Assert.Equal(string.Empty, svc.Decode("*#"));
        }

        [Fact]
        public void HandlesNoSendGracefully()
        {
            var svc = CreateService();
            Assert.Equal("BA", svc.Decode("22 2"));
        }

        [Fact]
        public void WrapAroundBehavior()
        {
            var svc = CreateService();
            Assert.Equal("A", svc.Decode("2222#"));
        }

        [Fact]
        public void RejectsTooLargeInput()
        {
            var svc = CreateService();
            var tooLarge = new string('2', 100_001);
            Assert.Throws<ArgumentException>(() => svc.Decode(tooLarge));
        }

        [Fact]
        public void ConsecutiveBackspacesRemoveMultipleChars()
        {
            var svc = CreateService();
            var result = svc.Decode("4433555 555666**#");
            Assert.Equal("HEL", result);
        }

        [Fact]
        public void BackspaceAfterDigitRemovesFlushedChar()
        {
            var svc = CreateService();
            Assert.Equal(string.Empty, svc.Decode("2*#"));
        }

        [Fact]
        public void UnknownCharactersActAsSeparatorsAndAreIgnored()
        {
            var svc = CreateService();
            Assert.Equal("AA", svc.Decode("2a2#"));
        }

        [Fact]
        public void MultipleSendSymbolsStopsAtFirstSend()
        {
            var svc = CreateService();
            Assert.Equal("B", svc.Decode("22##"));
        }
    }
}
