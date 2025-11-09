using System.Collections.Generic;

namespace OldPhonePad.Lib
{
    public interface IInputTokenizer
    {
        IEnumerable<Token> Tokenize(string input);
    }
}
