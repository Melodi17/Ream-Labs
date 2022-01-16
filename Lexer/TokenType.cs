namespace Ream;

public enum TokenType
{
    // Brackets
    Left_Parenthesis, Right_Parenthesis, Left_Brace,
    Right_Brace,

    // Single character
    Comma, Period, Plus, Minus, Star, Slash, Colon,
    Ampersand, Pipe, Equal, Not, Greater, Less,

    // Multi character
    Not_Equal, Equal_Equal, Greater_Equal, Less_Equal,
    Plus_Equal, Minus_Equal, Star_Equal, Slash_Equal,
    Colon_Colon,

    // Literals
    Identifier, String, Interger, Boolean, Sequence,

    // Keywords
    If, Else, Elif, For, While, Function, Global, Return,
    Null, Class, This,

    End
}
