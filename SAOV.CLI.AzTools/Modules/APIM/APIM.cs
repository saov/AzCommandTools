namespace SAOV.CLI.AzTools.Modules.ResourceGroup
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
            choices.Add($"[93](x) [yellow]Cancel[/][/]");
            if (azResourcesInResourcesListEntity != null)
            {
                bool showChoises = true;
                while (showChoises)
                {
                    AnsiConsole.Clear();
                    string moduleName = "/APIM/GetAPIMList";
                    ModuleHeader.Show(moduleName);
                    string apim = SelectionPrompt.Show(choices);
                    if (apim == "[93](x) [yellow]Cancel[/][/]")
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
            choices.Add($"[93](x) [yellow]Cancel[/][/]");
            if (azResourcesInResourcesListEntity != null)
            {
                bool showChoises = true;
                while (showChoises)
                {
                    AnsiConsole.Clear();
                    string moduleName = "/APIM/GetAPIMListWithOperations";
                    ModuleHeader.Show(moduleName);
                    string apim = SelectionPrompt.Show(choices);
                    if (apim == "[93](x) [yellow]Cancel[/][/]")
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
                        List<KeyValuePair<Markup, Justify>> columnsMain =
                        [
                            new(new("APIM"), Justify.Left),
                            new(new("Operations"), Justify.Center)
                        ];
                        Spectre.Console.Table mainTable = Components.Table.Show(true, $"[aqua]Azure APIM ([40]{apim}[/]) APIs([40]{aPIMEntity.Length}[/])[/]", string.Empty, columnsMain);
                        aPIMEntity = aPIMEntity.OrderBy(t => t.Name).ToArray();
                        for (int i = 0; i < aPIMEntity.Length; i++)
                        {
                            AnsiConsole.Write(new Markup(aPIMEntity[i].Name));
                            string stateColor = aPIMEntity[i].SubscriptionRequired ? "40" : "red";
                            string command = AzCommands.APIManagement_OperationList.Replace("@@@ResourceGroup", apimName.ResourceGroup)
                                                                                   .Replace("@@@APIM", apimName.Name)
                                                                                   .Replace("@@@APIs", aPIMEntity[i].Name);
                            aPIMEntity[i].Operations = CommandHelper.Run<APIMOperationEntity[]>(command, []);
                            List<KeyValuePair<Markup, Justify>> columnsOperation =
                            [
                                new(new("Name"), Justify.Left),
                                new(new("DisplayName"), Justify.Left),
                                new(new("ResourceGroup"), Justify.Left),
                                new(new("Method"), Justify.Left),
                                new(new("StatusCode"), Justify.Center)
                            ];
                            Spectre.Console.Table operationTable = null;
                            operationTable = Components.Table.Show(true, $"[aqua]Operations([40]{aPIMEntity[i].Operations.Length}[/])[/]", string.Empty, columnsOperation);
                            for (int j = 0; j < aPIMEntity[i].Operations.Length; j++)
                            {
                                if (aPIMEntity[i].Operations != null)
                                {
                                    operationTable.AddRow(new Markup($"[93]{aPIMEntity[i].Operations[j].Name}[/]"),
                                                          new Markup($"[yellow]{aPIMEntity[i].Operations[j].DisplayName}[/]"),
                                                          new Markup($"[40]{aPIMEntity[i].Operations[j].ResourceGroup}[/]"),
                                                          new Markup($"[93]{aPIMEntity[i].Operations[j].Method}[/]"),
                                                          new Markup($"[yellow]{aPIMEntity[i].Operations[j].StatusCode}[/]"));

                                }
                            }
                            mainTable.AddRow(new Markup($"Name : [93]{aPIMEntity[i].Name}[/]\nDisplayName : [yellow]{aPIMEntity[i].DisplayName}[/]\nPath : [yellow]{aPIMEntity[i].Path}[/]\nResourceGroup : [40]{aPIMEntity[i].ResourceGroup}[/]\nServiceUrl : [yellow]{aPIMEntity[i].ServiceUrl}[/]\nSubscriptionRequired : [{stateColor}]{aPIMEntity[i].SubscriptionRequired}[/]"),
                                             (operationTable != null ? operationTable : new Markup(string.Empty)));
                        }
                        FormatResults.Show<APIMEntity[]>(aPIMEntity, null, mainTable, moduleName.Replace("/", "_"));
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
            string command = AzCommands.ResourceGroup_ResourcesListInSubscriptionFilterType.Replace("@@@ResourceType", "Microsoft.ApiManagement/service");
            command = !string.IsNullOrWhiteSpace(AzCommand.AzureQueryFilters) ?
                                                                                command.Replace("@@@AzureQueryFilter", AzCommand.AzureQueryFilters).Replace("@@@AzureQueryFilterPropertyName", "name") :
                                                                                command.Replace("@@@AzureQueryFilter", "");
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
