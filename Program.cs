namespace Ream_Labs;
public class Program
{
    public static bool ErrorOccured = false;
    public static void Main(string[] args)
    {
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

    public static void Error(int line, string message)
    {
        Report(line, message);
    }

    public static void Report(int line, string message)
    {
        Console.Error.WriteLine($"Error on line {line}: {message}");
    }
}