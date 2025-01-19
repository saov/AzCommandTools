namespace SAOV.CommandTools.AzTools.Views
{
    using SAOV.CommandTools.AzTools.Commands.AzAccountShow;
    using SAOV.CommandTools.AzTools.Commands.AzAccountSubscriptionList;
    using SAOV.CommandTools.AzTools.Commands.AzCliVersion;
    using SAOV.CommandTools.AzTools.Commands.AzKeyVault;
    using SAOV.CommandTools.AzTools.Commands.AzLogin;
    using SAOV.CommandTools.AzTools.Commands.AzLogOut;
    using SAOV.CommandTools.AzTools.Commands.AzResourceGroupList;
    using Spectre.Console;

    internal static class Menu
    {
        internal static void Get()
        {
            List<string> choices = [];
            bool run = true;
            Enum.GetValues<MenuOptionsRoot>().ToList().ForEach(item => { choices.Add(item.ToString()); });
            Func<string, string> displaySelector = str =>
            {
                if (Enum.TryParse(str, out MenuOptionsRoot menuOptions))
                {
                    return $"[93]{menuOptions.ToName()}[/]";
                }
                throw new NotImplementedException();
            };
            while (run)
            {
                string option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[yellow]Select a [40]query.[/][/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(choices)
                    .UseConverter(displaySelector)
                    .HighlightStyle(Style.Plain.Background(Color.Grey))
                    .EnableSearch()
                );
                _ = Enum.TryParse(option, out MenuOptionsRoot menuOptions);
                run = menuOptions switch
                {
                    MenuOptionsRoot.AzLogin => AzLogin.Get(),
                    MenuOptionsRoot.AzCliVersion => AzCliVersion.Get(),
                    MenuOptionsRoot.AzAccountShow => AzAccountShow.Get(),
                    MenuOptionsRoot.AzAccountSubscriptionList => AzAccountSubscriptionList.Get(),
                    MenuOptionsRoot.AzResourceGroupList => AzResourceGroupList.Get(),
                    MenuOptionsRoot.AzKeyVaultList => AzKeyVaultList.Get(),
                    MenuOptionsRoot.About => About.Get(),
                    MenuOptionsRoot.AzLogOut => AzLogOut.Get(),
                    MenuOptionsRoot.Exit => false,
                    _ => false
                };
            }
        }
    }
}
