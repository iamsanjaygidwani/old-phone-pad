using System.Collections.Generic;

namespace OldPhonePad.Lib
{
    public interface IKeyMapping
    {
        bool TryGetMapping(char digit, out string? mapping);
    }
}
