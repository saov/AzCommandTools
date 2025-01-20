﻿namespace SAOV.CLI.AzTools.Modules.KeyVault
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
                    new(new("ResourceGroup"), Justify.Center),
                    new(new("Location"), Justify.Center)
                ];
                List<List<Markup>> rows = [];
                azKeyVaultEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                {
                    rows.Add([new($"[93]{item.Name}[/]"), new($"[40]{item.ResourceGroup}[/]"), new($"[yellow]{item.Location}[/]")]);
                });
                AnsiConsole.Write(Components.Table.Show(true, "[aqua]Azure KeyVaults[/]", string.Empty, columns, rows));
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
            azKeyVaultEntity.ToList().ForEach(item =>
            {
                AzKeyVaultEntity azKeyVaultEntityNetworkRule = CommandHelper.Run<AzKeyVaultEntity>(AzCommands.KeyVault_NetworkRule, new() { { "@@@ResourceGroup", item.ResourceGroup }, { "@@@KeyVault", item.Name } });
                if (azKeyVaultEntityNetworkRule != null)
                {
                    item.Bypass = azKeyVaultEntityNetworkRule.Bypass;
                    item.DefaultAction = azKeyVaultEntityNetworkRule.DefaultAction;
                }
            });
            if (azKeyVaultEntity != null)
            {
                List<KeyValuePair<Markup, Justify>> columns =
                [
                    new(new("Name"), Justify.Left),
                    new(new("ResourceGroup"), Justify.Center),
                    new(new("Bypass"), Justify.Center),
                    new(new("DefaultAction"), Justify.Center),
                    new(new("Location"), Justify.Center)
                ];
                List<List<Markup>> rows = [];
                azKeyVaultEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                {
                    string stateColor = item.DefaultAction == "Allow" ? "red" : "40";
                    rows.Add([new($"[93]{item.Name}[/]"), new($"[40]{item.ResourceGroup}[/]"), new($"[aqua]{item.Bypass}[/]"), new($"[{stateColor}]{item.DefaultAction}[/]"), new($"[yellow]{item.Location}[/]")]);
                });
                AnsiConsole.Write(Components.Table.Show(true, "[aqua]Azure KeyVaults With Network Rules[/]", string.Empty, columns, rows));
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
            choices.Add($"[93](x) ([yellow]Cancel[/])[/]");
            if (azKeyVaultEntity != null)
            {
                bool showChoises = true;
                while (showChoises)
                {
                    AnsiConsole.Clear();
                    ModuleHeader.Show("/KeyVault/GetKeyVaulSecretList");
                    string keyvault = SelectionPrompt.Show(choices);
                    if (keyvault == "[93](x) ([yellow]Cancel[/])[/]")
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
                            new(new("Secrets"), Justify.Left)
                        ];
                        List<List<Markup>> rows = [];
                        azKeyVaultSecretEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                        {
                            rows.Add([new($"[93]{item.Name}[/]")]);
                        });
                        AnsiConsole.Write(Components.Table.Show(true, "[aqua]Azure KeyVaults Secrets[/]", string.Empty, columns, rows));
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
            choices.Add($"[93](x) ([yellow]Cancel[/])[/]");
            if (azKeyVaultEntity != null)
            {
                bool showChoises = true;
                while (showChoises)
                {
                    AnsiConsole.Clear();
                    ModuleHeader.Show("/KeyVault/KeyVaultSecretShow");
                    string keyvault = SelectionPrompt.Show(choices);
                    if (keyvault == "[93](x) ([yellow]Cancel[/])[/]")
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
                        choicestKeyVaulSecret.Add($"[93](x) ([yellow]Cancel[/])[/]");
                        bool showChoisesKeyVaulSecret = true;
                        while (showChoisesKeyVaulSecret)
                        {
                            AnsiConsole.Clear();
                            ModuleHeader.Show("/KeyVault/KeyVaultSecretShow");
                            string keyvaultSecret = SelectionPrompt.Show(choicestKeyVaulSecret);
                            if (keyvaultSecret == "[93](x) ([yellow]Cancel[/])[/]")
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
                                    rows.Add([new($"[93]{azKeyVaultSecretValueEntity.Name}[/]"), new($"[yellow]\"[40]{azKeyVaultSecretValueEntity.Value}[/]\"[/]")]);
                                    AnsiConsole.Write(Components.Table.Show(true, "[aqua]Azure KeyVault Secret Value[/]", string.Empty, columns, rows));
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

        private static AzKeyVaultEntity[] GetKeyVaultListData()
        {
            return CommandHelper.Run<AzKeyVaultEntity[]>(AzCommands.KeyVault_List, []);
        }

        private static AzKeyVaultSecretEntity[] GetKeyVaulSecretListData(string keyVault)
        {
            return CommandHelper.Run<AzKeyVaultSecretEntity[]>(AzCommands.KeyVault_SecretList, new() { { "@@@KeyVault", keyVault } });
        }
    }
}