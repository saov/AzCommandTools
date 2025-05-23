﻿namespace SAOV.CLI.AzTools.Modules.ResourceGroup
{
    using SAOV.CLI.AzTools.Components;
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using SAOV.CLI.AzTools.Modules.ResourceGroup.Entities;
    using Spectre.Console;
    using System;

    internal static class ResourceGroup
    {
        public static bool Show()
        {
            List<string> choices = [];
            bool run = false;
            Enum.GetValues<ResourceGroupMenu>().ToList().ForEach(item => { choices.Add(item.ToString()); });
            while (!run)
            {
                ModuleHeader.Show("/ResourceGroup");
                string selectionPromptValue = SelectionPrompt.Show(choices);
                _ = Enum.TryParse(selectionPromptValue, out ResourceGroupMenu azMenu);
                run = azMenu switch
                {
                    ResourceGroupMenu.GetResourceGroupList => GetResourceGroupList(),
                    ResourceGroupMenu.GetResourcesInResouceGroup => GetResourcesInResouceGroup(),
                    ResourceGroupMenu.GetResourcesInSubscription => GetResourcesInSubscription(),
                    ResourceGroupMenu.Exit => true,
                    _ => false
                };
            }
            return run;
        }

        internal static bool GetResourceGroupList()
        {
            string moduleName = "/ResourceGroup/GetResourceGroupList";
            ModuleHeader.Show(moduleName);
            AzResourceGroupListEntity[] azResourceGroupListEntity = GetResourceGroupListData();
            if (azResourceGroupListEntity != null)
            {
                List<KeyValuePair<Markup, Justify>> columns =
                [
                    new(new("Name"), Justify.Left),
                    new(new("Location"), Justify.Center),
                    new(new("ProvisioningState"), Justify.Center)
                ];
                List<List<Markup>> rows = [];
                azResourceGroupListEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                {
                    string stateColor = item.ProvisioningState == "Succeeded" ? "40" : "red";
                    rows.Add([new($"[93]{item.Name}[/]"), new($"[yellow]{item.Location}[/]"), new($"[{stateColor}]{item.ProvisioningState}[/]")]);
                });
                FormatResults.Show<AzResourceGroupListEntity[]>(azResourceGroupListEntity, null, Components.Table.Show(true, $"[aqua]Azure ResourceGroups([40]{rows.Count}[/])[/]", string.Empty, columns, rows), moduleName.Replace("/", "_"));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        internal static bool GetResourcesInResouceGroup()
        {
            string moduleName = "/ResourceGroup/GetResourcesInResouceGroup";
            ModuleHeader.Show(moduleName);
            AzResourceGroupListEntity[] azResourceGroupListEntity = GetResourceGroupListData();
            List<string> choices = [];
            azResourceGroupListEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add($"{item.Name}"); });
            choices.Add(AzCommands.Choise_Cancel);
            if (azResourceGroupListEntity != null)
            {
                string resourceGroup = SelectionPrompt.Show(choices);
                if (resourceGroup != AzCommands.Choise_Cancel)
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string resourceGroupName = azResourceGroupListEntity.Where(t => resourceGroup.Contains(t.Name))
                                .OrderBy(t => t.Name)
                                .Select(t => t.Name)
                                .First();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    AzResourceGroupListEntity[] azResourcesInResourcesListEntity = CommandHelper.Run<AzResourceGroupListEntity[]>(AzCommands.ResourceGroup_ResourcesList, new() { { "@@@ResourceGroup", resourceGroupName } });
                    if (azResourcesInResourcesListEntity != null)
                    {
                        List<KeyValuePair<Markup, Justify>> columns =
                        [
                            new(new("Name"), Justify.Left),
                            new(new("Type"), Justify.Left),
                            new(new("ProvisioningState"), Justify.Center),
                            new(new("Location"), Justify.Left)
                        ];
                        List<List<Markup>> rows = [];
                        azResourcesInResourcesListEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                        {
                            string stateColor = item.ProvisioningState == "Succeeded" ? "40" : "red";
                            rows.Add([new($"[93]{item.Name}[/]"), new($"[purple]{item.Type}[/]"), new($"[{stateColor}]{item.ProvisioningState}[/]"), new($"[yellow]{item.Location}[/]")]);
                        });
                        string titleResult = $"[aqua]Azure Resources In ResourceGroup [yellow]\"[40]{resourceGroupName}[/]\"[/]([40]{rows.Count}[/])[/]";
                        FormatResults.Show<AzResourceGroupListEntity[]>(azResourcesInResourcesListEntity, new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows), moduleName.Replace("/", "_"));
                        AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                        _ = Console.ReadKey();
                        return true;
                    }
                }
            }
            return false;
        }

        internal static bool GetResourcesInSubscription()
        {
            string moduleName = "/ResourceGroup/GetResourcesInSubscription";
            ModuleHeader.Show(moduleName);
            string command = AzCommands.ResourceGroup_ResourcesListInSubscription;
            command = !string.IsNullOrWhiteSpace(AzCommand.AzureQueryFilters) ?
                                                                                command.Replace("@@@AzureQueryFilter", AzCommand.AzureQueryFilters).Replace("@@@AzureQueryFilterPropertyName", "name") :
                                                                                command.Replace("@@@AzureQueryFilter", string.Empty);
            AzResourceGroupListEntity[] azResourcesInResourcesListEntity = CommandHelper.Run<AzResourceGroupListEntity[]>(command, []);
                    if (azResourcesInResourcesListEntity != null)
                    {
                        List<KeyValuePair<Markup, Justify>> columns =
                        [
                            new(new("Name"), Justify.Left),
                            new(new("ResourceGroup"), Justify.Left),
                            new(new("Type"), Justify.Left),
                            new(new("ProvisioningState"), Justify.Center),
                            new(new("Location"), Justify.Left)
                        ];
                        List<List<Markup>> rows = [];
                        azResourcesInResourcesListEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                        {
                            string stateColor = item.ProvisioningState == "Succeeded" ? "40" : "red";
                            rows.Add([new($"[93]{item.Name}[/]"), new($"[yellow]{item.ResourceGroup}[/]"), new($"[purple]{item.Type}[/]"), new($"[{stateColor}]{item.ProvisioningState}[/]"), new($"[yellow]{item.Location}[/]")]);
                        });
                        string titleResult = $"[aqua]Azure Resources In Subscription ([40]{rows.Count}[/])[/]";
                        FormatResults.Show<AzResourceGroupListEntity[]>(azResourcesInResourcesListEntity, new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows), moduleName.Replace("/", "_"));
                        AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                        _ = Console.ReadKey();
                        return true;
                    }
            return false;
        }

        private static AzResourceGroupListEntity[] GetResourceGroupListData()
        {
            string command = AzCommands.ResourceGroup_List;
            command = !string.IsNullOrWhiteSpace(AzCommand.AzureQueryFilters) ?
                                                                                command.Replace("@@@AzureQueryFilter", AzCommand.AzureQueryFilters).Replace("@@@AzureQueryFilterPropertyName", "name") :
                                                                                command.Replace("@@@AzureQueryFilter", string.Empty);
            return CommandHelper.Run<AzResourceGroupListEntity[]>(command, []);
        }
    }
}
