namespace SAOV.CLI.AzTools.Helpers
{
    using Spectre.Console;
    using System.Text.Json;

    internal static class CommandHelper
    {
        internal static T Run<T>(string command, Dictionary<string, string> paramentersCommand, bool withoutStatus = false)
        {
            foreach (var item in paramentersCommand)
            {
                command = command.Replace(item.Key, item.Value);
            }
            (bool Success, string Output) = withoutStatus ?
                                                AzHelper.GetAzureInfo(command) :
                                                Components.Status.Show<(bool Success, string Output)>("Wait Azure response ...", () => { return AzHelper.GetAzureInfo(command); });
            if (Success)
            {
                if (!string.IsNullOrWhiteSpace(Output))
                {
                    return JsonSerializer.Deserialize<T>(Output);
                }
            }
            else
            {
                AnsiConsole.WriteLine();
                AnsiConsole.WriteException(new Exception(Output));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
            }
            return default;
        }

        internal static bool Run(string command, Dictionary<string, string> paramentersCommand, bool withoutStatus = false)
        {
            foreach (var item in paramentersCommand)
            {
                command = command.Replace(item.Key, item.Value);
            }
            (bool Success, string Output) = withoutStatus ?
                                                AzHelper.GetAzureInfo(command) :
                                                Components.Status.Show<(bool Success, string Output)>("Wait Azure response ...", () => { return AzHelper.GetAzureInfo(command); });
            if (Success)
            {
                AnsiConsole.Write(new Markup("[40]Success operation.[/]"));
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                AnsiConsole.WriteLine();
            }
            else
            {
                AnsiConsole.WriteException(new Exception(Output));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
            }
            _ = Console.ReadKey();
            return Success;
        }
    }
}
