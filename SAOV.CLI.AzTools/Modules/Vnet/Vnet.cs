namespace SAOV.CLI.AzTools.Modules.Vnet
{
    using SAOV.CLI.AzTools.Components;
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using SAOV.CLI.AzTools.Modules.Vnet.Entities;
    using Spectre.Console;
    using System.Text;

    internal static class Vnet
    {
        public static bool Show()
        {
            List<string> choices = [];
            bool run = false;
            Enum.GetValues<VnetMenu>().ToList().ForEach(item => { choices.Add(item.ToString()); });
            while (!run)
            {
                ModuleHeader.Show("/ResourceGroup");
                string selectionPromptValue = SelectionPrompt.Show(choices);
                _ = Enum.TryParse(selectionPromptValue, out VnetMenu azMenu);
                run = azMenu switch
                {
                    VnetMenu.GetVnetList => GetVnetList(),
                    VnetMenu.GetVnetListWithSubnets => GetVnetListWithSubnets(),
                    VnetMenu.Exit => true,
                    _ => false
                };
            }
            return run;
        }

        internal static bool GetVnetList()
        {
            string moduleName = "/Vnet/GetVnetList";
            ModuleHeader.Show(moduleName);
            AzVnetListEntity[] azVnetListEntity = GetVnetListData();
            if (azVnetListEntity != null)
            {
                List<KeyValuePair<Markup, Justify>> columns =
                [
                    new(new("Name"), Justify.Left),
                    new(new("ResourceGroup"), Justify.Left),
                    new(new("AddressSpace"), Justify.Left),
                    new(new("Subnets"), Justify.Left),
                    new(new("ProvisioningState"), Justify.Center)
                ];
                List<List<Markup>> rows = [];
                azVnetListEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                {
                    string stateColor = item.ProvisioningState == "Succeeded" ? "40" : "red";
                    StringBuilder sbAddressSpace = new();
                    foreach (string itemAddressSpace in item.AddressSpace)
                    {
                        sbAddressSpace.AppendLine(itemAddressSpace);
                    }
                    StringBuilder sbSubnets = new();
                    foreach (string itemSubnets in item.Subnets)
                    {
                        string[] subnets = itemSubnets.Split("/", StringSplitOptions.RemoveEmptyEntries);
                        sbSubnets.AppendLine(subnets[^1]);
                    }
                    rows.Add([new($"[93]{item.Name}[/]"), new($"[40]{item.ResourceGroup}[/]"), new($"[yellow]{sbAddressSpace}[/]"), new($"[yellow]{sbSubnets}[/]"), new($"[{stateColor}]{item.ProvisioningState}[/]")]);
                });
                FormatResults.Show<AzVnetListEntity[]>(azVnetListEntity, null, Components.Table.Show(true, $"[aqua]Azure Vnets([40]{rows.Count}[/])[/]", string.Empty, columns, rows), moduleName.Replace("/", "_"));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        internal static bool GetVnetListWithSubnets()
        {
            string moduleName = "/Vnet/GetVnetListWithSubnets";
            ModuleHeader.Show(moduleName);
            AzVnetListEntity[] azVnetListEntity = GetVnetListData();
            if (azVnetListEntity != null)
            {
                List<KeyValuePair<Markup, Justify>> columnsMain =
                [
                    new(new("Name"), Justify.Left),
                    new(new("ResourceGroup"), Justify.Left),
                    new(new("Subnets"), Justify.Left),
                    new(new("ProvisioningState"), Justify.Center)
                ];
                Spectre.Console.Table mainTable = Components.Table.Show(true, $"[aqua]Azure Vnets([40]{azVnetListEntity.Length}[/])[/]", string.Empty, columnsMain);
                azVnetListEntity = azVnetListEntity.OrderBy(t => t.Name).ToArray();
                for (int i = 0; i < azVnetListEntity.Length; i++)
                {
                    string stateColorMain = azVnetListEntity[i].ProvisioningState == "Succeeded" ? "40" : "red";
                    List<Dictionary<string, string>> itemsWithParamentersCommand = [];
                    azVnetListEntity[i].Subnets.ForEach(itemSubnet => { itemsWithParamentersCommand.Add(new() { { "@@@Ids", itemSubnet } }); });
                    azVnetListEntity[i].SubnetsDetails = Components.Progress.Show<AzSubnetEntity>($"Get Subnets for Vnet [yellow]([40]{azVnetListEntity[i].Name} [aqua]{i+1}[/][yellow]/[/][aqua]{azVnetListEntity.Length}[/][/])[/]", AzCommands.Subnet_Show, itemsWithParamentersCommand);
                    List<KeyValuePair<Markup, Justify>> columnsSubnet =
                    [
                        new(new("AddressPrefix"), Justify.Left),
                        new(new("Name"), Justify.Left),
                        new(new("NetworkSecurityGroup"), Justify.Left),
                        new(new("RouteTable"), Justify.Left),
                        new(new("ProvisioningState"), Justify.Center)
                    ];
                    Spectre.Console.Table subnetTable = null;
                    subnetTable = Components.Table.Show(true, $"[aqua]Subnets([40]{azVnetListEntity[i].Subnets.Count}[/])[/]", string.Empty, columnsSubnet);
                    for (int j = 0; j < azVnetListEntity[i].Subnets.Count; j++)
                    {
                        if (azVnetListEntity[i].SubnetsDetails[j] != null)
                        {
                            azVnetListEntity[i].SubnetsDetails[j].Id = azVnetListEntity[i].Subnets[j];
                            string stateColorSubnet = azVnetListEntity[i].SubnetsDetails[j].ProvisioningState == "Succeeded" ? "40" : "red";
                            string[]? networkSecurityGroupArray = azVnetListEntity[i].SubnetsDetails[j]?.NetworkSecurityGroup?.Split("/", StringSplitOptions.RemoveEmptyEntries);
                            string networkSecurityGroup = networkSecurityGroupArray != null && networkSecurityGroupArray.Length > 0 ? networkSecurityGroupArray[^1] : string.Empty;
                            string[]? routeTableArray = azVnetListEntity[i].SubnetsDetails[j]?.RouteTable?.Split("/", StringSplitOptions.RemoveEmptyEntries);
                            string routeTable = routeTableArray != null && routeTableArray.Length > 0 ? routeTableArray[^1] : string.Empty;
                            string? addressPrefix = string.IsNullOrEmpty(azVnetListEntity[i].SubnetsDetails[j].AddressPrefix) ?
                                azVnetListEntity[i].SubnetsDetails[j].AddressPrefixes != null && azVnetListEntity[i].SubnetsDetails[j].AddressPrefixes.Count > 0 ?
                                    string.Join(", ", azVnetListEntity[i].SubnetsDetails[j].AddressPrefixes) :
                                    string.Empty :
                                azVnetListEntity[i].SubnetsDetails[j].AddressPrefix;
                            subnetTable.AddRow(new Markup($"[93]{addressPrefix}[/]"), new Markup($"[yellow]{azVnetListEntity[i].SubnetsDetails[j].Name}[/]"), new Markup($"[yellow]{networkSecurityGroup}[/]"), new Markup($"[yellow]{routeTable}[/]"), new Markup($"[{stateColorSubnet}]{azVnetListEntity[i].SubnetsDetails[j].ProvisioningState}[/]"));
                        }
                    }
                    mainTable.AddRow(new Markup($"[93]{azVnetListEntity[i].Name}[/]"), new Markup($"[40]{azVnetListEntity[i].ResourceGroup}[/]"), (subnetTable != null ? subnetTable : new Markup(string.Empty)), new Markup($"[{stateColorMain}]{azVnetListEntity[i].ProvisioningState}[/]"));
                }
                FormatResults.Show<AzVnetListEntity[]>(azVnetListEntity, null, mainTable, moduleName.Replace("/","_"));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        private static AzVnetListEntity[] GetVnetListData()
        {
            string command = AzCommands.Vnet_List;
            command = !string.IsNullOrWhiteSpace(AzCommand.AzureQueryFilters) ?
                                                                                command.Replace("@@@AzureQueryFilter", AzCommand.AzureQueryFilters).Replace("@@@AzureQueryFilterPropertyName", "name") :
                                                                                command.Replace("@@@AzureQueryFilter", string.Empty);
            return CommandHelper.Run<AzVnetListEntity[]>(command, []);
        }
    }
}
