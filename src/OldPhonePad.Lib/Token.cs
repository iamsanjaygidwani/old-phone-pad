namespace OldPhonePad.Lib
{
    public enum TokenType { Digit, Pause, Backspace, Send, Unknown }

    public sealed record Token(TokenType Type, char Value);
}
