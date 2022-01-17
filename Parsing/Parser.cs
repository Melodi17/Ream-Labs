using Ream.Lexing;

namespace Ream.Parsing
{
    public class Parser
    {
        private class ParseError : Exception { }
        public bool AtEnd => Peek().Type == TokenType.End;
        private readonly List<Token> Tokens;
        private int Current = 0;

        public Parser(List<Token> tokens)
        {
            this.Tokens = tokens;
        }
        public List<Stmt> Parse()
        {
            List<Stmt> statements = new();
            while (!AtEnd)
            {
                statements.Add(Statement());
            }

            return statements;
        }
        private Expr Expression()
        {
            return ExprEquality();
        }
        private Expr ExprEquality()
        {
            Expr expr = ExprComparison();

            while (Match(TokenType.Not_Equal, TokenType.Equal_Equal))
            {
                Token op = Previous();
                Expr right = ExprComparison();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }
        private Expr ExprComparison()
        {
            Expr expr = ExprTerm();

            while (Match(TokenType.Greater, TokenType.Greater_Equal, TokenType.Less, TokenType.Less_Equal))
            {
                Token op = Previous();
                Expr right = ExprTerm();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }
        private Expr ExprTerm()
        {
            Expr expr = ExprFactor();

            while (Match(TokenType.Plus, TokenType.Minus))
            {
                Token op = Previous();
                Expr right = ExprFactor();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }
        private Expr ExprFactor()
        {
            Expr expr = ExprUnary();

            while (Match(TokenType.Slash, TokenType.Star))
            {
                Token op = Previous();
                Expr right = ExprUnary();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }
        private Expr ExprUnary()
        {
            if (Match(TokenType.Not, TokenType.Minus))
            {
                Token op = Previous();
                Expr right = ExprUnary();
                return new Expr.Unary(op, right);
            }

            return ExprPrimary();
        }
        private Expr ExprPrimary()
        {
            Token tok = Peek();
            if (Match(TokenType.True)) return new Expr.Literal(true);
            if (Match(TokenType.False)) return new Expr.Literal(true);
            if (Match(TokenType.Null)) return new Expr.Literal(true);
            if (Match(TokenType.String, TokenType.Interger))
                return new Expr.Literal(Previous().Value);
            if (Match(TokenType.Left_Parenthesis))
            {
                Expr expr = Expression();
                Consume(TokenType.Right_Parenthesis, "Expected ')' after expression");
                return new Expr.Grouping(expr);
            }

            throw Error(Peek(), "Expected expression");
        }
        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }
        private bool Check(TokenType type)
        {
            if (AtEnd) return false;
            return Peek().Type == type;
        }
        private Token Consume(TokenType type, string message, bool allowPrematureEnd = false)
        {
            if (Check(type) || (allowPrematureEnd && AtEnd)) return Advance();

            throw Error(Peek(), message);
        }
        private Token Advance()
        {
            if (!AtEnd) Current++;
            return Previous();
        }
        private Token Peek()
            => Tokens[Current];
        private Token Previous()
            => Tokens[Current - 1];
        private ParseError Error(Token token, string message)
        {
            Program.Error(token, message);
            return new ParseError();
        }
        private void Synchronize()
        {
            Advance();

            while (!AtEnd)
            {
                if (Previous().Type == TokenType.Newline) return;

                switch (Peek().Type)
                {
                    case TokenType.If:
                    case TokenType.Else:
                    case TokenType.Elif:
                    case TokenType.For:
                    case TokenType.While:
                    case TokenType.Function:
                    case TokenType.Global:
                    case TokenType.Return:
                    case TokenType.Class:
                        return;
                }

                Advance();
            }
        }

        private Stmt Statement()
        {
            if (Match(TokenType.Write)) return PrintStatement();

            return ExpressionStatement();
        }

        private Stmt ExpressionStatement()
        {
            Expr expr = Expression();
            Consume(TokenType.Newline, "Expected line to end", true);
            return new Stmt.Expression(expr);
        }

        private Stmt PrintStatement()
        {
            Expr value = Expression();
            Consume(TokenType.Newline, "Expected line to end", true);
            return new Stmt.Write(value);
        }
    }
}
