namespace SAOV.CLI.AzTools.Modules.ACR
{
    using SAOV.CLI.AzTools.Components;
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using SAOV.CLI.AzTools.Modules.ACR.Entities;
    using SAOV.CLI.AzTools.Modules.ResourceGroup.Entities;
    using Spectre.Console;
    using System;
    using System.Collections.Generic;

    internal static class ACR
    {
        public static bool Show()
        {
            List<string> choices = [];
            bool run = false;
            Enum.GetValues<ACRMenu>().ToList().ForEach(item => { choices.Add(item.ToString()); });
            while (!run)
            {
                ModuleHeader.Show("/ACR");
                string selectionPromptValue = SelectionPrompt.Show(choices);
                _ = Enum.TryParse(selectionPromptValue, out ACRMenu azMenu);
                run = azMenu switch
                {
                    ACRMenu.GetACRList => GetACRList(),
                    ACRMenu.GetACRRepositories => GetACRRepositories(),
                    ACRMenu.GetACRRepositoryTags => GetACRRepositoryTags(),
                    ACRMenu.Exit => true,
                    _ => false
                };
            }
            return run;
        }

        internal static bool GetACRList()
        {
            ModuleHeader.Show("/ACR/GetACRList");
            AzResourceGroupListEntity[] azResourceGroupListEntity = GetResourceGroupListData();
            if (azResourceGroupListEntity != null)
            {
                List<Dictionary<string, string>> itemsWithParamentersCommand = [];
                azResourceGroupListEntity.ToList().ForEach(item => { itemsWithParamentersCommand.Add(new() { { "@@@ACRName", item.Name } }); });
                List<ACREntity> aCRs = Components.Progress.Show<ACREntity>("Get ACRs", AzCommands.ACR_Show, itemsWithParamentersCommand);
                List<KeyValuePair<Markup, Justify>> columns =
                [
                    new(new("Name"), Justify.Left),
                    new(new(""), Justify.Left),
                    new(new(""), Justify.Left)
                ];
                List<List<Markup>> rows = [];
                aCRs.ToList().ForEach(item =>
                {
                    string colorProvisioningState = item.ProvisioningState == "Succeeded" ? "40" : "red";
                    string colorPublicNetworkAccess = item.PublicNetworkAccess == "Enabled" ? "red" : "40";
                    string colorZoneRedundancy = item.ZoneRedundancy == "Disabled" ? "red" : "40";
                    rows.Add([new($"[93]{item.Name}[/]"), new($"[gray]DataEndpointHostNames[/] [yellow]:[/] {item.DataEndpointHostNames}\n" +
                                                              $"[gray]Location[/] [yellow]:[/] {item.Location}\n" +
                                                              $"[gray]LoginServer[/] [yellow]:[/] {item.LoginServer}\n" +
                                                              $"[gray]NetworkRuleBypassOptions[/] [yellow]:[/] {item.NetworkRuleBypassOptions}\n" +
                                                              $"[gray]NetworkRuleSetDefaultAction[/] [yellow]:[/] {item.NetworkRuleSetDefaultAction}\n" +
                                                              $"[gray]ProvisioningState[/] [yellow]:[/] [{colorProvisioningState}]{item.ProvisioningState}[/]"),
                                                          new($"[gray]PublicNetworkAccess[/] [yellow]:[/] [{colorPublicNetworkAccess}]{item.PublicNetworkAccess}[/]\n" +
                                                              $"[gray]ResourceGroup[/] [yellow]:[/] {item.ResourceGroup}\n" +
                                                              $"[gray]SkuName[/] [yellow]:[/] {item.SkuName}\n" +
                                                              $"[gray]SkuTier[/] [yellow]:[/] {item.SkuTier}\n" +
                                                              $"[gray]Type[/] [yellow]:[/] {item.Type}\n" +
                                                              $"[gray]ZoneRedundancy[/] [yellow]:[/] [{colorZoneRedundancy}]{item.ZoneRedundancy}[/]")]);
                });
                string titleResult = $"[aqua]Azure ACRs([40]{rows.Count}[/])[/]";
                FormatResults.Show<List<ACREntity>>(aCRs, new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows), "ACR");
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
            }
            return false;
        }

