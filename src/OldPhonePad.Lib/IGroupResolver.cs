namespace OldPhonePad.Lib
{
    public interface IGroupResolver
    {
        char? Resolve(char digit, int presses);
    }
}
