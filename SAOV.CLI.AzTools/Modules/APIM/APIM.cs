namespace SAOV.CLI.AzTools.Modules.APIM
{
    using SAOV.CLI.AzTools.Components;
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using SAOV.CLI.AzTools.Modules.APIM.Entities;
    using SAOV.CLI.AzTools.Modules.ResourceGroup.Entities;
    using Spectre.Console;
    using System;
    using System.Collections.Generic;

    internal static class APIM
    {
        public static bool Show()
        {
            List<string> choices = [];
            bool run = false;
            Enum.GetValues<APIMMenu>().ToList().ForEach(item => { choices.Add(item.ToString()); });
            while (!run)
            {
                ModuleHeader.Show("/APIM");
                string selectionPromptValue = SelectionPrompt.Show(choices);
                _ = Enum.TryParse(selectionPromptValue, out APIMMenu azMenu);
                run = azMenu switch
                {
                    APIMMenu.GetAPIMList => GetAPIMList(),
                    APIMMenu.GetAPIMListWithOperations => GetAPIMListWithOperations(),
                    APIMMenu.Exit => true,
                    _ => false
                };
            }
            return run;
        }

        internal static bool GetAPIMList()
        {
            AzResourceGroupListEntity[] azResourcesInResourcesListEntity = GetResourceGroupListData();
            List<string> choices = [];
            azResourcesInResourcesListEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add($"{item.Name} ({item.ResourceGroup})"); });
            choices.Add(AzCommands.Choise_Cancel);
            if (azResourcesInResourcesListEntity != null)
            {
                bool showChoises = true;
                while (showChoises)
                {
                    AnsiConsole.Clear();
                    string moduleName = "/APIM/GetAPIMList";
                    ModuleHeader.Show(moduleName);
                    string apim = SelectionPrompt.Show(choices);
                    if (apim == AzCommands.Choise_Cancel)
                    {
                        showChoises = false;
                        return true;
                    }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                    (string Name, string ResourceGroup) apimName = azResourcesInResourcesListEntity.Where(t => apim.Contains($"{t.Name} ({t.ResourceGroup})"))
                    .OrderBy(t => t.Name)
                      .Select(t => (t.Name, t.ResourceGroup))
                      .First();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    APIMEntity[] aPIMEntity = AzAPIManagementList(apimName);
                    if (aPIMEntity != null)
                    {
                        List<KeyValuePair<Markup, Justify>> columns =
                        [
                            new(new("Name"), Justify.Left),
                            new(new("DisplayName"), Justify.Left),
                            new(new("Path"), Justify.Left),
                            new(new("ResourceGroup"), Justify.Left),
                            new(new("ServiceUrl"), Justify.Left),
                            new(new("SubscriptionRequired"), Justify.Center)
                        ];
                        List<List<Markup>> rows = [];
                        aPIMEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                        {
                            string stateColor = item.SubscriptionRequired ? "40" : "red";
                            rows.Add([new($"[93]{item.Name}[/]"), new($"[yellow]{item.DisplayName}[/]"), new($"{item.Path}"), new($"[40]{item.ResourceGroup}[/]"), new($"{item.ServiceUrl}"), new($"[{stateColor}]{item.SubscriptionRequired}[/]")]);
                        });
                        FormatResults.Show<APIMEntity[]>(aPIMEntity, null, Components.Table.Show(true, $"[aqua]Azure APIM APIs([40]{rows.Count}[/])[/]", string.Empty, columns, rows), moduleName.Replace("/", "_"));
                        AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                        _ = Console.ReadKey();
                        return true;
                    }
                }
            }
            return false;
        }

        internal static bool GetAPIMListWithOperations()
        {
            AzResourceGroupListEntity[] azResourcesInResourcesListEntity = GetResourceGroupListData();
            List<string> choices = [];
            azResourcesInResourcesListEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add($"{item.Name} ({item.ResourceGroup})"); });
            choices.Add(AzCommands.Choise_Cancel);
            if (azResourcesInResourcesListEntity != null)
            {
                bool showChoises = true;
                while (showChoises)
                {
                    AnsiConsole.Clear();
                    string moduleName = "/APIM/GetAPIMListWithOperations";
                    ModuleHeader.Show(moduleName);
                    string apim = SelectionPrompt.Show(choices);
                    if (apim == AzCommands.Choise_Cancel)
                    {
                        showChoises = false;
                        return true;
                    }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                    (string Name, string ResourceGroup) apimName = azResourcesInResourcesListEntity.Where(t => apim.Contains($"{t.Name} ({t.ResourceGroup})"))
                    .OrderBy(t => t.Name)
                      .Select(t => (t.Name, t.ResourceGroup))
                      .First();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    APIMEntity[] aPIMEntity = AzAPIManagementList(apimName);
                    if (aPIMEntity != null)
                    {
                        List<string> choicesAPIMs = [];
                        aPIMEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choicesAPIMs.Add($"{item.Name}"); });
                        choicesAPIMs.Add(AzCommands.Choise_Cancel);
                        bool showChoisesAPIMs = true;
                        while (showChoisesAPIMs)
                        {
                            AnsiConsole.Clear();
                            moduleName = "/APIM/GetAPIMListWithOperations";
                            ModuleHeader.Show(moduleName);
                            string api = SelectionPrompt.Show(choicesAPIMs);
                            if (api == AzCommands.Choise_Cancel)
                            {
                                showChoises = false;
                                return true;
                            }
                            string command = AzCommands.APIManagement_OperationList.Replace("@@@ResourceGroup", apimName.ResourceGroup)
                                                                                   .Replace("@@@APIM", apimName.Name)
                                                                                   .Replace("@@@APIs", api);
                            APIMOperationEntity[] operations = CommandHelper.Run<APIMOperationEntity[]>(command, []);
                            List<KeyValuePair<Markup, Justify>> columns =
                            [
                                new(new("Name"), Justify.Left),
                                new(new("DisplayName"), Justify.Left),
                                new(new("Method"), Justify.Left),
                                new(new("ResourceGroup"), Justify.Left),
                                new(new("StatusCode"), Justify.Center)
                            ];
                            List<List<Markup>> rows = [];
                            operations.OrderBy(t => t.Name).ToList().ForEach(item =>
                            {
                                rows.Add([new($"[93]{item.Name}[/]"), new($"[yellow]{item.DisplayName}[/]"), new($"{item.Method}"), new($"[40]{item.ResourceGroup}[/]"), new($"{item.StatusCode}")]);
                            });
                            FormatResults.Show<APIMEntity[]>(aPIMEntity, null, Components.Table.Show(true, $"[aqua]Azure Operations([40]{operations.Length}[/])[/]", string.Empty, columns, rows), moduleName.Replace("/", "_"));
                            AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                            _ = Console.ReadKey();
                        }
                    }
                }
            }
            return false;
        }

        private static AzResourceGroupListEntity[] GetResourceGroupListData()
        {
            string command = AzCommands.ResourceGroup_ResourcesListInSubscriptionFilterType.Replace("@@@ResourceType", "Microsoft.ApiManagement/service");
            command = !string.IsNullOrWhiteSpace(AzCommand.AzureQueryFilters) ?
                                                                                command.Replace("@@@AzureQueryFilter", AzCommand.AzureQueryFilters).Replace("@@@AzureQueryFilterPropertyName", "name") :
                                                                                command.Replace("@@@AzureQueryFilter", string.Empty);
            command = command.Replace("[ && ", "[?");
            return CommandHelper.Run<AzResourceGroupListEntity[]>(command, []);
        }

        private static APIMEntity[] AzAPIManagementList((string Name, string ResourceGroup) apimName)
        {
            string command = AzCommands.APIManagement_List.Replace("@@@ResourceGroup", apimName.ResourceGroup)
                                                          .Replace("@@@APIM", apimName.Name);
            return CommandHelper.Run<APIMEntity[]>(command, []);
        }
    }
}
