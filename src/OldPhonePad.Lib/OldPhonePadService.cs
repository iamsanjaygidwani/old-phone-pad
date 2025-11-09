using System;
using System.Text;
using Microsoft.Extensions.Logging;

namespace OldPhonePad.Lib
{
    public sealed class OldPhonePadService : IOldPhonePadService
    {
        private readonly IInputTokenizer _tokenizer;
        private readonly IGroupResolver _resolver;
        private readonly ILogger<OldPhonePadService>? _logger;
        private const int DefaultMaxInputLength = 100_000; // guardrail

        public OldPhonePadService(IInputTokenizer tokenizer, IGroupResolver resolver, ILogger<OldPhonePadService>? logger = null)
        {
            _tokenizer = tokenizer ?? throw new ArgumentNullException(nameof(tokenizer));
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _logger = logger;
        }

        public string Decode(string input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));
            if (input.Length > DefaultMaxInputLength) throw new ArgumentException("Input too large", nameof(input));

            var sb = new StringBuilder(capacity: Math.Min(128, input.Length));
            char? currentDigit = null;
            int count = 0;

            void Flush()
            {
                if (currentDigit != null)
                {
                    var ch = _resolver.Resolve(currentDigit.Value, count);
                    if (ch != null) sb.Append(ch.Value);
                    else _logger?.LogWarning("Unknown mapping for digit {Digit}", currentDigit);
                    currentDigit = null;
                    count = 0;
                }
            }

            foreach (var token in _tokenizer.Tokenize(input))
            {
                switch (token.Type)
                {
                    case TokenType.Digit:
                        if (currentDigit == null) { currentDigit = token.Value; count = 1; }
                        else if (currentDigit == token.Value) { count++; }
                        else { Flush(); currentDigit = token.Value; count = 1; }
                        break;

                    case TokenType.Pause:
                        Flush();
                        break;

                    case TokenType.Backspace:
                        Flush();
                        if (sb.Length > 0) { sb.Length -= 1; _logger?.LogDebug("Backspace applied"); }
                        else _logger?.LogDebug("Backspace ignored (empty)");
                        break;

                    case TokenType.Send:
                        Flush();
                        goto Done;

                    case TokenType.Unknown:
                        Flush();
                        _logger?.LogDebug("Ignored unknown char {Char}", token.Value);
                        break;
                }
            }

        Done:
            // flush remaining if no send token encountered
            Flush();
            return sb.ToString();
        }
    }
}
