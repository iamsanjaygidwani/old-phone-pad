using System;

namespace OldPhonePad.Lib
{
    public sealed class GroupResolver : IGroupResolver
    {
        private readonly IKeyMapping _mapping;

        public GroupResolver(IKeyMapping mapping)
        {
            _mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        }

        public char? Resolve(char digit, int presses)
        {
            if (!_mapping.TryGetMapping(digit, out var map) || string.IsNullOrEmpty(map)) return null;
            if (presses <= 0) return null;
            var idx = (presses - 1) % map.Length;
            return map[idx];
        }
    }
}
