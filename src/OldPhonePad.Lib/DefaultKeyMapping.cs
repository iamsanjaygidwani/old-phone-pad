using System.Collections.Generic;

namespace OldPhonePad.Lib
{
    public sealed class DefaultKeyMapping : IKeyMapping
    {
        private static readonly Dictionary<char, string> _map = new()
        {
            ['1'] = "&'(",
            ['2'] = "ABC",
            ['3'] = "DEF",
            ['4'] = "GHI",
            ['5'] = "JKL",
            ['6'] = "MNO",
            ['7'] = "PQRS",
            ['8'] = "TUV",
            ['9'] = "WXYZ",
            ['0'] = " "
        };

        public bool TryGetMapping(char digit, out string? mapping) => _map.TryGetValue(digit, out mapping);
    }
}
