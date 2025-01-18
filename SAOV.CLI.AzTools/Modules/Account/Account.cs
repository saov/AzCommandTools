using SAOV.CLI.AzTools.Components;
using SAOV.CLI.AzTools.Helpers;
using SAOV.CLI.AzTools.Modules.Account.Entities;
using Spectre.Console;

namespace SAOV.CLI.AzTools.Modules.Account
{
    internal class Account
    {
        public static bool Show()
        {
            List<string> choices = [];
            bool run = false;
            Enum.GetValues<AccountMenu>().ToList().ForEach(item => { choices.Add(item.ToString()); });
            while (!run)
            {
                ModuleHeader.Show("/Account");
                string selectionPromptValue = Components.SelectionPrompt.Show(choices);
                _ = Enum.TryParse(selectionPromptValue, out AccountMenu mainMenuOption);
                run = mainMenuOption switch
                {
                    AccountMenu.LogInByServicePrincipal => LogInByServicePrincipal(),
                    AccountMenu.LogOut => LogOut(),
                    AccountMenu.Exit => true,
                    _ => false
                };
            }
            return run;
        }

        private static bool LogInByServicePrincipal()
        {
            ModuleHeader.Show("/Account/LogInByServicePrincipal");
            AzLoginEntity[] azLoginEntity = [];
            string tenantId = TextPrompt.Show("[93]Provide the [40]tenant id[/] :[/]", (value) => { return !string.IsNullOrWhiteSpace(value); }, "Field is required.", true);
            string serviceIdentity = TextPrompt.Show("[93]Provide the [40]service identity[/] :[/]", (value) => { return !string.IsNullOrWhiteSpace(value); }, "Field is required.", true);
            string secret = TextPrompt.Show("[93]Provide the [40]secret[/] :[/]", (value) => { return !string.IsNullOrWhiteSpace(value); }, "Field is required.", true, true);
            if (string.IsNullOrWhiteSpace(tenantId) || string.IsNullOrWhiteSpace(serviceIdentity) || string.IsNullOrWhiteSpace(secret))
            {
                AnsiConsole.WriteLine();
                AnsiConsole.Write(new Markup("[red]Operation canceled by the user.[/]"));
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
            }
            else
            {
                azLoginEntity = CommandHelper.Run<AzLoginEntity[]>(AzCommands.Account_Login, new() { { "@@@User", serviceIdentity }, { "@@@Tenant", tenantId }, { "@@@Secret", secret } });
                if (azLoginEntity != null)
                {
                    List<KeyValuePair<Markup, Justify>> columns =
                    [
                        new(new("Name"), Justify.Left),
                        new(new("Id"), Justify.Center),
                        new(new("State"), Justify.Center),
                        new(new("IsDefault"), Justify.Center),
                    ];
                    List<List<Markup>> rows = [];
                    azLoginEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                    {
                        string stateColor = item.State == "Enabled" ? "40" : "red";
                        string isDefault = item.IsDefault ? "40" : "red";
                        rows.Add(
                                [
                                    new($"[93]{item.Name}[/]"),
                                    new($"[yellow]{item.Id}[/]"),
                                    new($"[{stateColor}]{item.State}[/]"),
                                    new($"[{isDefault}]{item.IsDefault}[/]")
                                ]
                         );
                    });
                    Components.Table.Show("[aqua]Azure Subscriptions[/]", columns, rows);
                    AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                    _ = Console.ReadKey();
                    return true;
                }
            }
            return false;
        }

        private static bool LogOut()
        {
            ModuleHeader.Show("/Account/LogOut");
            return CommandHelper.Run(AzCommands.Account_Logout, []);
        }
    }
}
