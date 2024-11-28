﻿namespace SAOV.CommandTools.AzTools.Views
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
            Enum.GetValues<MenuOptionsEnum>().ToList().ForEach(item => { choices.Add(item.ToString()); });
            Func<string, string> displaySelector = str =>
            {
                if (Enum.TryParse(str, out MenuOptionsEnum menuOptions))
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
                    .PageSize(5)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(choices)
                    .UseConverter(displaySelector)
                    .HighlightStyle(Style.Plain.Background(Color.Grey))
                );
                _ = Enum.TryParse(option, out MenuOptionsEnum menuOptions);
                run = menuOptions switch
                {
                    MenuOptionsEnum.AzCliVersion => AzCliVersion.Get(),
                    MenuOptionsEnum.AzAccountShow => AzAccountShow.Get(),
                    MenuOptionsEnum.AzAccountSubscriptionList => AzAccountSubscriptionList.Get(),
                    MenuOptionsEnum.AzResourceGroupList => AzResourceGroupList.Get(),
                    MenuOptionsEnum.About => About.Get(),
                    MenuOptionsEnum.Exit => false,
                    _ => false
                };
            }
        }
    }
}