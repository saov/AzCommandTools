namespace SAOV.CLI.AzTools.Components
{
    using Spectre.Console;
    using Spectre.Console.Rendering;

    internal static class FormatResults
    {
        internal static void Show<T>(T dataRaw, Markup title, IRenderable data) {
            string formatResult = TextPrompt.Show<string>("What format do you want to display the query result?", ["table", "json"], 0);
            AnsiConsole.WriteLine();
            if (formatResult == "json")
            {
                if (title != null)
                {
                    AnsiConsole.Write(title);
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine();
                }
                AnsiConsole.Write(JsonText.Show<T>(dataRaw));
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();
                return;
            }
            AnsiConsole.Write(data);
        }
    }
}
