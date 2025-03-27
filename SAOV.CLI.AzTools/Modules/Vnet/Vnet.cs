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
            ModuleHeader.Show("/Vnet/GetVnetList");
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
                FormatResults.Show<AzVnetListEntity[]>(azVnetListEntity, null, Components.Table.Show(true, $"[aqua]Azure Vnets([40]{rows.Count}[/])[/]", string.Empty, columns, rows));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        internal static bool GetVnetListWithSubnets()
        {
            ModuleHeader.Show("/Vnet/GetVnetListWithSubnets");
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
                azVnetListEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                {
                    string stateColorMain = item.ProvisioningState == "Succeeded" ? "40" : "red";
                    item.Subnets.ToList().ForEach(itemSubnet =>
                    {
                        AzSubnetEntity azSubnetEntity = CommandHelper.Run<AzSubnetEntity>(AzCommands.Subnet_Show, new() { { "@@@Ids", itemSubnet } });
                        Spectre.Console.Table subnetTable = null;
                        if (azSubnetEntity != null)
                        {
                            azSubnetEntity.Id = itemSubnet;
                            string stateColorSubnet = azSubnetEntity.ProvisioningState == "Succeeded" ? "40" : "red";
                            List<KeyValuePair<Markup, Justify>> columnsSubnet =
                            [
                                new(new("AddressPrefix"), Justify.Left),
                                new(new("Name"), Justify.Left),
                                new(new("NetworkSecurityGroup"), Justify.Left),
                                new(new("RouteTable"), Justify.Left),
                                new(new("ProvisioningState"), Justify.Center)
                            ];
                            subnetTable = Components.Table.Show(true, $"[aqua]Subnets([40]{item.Subnets.Count}[/])[/]", string.Empty, columnsSubnet);
                            item.SubnetsDetails.Add(azSubnetEntity);
                            subnetTable.AddRow(new Markup($"[93]{azSubnetEntity.AddressPrefix}[/]"), new Markup($"[yellow]{azSubnetEntity.Name}[/]"), new Markup($"[yellow]{azSubnetEntity.NetworkSecurityGroup}[/]"), new Markup($"[yellow]{azSubnetEntity.RouteTable}[/]"), new Markup($"[{stateColorSubnet}]{azSubnetEntity.ProvisioningState}[/]"));
                        }
                        mainTable.AddRow(new Markup($"[93]{item.Name}[/]"), new Markup($"[40]{item.ResourceGroup}[/]"), (subnetTable != null ? subnetTable : new Markup(string.Empty)), new Markup($"[{stateColorMain}]{item.ProvisioningState}[/]"));
                    });
                });
                FormatResults.Show<AzVnetListEntity[]>(azVnetListEntity, null, mainTable);
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
                                                                                command.Replace("@@@AzureQueryFilter", "");
            return CommandHelper.Run<AzVnetListEntity[]>(command, []);
        }
    }
}
