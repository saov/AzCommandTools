namespace SAOV.CLI.AzTools.Modules.ResourceGroup
{
    using SAOV.CLI.AzTools.Components;
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using SAOV.CLI.AzTools.Modules.ResourceGroup.Entities;
    using Spectre.Console;

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
                    ResourceGroupMenu.Exit => true,
                    _ => false
                };
            }
            return run;
        }

        internal static bool GetResourceGroupList()
        {
            ModuleHeader.Show("/ResourceGroup/GetResourceGroupList");
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
                AnsiConsole.Write(Components.Table.Show(true, $"[aqua]Azure ResourceGroups([40]{rows.Count}[/])[/]", string.Empty, columns, rows));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        internal static bool GetResourcesInResouceGroup()
        {
            ModuleHeader.Show("/ResourceGroup/GetResourcesInResouceGroup");
            AzResourceGroupListEntity[] azResourceGroupListEntity = GetResourceGroupListData();
            List<string> choices = [];
            azResourceGroupListEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add($"{item.Name}"); });
            choices.Add($"[93](x) [yellow]Cancel[/][/]");
            if (azResourceGroupListEntity != null)
            {
                string resourceGroup = SelectionPrompt.Show(choices);
                if (resourceGroup != "[93](x) [yellow]Cancel[/][/]")
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
                        AnsiConsole.Write(Components.Table.Show(true, $"[aqua]Azure Resources In ResourceGroup([40]{rows.Count}[/])[/]", string.Empty, columns, rows));
                        AnsiConsole.WriteLine();
                        AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                        _ = Console.ReadKey();
                        return true;
                    }
                }
            }
            return false;
        }

        private static AzResourceGroupListEntity[] GetResourceGroupListData()
        {
            string command = AzCommands.ResourceGroup_List;
            command = !string.IsNullOrWhiteSpace(Program.AzureQueryFilters) ?
                                                                                command.Replace("@@@AzureQueryFilter", Program.AzureQueryFilters).Replace("@@@AzureQueryFilterPropertyName", "name") :
                                                                                command.Replace("@@@AzureQueryFilter", "");
            return CommandHelper.Run<AzResourceGroupListEntity[]>(command, []);
        }
    }
}
