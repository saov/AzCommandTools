namespace SAOV.CLI.AzTools.Modules.KeyVault
{
    using SAOV.CLI.AzTools.Components;
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using SAOV.CLI.AzTools.Modules.KeyVault.Entities;
    using Spectre.Console;

    internal static class KeyVault
    {
        public static bool Show()
        {
            List<string> choices = [];
            bool run = false;
            Enum.GetValues<KeyVaultMenu>().ToList().ForEach(item => { choices.Add(item.ToString()); });
            while (!run)
            {
                ModuleHeader.Show("/KeyVault");
                string selectionPromptValue = SelectionPrompt.Show(choices);
                _ = Enum.TryParse(selectionPromptValue, out KeyVaultMenu mainMenuOption);
                run = mainMenuOption switch
                {
                    KeyVaultMenu.GetKeyVaultList => GetKeyVaultList(),
                    KeyVaultMenu.GetKeyVaultListWithNetworkRules => GetKeyVaultListWithNetworkRules(),
                    KeyVaultMenu.GetKeyVaulSecretList => GetKeyVaulSecretList(),
                    KeyVaultMenu.KeyVaultSecretShow => KeyVaultSecretShow(),
                    KeyVaultMenu.KeyVaultAllSecretShow => KeyVaultAllSecretShow(),
                    KeyVaultMenu.Exit => true,
                    _ => false
                };
            }
            return run;
        }

        private static bool GetKeyVaultList()
        {
            ModuleHeader.Show("/KeyVault/GetKeyVaultList");
            AzKeyVaultEntity[] azKeyVaultEntity = GetKeyVaultListData();
            if (azKeyVaultEntity != null)
            {
                List<KeyValuePair<Markup, Justify>> columns =
                [
                    new(new("Name"), Justify.Left),
                    new(new("ResourceGroup"), Justify.Left),
                    new(new("Location"), Justify.Center)
                ];
                List<List<Markup>> rows = [];
                azKeyVaultEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                {
                    rows.Add([new($"[93]{item.Name}[/]"), new($"[40]{item.ResourceGroup}[/]"), new($"[yellow]{item.Location}[/]")]);
                });
                string titleResult = $"[aqua]Azure KeyVaults([40]{rows.Count}[/])[/]";
                FormatResults.Show<AzKeyVaultEntity[]>(azKeyVaultEntity, new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        private static bool GetKeyVaultListWithNetworkRules()
        {
            ModuleHeader.Show("/KeyVault/GetKeyVaultListWithNetworkRules");
            AzKeyVaultEntity[] azKeyVaultEntity = GetKeyVaultListData();
            if (azKeyVaultEntity != null)
            {
                if (azKeyVaultEntity.Length == 0)
                {
                    AnsiConsole.Write(new Markup("[red]No Resources found.[/]"));
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine();
                    AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                    _ = Console.ReadKey();
                    return true;
                }
                List<Dictionary<string, string>> itemsWithParamentersCommand = [];
                azKeyVaultEntity.ToList().ForEach(item => { itemsWithParamentersCommand.Add(new() { { "@@@ResourceGroup", item.ResourceGroup }, { "@@@KeyVault", item.Name } }); });
                List<AzKeyVaultEntity> networkRules = Components.Progress.Show<AzKeyVaultEntity>("Get NetworkRules for KeyVaults", AzCommands.KeyVault_NetworkRule, itemsWithParamentersCommand);
                for (int i = 0; i < azKeyVaultEntity.Length; i++)
                {
                    azKeyVaultEntity[i].NetworkRuleSetBypass = networkRules[i]?.NetworkRuleSetBypass;
                    azKeyVaultEntity[i].NetworkRuleSetDefaultAction = networkRules[i]?.NetworkRuleSetDefaultAction;
                }
                List<KeyValuePair<Markup, Justify>> columns =
                [
                    new(new("Name"), Justify.Left),
                    new(new("ResourceGroup"), Justify.Left),
                    new(new("Bypass"), Justify.Center),
                    new(new("DefaultAction"), Justify.Center),
                    new(new("Location"), Justify.Center)
                ];
                List<List<Markup>> rows = [];
                azKeyVaultEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                {
                    string stateColor = item.NetworkRuleSetDefaultAction == "Allow" ? "red" : "40";
                    rows.Add([new($"[93]{item.Name}[/]"), new($"[40]{item.ResourceGroup}[/]"), new($"[aqua]{item.NetworkRuleSetBypass}[/]"), new($"[{stateColor}]{item.NetworkRuleSetDefaultAction}[/]"), new($"[yellow]{item.Location}[/]")]);
                });
                string titleResult = $"[aqua]Azure KeyVaults With Network Rules([40]{rows.Count}[/])[/]";
                FormatResults.Show<AzKeyVaultEntity[]>(azKeyVaultEntity, new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        private static bool GetKeyVaulSecretList()
        {
            AzKeyVaultEntity[] azKeyVaultEntity = GetKeyVaultListData();
            List<string> choices = [];
            azKeyVaultEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add($"{item.Name}"); });
            choices.Add($"[93](x) [yellow]Cancel[/][/]");
            if (azKeyVaultEntity != null)
            {
                bool showChoises = true;
                while (showChoises)
                {
                    AnsiConsole.Clear();
                    ModuleHeader.Show("/KeyVault/GetKeyVaulSecretList");
                    string keyvault = SelectionPrompt.Show(choices);
                    if (keyvault == "[93](x) [yellow]Cancel[/][/]")
                    {
                        showChoises = false;
                        return true;
                    }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string keyvaultName = azKeyVaultEntity.Where(t => keyvault.Contains(t.Name))
                                .OrderBy(t => t.Name)
                                .Select(t => t.Name)
                                .First();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    AzKeyVaultSecretEntity[] azKeyVaultSecretEntity = GetKeyVaulSecretListData(keyvaultName);
                    if (azKeyVaultSecretEntity != null)
                    {
                        List<KeyValuePair<Markup, Justify>> columns =
                        [
                            new(new("Secrets"), Justify.Left),
                            new(new("Value"), Justify.Left)
                        ];
                        List<List<Markup>> rows = [];
                        azKeyVaultSecretEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                        {
                            rows.Add([new($"[93]{item.Name}[/]"), new(string.Empty)]);
                        });
                        string titleResult = $"[aqua]Azure Secrets of {Environment.NewLine}[yellow]\"[40]{keyvaultName}[/]\"[/]([40]{rows.Count}[/])[/]";
                        FormatResults.Show<AzKeyVaultSecretEntity[]>(azKeyVaultSecretEntity, new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows));
                        AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                        _ = Console.ReadKey();
                    }
                }
                return true;
            }
            return false;
        }

        private static bool KeyVaultSecretShow()
        {
            AzKeyVaultEntity[] azKeyVaultEntity = GetKeyVaultListData();
            List<string> choices = [];
            azKeyVaultEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add($"{item.Name}"); });
            choices.Add($"[93](x) [yellow]Cancel[/][/]");
            if (azKeyVaultEntity != null)
            {
                bool showChoises = true;
                while (showChoises)
                {
                    AnsiConsole.Clear();
                    ModuleHeader.Show("/KeyVault/KeyVaultSecretShow");
                    string keyvault = SelectionPrompt.Show(choices);
                    if (keyvault == "[93](x) [yellow]Cancel[/][/]")
                    {
                        showChoises = false;
                        return true;
                    }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string keyvaultName = azKeyVaultEntity.Where(t => keyvault.Contains(t.Name))
                                .OrderBy(t => t.Name)
                                .Select(t => t.Name)
                                .First();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    AzKeyVaultSecretEntity[] azKeyVaultSecretEntity = GetKeyVaulSecretListData(keyvaultName);
                    if (azKeyVaultSecretEntity != null)
                    {
                        List<string> choicestKeyVaulSecret = [];
                        azKeyVaultSecretEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choicestKeyVaulSecret.Add($"{item.Name}"); });
                        choicestKeyVaulSecret.Add($"[93](x) [yellow]Cancel[/][/]");
                        bool showChoisesKeyVaulSecret = true;
                        while (showChoisesKeyVaulSecret)
                        {
                            AnsiConsole.Clear();
                            ModuleHeader.Show("/KeyVault/KeyVaultSecretShow");
                            string keyvaultSecret = SelectionPrompt.Show(choicestKeyVaulSecret, title: $"[yellow]Select an option of [yellow]\"[40]{keyvault}[/]\"[/] ([40]{choicestKeyVaulSecret.Count - 1}[/]).[/]");
                            if (keyvaultSecret == "[93](x) [yellow]Cancel[/][/]")
                            {
                                showChoisesKeyVaulSecret = false;
                            }
                            else
                            {
                                AzKeyVaultSecretEntity azKeyVaultSecretValueEntity = CommandHelper.Run<AzKeyVaultSecretEntity>(AzCommands.KeyVault_SecretShow, new() { { "@@@KeyVault", keyvault }, { "@@@SecretName", keyvaultSecret } });
                                if (azKeyVaultSecretValueEntity != null)
                                {
                                    List<KeyValuePair<Markup, Justify>> columns =
                                    [
                                        new(new("Secret"), Justify.Left),
                                        new(new("Value"), Justify.Left)
                                    ];
                                    List<List<Markup>> rows = [];
                                    rows.Add([new($"[93]{azKeyVaultSecretValueEntity.Name}[/]"), new($"[yellow]\"[40]{Markup.Escape(azKeyVaultSecretValueEntity.Value)}[/]\"[/]")]);
                                    string titleResult = $"[aqua]Azure Secret [yellow]\"[40]{keyvaultSecret}[/]\"[/] Value of [yellow]\"[40]{keyvault}[/]\"[/]([40]{rows.Count}[/])[/]";
                                    FormatResults.Show<AzKeyVaultSecretEntity>(azKeyVaultSecretValueEntity, new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows));
                                    AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                                    _ = Console.ReadKey();
                                }
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private static bool KeyVaultAllSecretShow()
        {
            AzKeyVaultEntity[] azKeyVaultEntity = GetKeyVaultListData();
            List<string> choices = [];
            azKeyVaultEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add($"{item.Name}"); });
            choices.Add($"[93](x) [yellow]Cancel[/][/]");
            if (azKeyVaultEntity != null)
            {
                bool showChoises = true;
                while (showChoises)
                {
                    AnsiConsole.Clear();
                    ModuleHeader.Show("/KeyVault/KeyVaultAllSecretShow");
                    string keyvault = SelectionPrompt.Show(choices);
                    if (keyvault == "[93](x) [yellow]Cancel[/][/]")
                    {
                        showChoises = false;
                        return true;
                    }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string keyvaultName = azKeyVaultEntity.Where(t => keyvault.Contains(t.Name))
                                .OrderBy(t => t.Name)
                                .Select(t => t.Name)
                                .First();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    AzKeyVaultSecretEntity[] azKeyVaultSecretEntity = GetKeyVaulSecretListData(keyvaultName);
                    if (azKeyVaultSecretEntity != null)
                    {
                        List<Dictionary<string, string>> itemsWithParamentersCommand = [];
                        azKeyVaultSecretEntity.ToList().ForEach(item => { itemsWithParamentersCommand.Add(new() { { "@@@KeyVault", keyvault }, { "@@@SecretName", item.Name } }); });
                        List<AzKeyVaultSecretEntity> secrets = Components.Progress.Show<AzKeyVaultSecretEntity>("Get Secrets for KeyVault", AzCommands.KeyVault_SecretShow, itemsWithParamentersCommand);
                        List<KeyValuePair<Markup, Justify>> columns =
                        [
                            new(new("Secret"), Justify.Left),
                            new(new("Value"), Justify.Left)
                        ];
                        List<List<Markup>> rows = [];
                        secrets.ToList().ForEach(item =>
                        {
                            rows.Add([new($"[93]{item.Name}[/]"), new($"[yellow]\"[40]{Markup.Escape(item.Value)}[/]\"[/]")]);
                        });
                        string titleResult = $"[aqua]Azure All Secrets Values of KeyVault [yellow]\"[40]{keyvaultName}[/]\"[/]([40]{rows.Count}[/])[/]";
                        FormatResults.Show<List<AzKeyVaultSecretEntity>>(secrets, new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows));
                        AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                        _ = Console.ReadKey();
                    }
                }
                return true;
            }
            return false;
        }

        private static AzKeyVaultEntity[] GetKeyVaultListData()
        {
            string command = AzCommands.KeyVault_List;
            command = !string.IsNullOrWhiteSpace(Program.AzureQueryFilters) ?
                                                                                command.Replace("@@@AzureQueryFilter", Program.AzureQueryFilters).Replace("@@@AzureQueryFilterPropertyName", "name") :
                                                                                command.Replace("@@@AzureQueryFilter", "");
            return CommandHelper.Run<AzKeyVaultEntity[]>(command, []);
        }

        private static AzKeyVaultSecretEntity[] GetKeyVaulSecretListData(string keyVault)
        {
            return CommandHelper.Run<AzKeyVaultSecretEntity[]>(AzCommands.KeyVault_SecretList, new() { { "@@@KeyVault", keyVault } });
        }
    }
}
