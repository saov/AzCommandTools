namespace SAOV.CLI.AzTools.Components
{
    using Spectre.Console;

    internal static class SelectionPrompt
    {
        internal static string Show(List<string> choices, string choicesColorText = "[93]", string title = null, int pageSize = 10, string moreChoicesText = "[grey](Move up and down to reveal more options)[/]", string searchPlaceholderText = "[grey](Type to search)[/]", bool withCancelOrExitchoise = true)
        {
            string displaySelector(string str)
            {
                return $"{choicesColorText}{str}[/]";
            }
            int choicesCount = withCancelOrExitchoise ? choices.Count - 1 : choices.Count;
            title = title != null ? title : $"[yellow]Select an option ([40]{choicesCount}[/]).[/]";
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"{title}")
                    .PageSize(pageSize)
                    .MoreChoicesText(moreChoicesText)
                    .AddChoices(choices)
                    .UseConverter(displaySelector)
                    .HighlightStyle(Style.Plain.Background(Color.Grey))
                    .SearchPlaceholderText(searchPlaceholderText)
                    .EnableSearch()
            );
        }
    }
}
