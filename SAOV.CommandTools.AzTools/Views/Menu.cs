namespace SAOV.CommandTools.AzTools.Views
{
    using SAOV.CommandTools.AzTools.Commands.AzAccountShow;
    using SAOV.CommandTools.AzTools.Commands.AzAccountSubscriptionList;
    using SAOV.CommandTools.AzTools.Commands.AzCliVersion;
    using SAOV.CommandTools.AzTools.Commands.AzResourceGroupList;
    using Spectre.Console;

    internal static class Menu
    {
        internal static void Get()
        {
            List<string> choices = [];
            bool run = true;
            Enum.GetValues<MenuOptions>().ToList().ForEach(item => { choices.Add(item.ToString()); });
            Func<string, string> displaySelector = str =>
            {
                return $"[93]{str}[/]";
            };
            while (run)
            {
                string option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select a [40]query[/] or [/][red]CTR+C to exit.[/]")
                    .PageSize(5)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(choices)
                    .UseConverter(displaySelector)
                    .HighlightStyle(Style.Plain.Background(Color.Grey))
                );
                _ = Enum.TryParse(option, out MenuOptions menuOptions);
                run = menuOptions switch
                {
                    MenuOptions.AzCliVersion => AzCliVersion.Get(),
                    MenuOptions.AzAccountShow => AzAccountShow.Get(),
                    MenuOptions.AzAccountSubscriptionList => AzAccountSubscriptionList.Get(),
                    MenuOptions.AzResourceGroupList => AzResourceGroupList.Get(),
                    MenuOptions.About => About.Get(),
                    MenuOptions.Exit => false,
                    _ => false
                };
            }
        }
    }
}
