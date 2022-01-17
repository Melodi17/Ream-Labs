using Ream.Lexing;
using Ream.Parsing;

namespace Ream
{
    public class Program
    {
        public static bool ErrorOccured = false;
        public static void Main(string[] args)
        {
            Expr expression = new Expr.Binary(
                new Expr.Unary(
                    new Token(TokenType.Minus, "-", null, 1),
                    new Expr.Literal(123)),
                new Token(TokenType.Star, "*", null, 1),
                new Expr.Grouping(
                    new Expr.Literal(45.67)));

            Console.WriteLine(new ASTPrinter().Print(expression));
            return;

            //ASTGenerator.DefineAst(Path.Join("..", "..", "..", "Parsing", "ASTExpr.cs"), "Expr", new string[]
            //{
            //    "Binary : Expr left, Token @operator, Expr right",
            //    "Grouping : Expr expression",
            //    "Literal : Object value",
            //    "Unary : Token @operator, Expr right"
            //}.ToList());

            if (args.Any())
                RunFile(args.First());
            else
                RunPrompt();
        }

        private static void RunFile(string path)
        {
            Run(File.ReadAllText(path));

            if (ErrorOccured)
                Environment.Exit(65);
        }

        private static void RunPrompt()
        {
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line == null) break;
                Run(line);
                ErrorOccured = false;
            }
        }

        private static void Run(string source)
        {
            Lexer lexer = new(source);
            List<Token> tokens = lexer.Lex();

            foreach (Token tok in tokens)
            {
                Console.WriteLine(tok);
            }
        }

        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.End)
                Report(token.Line, " at end", message);
            else
                Report(token.Line, $" at '{token.Raw}'", message);
        }

        public static void Report(int line, string location, string message)
        {
            Console.Error.WriteLine($"Error on line {line}{location}: {message}");
        }
    }
}