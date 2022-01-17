using Ream.Interpreting;
using Ream.Lexing;
using Ream.Parsing;
using Ream.Tools;

namespace Ream
{
    public class Program
    {
        public static readonly Interpreter Interpreter = new();
        public static bool ErrorOccured = false;
        public static bool RuntimeErrorOccured = false;
        public static void Main(string[] args)
        {
            //ASTGenerator.DefineAst(Path.Join("..", "..", "..", "Parsing", "ASTExpr.cs"), "Expr", new string[]
            //{
            //    "Binary : Expr left, Token @operator, Expr right",
            //    "Grouping : Expr expression",
            //    "Literal : Object value",
            //    "Unary : Token @operator, Expr right"
            //}.ToList());

            //ASTGenerator.DefineAst(Path.Join("..", "..", "..", "Interpreting", "Stmt.cs"), "Stmt", new string[]
            //{
            //    "Expression : Expr expression",
            //    "Write      : Expr expression",
            //}.ToList());

            if (args.Any())
                RunFile(args.First());
            else
                RunPrompt();
        }

        private static void RunFile(string path)
        {
            Run(File.ReadAllText(path));

            if (ErrorOccured) Environment.Exit(65);
            if (RuntimeErrorOccured) Environment.Exit(70);
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
            Parser parser = new Parser(tokens);
            List<Stmt> statements = parser.Parse();

            if (ErrorOccured) return;

            Interpreter.Interpret(statements);
            //Console.WriteLine(new ASTPrinter().Print(expression));
        }
        public static void RuntimeError(RuntimeError error)
        {
            Console.Error.WriteLine($"Error on line {error.Token.Line}: {error.Message}");
            RuntimeErrorOccured = true;
        }

        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.End)
                Report(token.Line, " at end", message);
            else
                Report(token.Line, $" at '{token.Raw}'", message);
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        public static void Report(int line, string location, string message)
        {
            Console.Error.WriteLine($"Error on line {line}{location}: {message}");
        }
    }
}