namespace GitHubGrabber.Helpers
{
    [ExcludeFromCodeCoverage]
    public class ConsoleWriter
    {
        public static void WriteLine(string message, NewLineIn? newLineIn = null)
        {
            switch (newLineIn)
            {
                case NewLineIn.Start:
                    Console.WriteLine($"{Environment.NewLine}{message}");
                    break;

                case NewLineIn.End:
                    Console.WriteLine($"{message}{Environment.NewLine}");
                    break;

                default:
                    Console.WriteLine(message);
                    break;
            }
        }

        public static void WriteLines(IEnumerable<string> strings)
        {
            foreach (var str in strings)
            {
                Console.WriteLine(str);
            }
        }
    }
}
