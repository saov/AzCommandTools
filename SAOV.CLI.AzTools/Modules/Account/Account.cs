﻿namespace SAOV.CLI.AzTools.Modules.Account
{
    using SAOV.CLI.AzTools.Components;
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using SAOV.CLI.AzTools.Modules.Account.Entities;
    using Spectre.Console;

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
                string selectionPromptValue = SelectionPrompt.Show(choices);
                _ = Enum.TryParse(selectionPromptValue, out AccountMenu mainMenuOption);
                run = mainMenuOption switch
                {
                    AccountMenu.LogIn => LogIn(),
                    AccountMenu.LogOut => LogOut(),
                    AccountMenu.ShowCurrentSubscription => ShowCurrentSubscription(),
                    AccountMenu.GetSubscriptionList => GetSubscriptionList(),
                    AccountMenu.SetSubscription => SetSubscription(),
                    AccountMenu.Exit => true,
                    _ => false
                };
            }
            return run;
        }

        private static bool LogIn()
        {
            string moduleName = "/Account/LogIn";
            ModuleHeader.Show(moduleName);
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
                    AnsiConsole.WriteLine();
                    AnsiConsole.Write(Components.Table.Show(true, $"[aqua]Azure Subscriptions([40]{rows.Count}[/])[/]", string.Empty, columns, rows));
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
            if (TextPrompt.Show("Do you want to continue?"))
            {
                AnsiConsole.WriteLine();
                return CommandHelper.Run(AzCommands.Account_Logout, []);
            }
            return true;
        }

        private static bool ShowCurrentSubscription()
        {
            string moduleName = "/Account/ShowCurrentSubscription";
            ModuleHeader.Show(moduleName);
            AzAccountShowEntity azAccountShowEntity = CommandHelper.Run<AzAccountShowEntity>(AzCommands.Account_Show, []);
            if (azAccountShowEntity != null)
            {
                List<KeyValuePair<Markup, Justify>> columns =
                [
                    new(new("Property Name"), Justify.Right),
                    new(new("Property Value"), Justify.Left)
                ];
                List<List<Markup>> rows = [];
                rows.Add([new($"[93]Id[/]"), new($"[40]{azAccountShowEntity.Id}[/]")]);
                rows.Add([new($"[93]Name[/]"), new($"[40]{azAccountShowEntity.Name}[/]")]);
                rows.Add([new($"[93]State[/]"), new($"[40]{azAccountShowEntity.State}[/]")]);
                rows.Add([new($"[93]TenantDisplayName[/]"), new($"[40]{azAccountShowEntity.TenantDisplayName}[/]")]);
                rows.Add([new($"[93]TenantId[/]"), new($"[40]{azAccountShowEntity.TenantId}[/]")]);
                rows.Add([new($"[93]UserName[/]"), new($"[40]{azAccountShowEntity.UserName}[/]")]);
                rows.Add([new($"[93]UserType[/]"), new($"[40]{azAccountShowEntity.UserType}[/]")]);
                string titleResult = $"[aqua]Azure Account Info([40]{rows.Count}[/])[/]";
                FormatResults.Show<AzAccountShowEntity>(azAccountShowEntity, new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows), moduleName.Replace("/", "_"));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        private static bool GetSubscriptionList()
        {
            string moduleName = "/Account/GetSubscriptionList";
            ModuleHeader.Show(moduleName);
            AzAccountSubscriptionListEntity[] azAccountSubscriptionListEntity = GetSubscriptionListData();
            if (azAccountSubscriptionListEntity != null)
            {
                List<KeyValuePair<Markup, Justify>> columns =
                [
                    new(new("Name"), Justify.Left),
                    new(new("SubscriptionId"), Justify.Left),
                    new(new("State"), Justify.Center)
                ];
                List<List<Markup>> rows = [];
                azAccountSubscriptionListEntity.OrderBy(t => t.DisplayName).ToList().ForEach(item =>
                {
                    string stateColor = item.State == "Enabled" ? "40" : "red";
                    rows.Add([new($"[93]{item.DisplayName}[/]"), new($"[yellow]{item.SubscriptionId}[/]"), new($"[{stateColor}]{item.State}[/]")]);
                });
                string titleResult = $"[aqua]Azure Subscriptions([40]{rows.Count}[/])[/]";
                FormatResults.Show<AzAccountSubscriptionListEntity[]>(azAccountSubscriptionListEntity, new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows), moduleName.Replace("/", "_"));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        private static bool SetSubscription()
        {
            ModuleHeader.Show("/Account/SetSubscription");
            List<AzAccountSubscriptionListEntity> azAccountSubscriptionListEntity = GetSubscriptionListData().ToList().Where(t => t.State == "Enabled").OrderBy(t => t.DisplayName).ToList();
            List<string> choices = [];
            azAccountSubscriptionListEntity.ForEach(item => { choices.Add($"{item.DisplayName}({item.SubscriptionId})"); });
            choices.Add(AzCommands.Choise_Cancel);
            if (azAccountSubscriptionListEntity != null)
            {
                string subscription = SelectionPrompt.Show(choices);
                if (subscription != AzCommands.Choise_Cancel)
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string subscriptionId = azAccountSubscriptionListEntity.Where(t => t.State == "Enabled" && subscription.Contains(t.SubscriptionId))
                                .OrderBy(t => t.DisplayName)
                                .Select(t => t.SubscriptionId)
                                .First();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    return CommandHelper.Run(AzCommands.Account_SetSubscription, new() { { "@@@SubscriptionId", subscriptionId } });
                }
            }
            return false;
        }

        private static AzAccountSubscriptionListEntity[] GetSubscriptionListData()
        {
            return CommandHelper.Run<AzAccountSubscriptionListEntity[]>(AzCommands.Account_SubscriptionList, []);
        }
    }
}
