using System;
using System.Collections.Generic;

namespace OldPhonePad.Lib
{
    public sealed class SimpleTokenizer : IInputTokenizer
    {
        private readonly char _backspace;
        private readonly char _send;

        public SimpleTokenizer(char backspace = '*', char send = '#')
        {
            _backspace = backspace;
            _send = send;
        }

        public IEnumerable<Token> Tokenize(string input)
        {
            if (input is null) yield break;

            foreach (var ch in input)
            {
                if (ch == _backspace) yield return new Token(TokenType.Backspace, ch);
                else if (ch == _send) yield return new Token(TokenType.Send, ch);
                else if (char.IsWhiteSpace(ch)) yield return new Token(TokenType.Pause, ch);
                else if (char.IsDigit(ch)) yield return new Token(TokenType.Digit, ch);
                else yield return new Token(TokenType.Unknown, ch);
            }
        }
    }
}
