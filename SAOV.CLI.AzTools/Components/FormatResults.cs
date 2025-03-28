namespace SAOV.CLI.AzTools.Components
{
    using SAOV.CLI.AzTools.Helpers;
    using Spectre.Console;
    using Spectre.Console.Rendering;

    internal static class FormatResults
    {
        internal static void Show<T>(T dataRaw, Markup title, IRenderable data, string? moduleName)
        {
            string formatResult = TextPrompt.Show<string>("What format do you want to display the query result?", ["table", "json", "file"], 0);
            AnsiConsole.WriteLine();
            switch (formatResult)
            {
                case "json":
                    if (title != null)
                    {
                        AnsiConsole.Write(title);
                        AnsiConsole.WriteLine();
                        AnsiConsole.WriteLine();
                    }
                    AnsiConsole.Write(JsonText.Show<T>(dataRaw));
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine();
                    break;
                case "file":
                    string fileName = $"azs{moduleName}_{DateTime.Now.ToString("yyyyMMddHHmmssff")}.json";
                    File.WriteAllText(fileName, JsonHelper.GetJson<T>(dataRaw));
                    AnsiConsole.Write(new Markup($"[93]The file [yellow]'[/][aqua]{fileName}[/][yellow]'[/] was written successfully.[/]"));
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine();
                    break;
                default:
                    AnsiConsole.Write(data);
                    break;
            }
        }
    }
}
