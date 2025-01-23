namespace SAOV.CLI.AzTools
{
    using Spectre.Console.Cli;
    using System.Text;

    internal class Program
    {
        static int Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            CommandApp<AzCommand> app = new();
            return app.Run(args);
        }
    }
}