        internal static bool GetACRRepositories()
        {
            AzResourceGroupListEntity[] azResourceGroupListEntity = GetResourceGroupListData();
            List<string> choices = [];
            azResourceGroupListEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add($"{item.Name}"); });
            choices.Add($"[93](x) [yellow]Cancel[/][/]");
            if (azResourceGroupListEntity != null)
            {
                bool showChoises = true;
                while (showChoises)
                {
                    AnsiConsole.Clear();
                    string moduleName = "/ACR/GetACRRepositories";
                    ModuleHeader.Show(moduleName);
                    string acr = SelectionPrompt.Show(choices);
                    if (acr == "[93](x) [yellow]Cancel[/][/]")
                    {
                        showChoises = false;
                        return true;
                    }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string aCRName = azResourceGroupListEntity.Where(t => acr.Contains(t.Name))
                                .OrderBy(t => t.Name)
                                .Select(t => t.Name)
                                .First();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    string[] repositories = GetACRRepositoriesData(aCRName);
                    List<KeyValuePair<Markup, Justify>> columns =
                    [
                        new(new("Repositories"), Justify.Left)
                    ];
                    List<List<Markup>> rows = [];
                    repositories.ToList().ForEach(item =>
                    {
                        rows.Add([new($"[93]{item}[/]")]);
                    });
                    string titleResult = $"[aqua]Azure ACR([40]{aCRName}[/]) Repositories([40]{repositories.Length}[/])[/]";
                    FormatResults.Show<List<string>>(repositories.ToList(), new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows), "ACR Repositories");
                    AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                    _ = Console.ReadKey();
                }
            }
            return false;
        }

        internal static bool GetACRRepositoryTags()
        {
            AzResourceGroupListEntity[] azResourceGroupListEntity = GetResourceGroupListData();
            List<string> choices = [];
            azResourceGroupListEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add($"{item.Name}"); });
            choices.Add($"[93](x) [yellow]Cancel[/][/]");
            if (azResourceGroupListEntity != null)
            {
                bool showChoises = true;
                while (showChoises)
                {
                    AnsiConsole.Clear();
                    string moduleName = "/ACR/GetACRRepositoryTags";
                    ModuleHeader.Show(moduleName);
                    string acr = SelectionPrompt.Show(choices);
                    if (acr == "[93](x) [yellow]Cancel[/][/]")
                    {
                        showChoises = false;
                        return true;
                    }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string aCRName = azResourceGroupListEntity.Where(t => acr.Contains(t.Name))
                                .OrderBy(t => t.Name)
                                .Select(t => t.Name)
                                .First();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    string[] repositories = GetACRRepositoriesData(aCRName);
                    List<string> choicesRepositories = [];
                    repositories.OrderBy(t => t).ToList().ForEach(item => { choicesRepositories.Add($"{item}"); });
                    choicesRepositories.Add($"[93](x) [yellow]Cancel[/][/]");
                    bool showChoisesRepositories = true;
                    while (showChoisesRepositories)
                    {
                        AnsiConsole.Clear();
                        ModuleHeader.Show(moduleName);
                        string repository = SelectionPrompt.Show(choicesRepositories);
                        if (repository == "[93](x) [yellow]Cancel[/][/]")
                        {
                            showChoises = false;
                            return true;
                        }
                        string[] repositoryTags = CommandHelper.Run<string[]>(AzCommands.ACR_RepositoryTags, new() { { "@@@ACRName", aCRName }, { "@@@ACRRepositoryName", repository } });
                        List<KeyValuePair<Markup, Justify>> columns =
                        [
                            new(new("Tags"), Justify.Left)
                        ];
                        List<List<Markup>> rows = [];
                        repositoryTags.ToList().ForEach(item =>
                        {
                            rows.Add([new($"[93]{item}[/]")]);
                        });
                        string titleResult = $"[aqua]Azure ACR ([40]{aCRName}[/]) Repository ([40]{repository}[/]) Tags([40]{repositoryTags.Length}[/])[/]";
                        FormatResults.Show<List<string>>(repositoryTags.ToList(), new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows), "ACR Repository Tags");
                        AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                        _ = Console.ReadKey();
                    }
                }
            }
            return false;
        }

        private static AzResourceGroupListEntity[] GetResourceGroupListData()
        {
            string command = AzCommands.ResourceGroup_ResourcesListInSubscriptionFilterType.Replace("@@@ResourceType", "Microsoft.ContainerRegistry/registries");
            command = !string.IsNullOrWhiteSpace(AzCommand.AzureQueryFilters) ?
                                                                                command.Replace("@@@AzureQueryFilter", AzCommand.AzureQueryFilters).Replace("@@@AzureQueryFilterPropertyName", "name") :
                                                                                command.Replace("@@@AzureQueryFilter", "");
            command = command.Replace("[ && ", "[?");
            return CommandHelper.Run<AzResourceGroupListEntity[]>(command, []);
        }

        private static string[] GetACRRepositoriesData(string aCRName)
        {
            return CommandHelper.Run<string[]>(AzCommands.ACR_Repositories, new() { { "@@@ACRName", aCRName } });
        }

    }
}
