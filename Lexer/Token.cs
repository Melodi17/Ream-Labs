namespace Ream
{
    public class Token
    {
        public readonly TokenType Type;
        public readonly string Raw;
        public readonly object Value;
        public readonly int Line;

        public Token(TokenType type, string raw, object value, int line)
        {
            Type = type;
            Raw = raw;
            Value = value;
            Line = line;
        }

        public override string ToString()
        {
            return $"{Type} {Raw} {Value}";
        }
    }
}