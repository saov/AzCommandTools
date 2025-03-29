namespace SAOV.CLI.AzTools
{
    using SAOV.CLI.AzTools.Components;
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using SAOV.CLI.AzTools.Modules.Account;
    using SAOV.CLI.AzTools.Modules.AzureCli;
    using SAOV.CLI.AzTools.Modules.AzureCli.Entities;
    using SAOV.CLI.AzTools.Modules.KeyVault;
    using SAOV.CLI.AzTools.Modules.ResourceGroup;
    using SAOV.CLI.AzTools.Modules.Vnet;
    using Spectre.Console;
    using Spectre.Console.Cli;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    internal sealed class AzCommand : Command<AzCommand.Settings>
    {
        internal static string? AzureQueryFilters;
        internal static string? Filters;

        public sealed class Settings : CommandSettings
        {
            [Description("Resource Search Filter.")]
            [CommandArgument(0, "[filter]")]
            public string? Filter { get; set; }
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.Filter))
            {
                SetFilter(settings.Filter);
            }
            AzCliVersionEntity azVersionEntity = CommandHelper.Run<AzCliVersionEntity>(AzCommands.AzureCli_Version, [], false, false);
            if (azVersionEntity != null)
            {
                List<string> choices = [];
                bool run = true;
                Enum.GetValues<MainMenu>().ToList().ForEach(item => { choices.Add(item.ToString()); });
                while (run)
                {
                    AnsiConsole.Clear();
                    Banner.Show();
                    string selectionPromptValue = Components.SelectionPrompt.Show(choices);
                    _ = Enum.TryParse(selectionPromptValue, out MainMenu mainMenuOption);
                    run = mainMenuOption switch
                    {
                        MainMenu.AzureCli => AzureCli.Show(),
                        MainMenu.Account => Account.Show(),
                        MainMenu.KeyVault => KeyVault.Show(),
                        MainMenu.ResourceGroup => ResourceGroup.Show(),
                        MainMenu.Vnet => Vnet.Show(),
                        MainMenu.QueryFilters => QueryFilters(),
                        MainMenu.NavigationMap => NavigationMap(),
                        MainMenu.Exit => false,
                        _ => false
                    };
                }
                Console.Clear();
            }
            else
            {
                AnsiConsole.Clear();
                Banner.Show();
                AnsiConsole.Write(new Markup("[red]The installed [40]az[/] command was not detected. [yellow]([/][40]https://learn.microsoft.com/en-us/cli/azure/install-azure-cli[/][yellow])[/][/]"));
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();
            }
            return 0;
        }

        private static bool QueryFilters()
        {
            AnsiConsole.Clear();
            Banner.Show();
            AnsiConsole.Write(new Markup("[red]Filter is Case Sensitive or Lower Case or UpperCase.[/]"));
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
            string filter = TextPrompt.Show("[93]Filter for AzCli Search ([40]Space for multi filters[/])[/] [yellow]: [/]", (value) => { return true; }, "", true);
            SetFilter(filter);
            return true;
        }

        private static bool NavigationMap()
        {
            AnsiConsole.Clear();
            ModuleHeader.Show("/NavigationMap");
            Tree menu = new Tree(new Markup("[Magenta1]Menu[/]"))
                            .Style("40")
                            .Guide(TreeGuide.DoubleLine);
            TreeNode menu_AzureCli = menu.AddNode(new Tree("[Turquoise2]AzureCli[/]"));
            _ = menu_AzureCli.AddNode(new Tree("[Gold1]AzCliUpgrade[/] [red]-->[/] Az Cli Upgrade"));
            _ = menu_AzureCli.AddNode(new Tree("[Gold1]GetExtensionsInstalledList[/] [red]-->[/] Get Extensions Installed List"));
            _ = menu_AzureCli.AddNode(new Tree("[Gold1]GetExtensionsAvailableList[/] [red]-->[/] Get Extensions Available List"));
            _ = menu_AzureCli.AddNode(new Tree("[Gold1]UpdateExtension[/] [red]-->[/] Update Extension"));
            _ = menu_AzureCli.AddNode(new Tree("[Gold1]AddExtension[/] [red]-->[/] Add Extension"));
            _ = menu_AzureCli.AddNode(new Tree("[Gold1]RemoveExtension[/] [red]-->[/] Remove Extension"));
            TreeNode menu_Account = menu.AddNode(new Tree("[Turquoise2]Account[/]"));
            _ = menu_Account.AddNode(new Tree("[Gold1]LogIn[/] [red]-->[/] Log In"));
            _ = menu_Account.AddNode(new Tree("[Gold1]LogOut[/] [red]-->[/] Log Out"));
            _ = menu_Account.AddNode(new Tree("[Gold1]ShowCurrentSubscription[/] [red]-->[/] Show Current Subscription"));
            _ = menu_Account.AddNode(new Tree("[Gold1]GetSubscriptionList[/] [red]-->[/] Get Subscription List"));
            _ = menu_Account.AddNode(new Tree("[Gold1]SetSubscription[/] [red]-->[/] Set Subscription"));
            TreeNode menu_KeyVault = menu.AddNode(new Tree("[Turquoise2]KeyVault[/]"));
            _ = menu_KeyVault.AddNode(new Tree("[Gold1]GetKeyVaultList[/] [red]-->[/] Get KeyVault List"));
            _ = menu_KeyVault.AddNode(new Tree("[Gold1]GetKeyVaultListWithNetworkRules[/] [red]-->[/] Get KeyVault List With Network Rules"));
            _ = menu_KeyVault.AddNode(new Tree("[Gold1]GetKeyVaulSecretList[/] [red]-->[/] Get KeyVaul Secret List"));
            _ = menu_KeyVault.AddNode(new Tree("[Gold1]KeyVaultSecretShow[/] [red]-->[/] Key Vault Secret Show"));
            _ = menu_KeyVault.AddNode(new Tree("[Gold1]KeyVaultAllSecretShow[/] [red]-->[/] Key Vault All Secret Show"));
            TreeNode menu_ResourceGroup = menu.AddNode(new Tree("[Turquoise2]ResourceGroup[/]"));
            _ = menu_ResourceGroup.AddNode(new Tree("[Gold1]GetResourceGroupList[/] [red]-->[/] Get ResourceGroup List"));
            _ = menu_ResourceGroup.AddNode(new Tree("[Gold1]GetResourcesInResouceGroup[/] [red]-->[/] Get Resources In ResouceGroup"));
            _ = menu_ResourceGroup.AddNode(new Tree("[Gold1]GetResourcesInSubscription[/] [red]-->[/] Get Resources In Subscription"));
            TreeNode menu_Vnet = menu.AddNode(new Tree("[Turquoise2]Vnet[/]"));
            _ = menu_Vnet.AddNode(new Tree("[Gold1]GetVnetList[/] [red]-->[/] Get Vnet List"));
            _ = menu_Vnet.AddNode(new Tree("[Gold1]GetVnetListWithSubnets[/] [red]-->[/] Get Vnets With Subnets"));
            _ = menu.AddNode(new Tree("[Turquoise2]QueryFilters[/] [red]-->[/] Resource Search Filter"));
            _ = menu.AddNode(new Tree("[Turquoise2]NavigationMap[/] [red]-->[/] Navigation Map"));
            _ = menu.AddNode(new Tree("[Turquoise2]Exit[/] [red]-->[/] Exit"));
            AnsiConsole.Write(menu);
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
            _ = Console.ReadKey();
            return true;
        }

        private static void SetFilter(string filter)
        {
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] filters = filter.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                StringBuilder stringBuilder = new();
                Filters = string.Join(" ", filters);
                for (int i = 0; i < filters.Length; i++)
                {
                    if (i == 0)
                    {
                        Filters = $"({filters[i]} || {filters[i].ToLower()} || {filters[i].ToUpper()})";
                        stringBuilder.Append($"?(contains(@@@AzureQueryFilterPropertyName, '{filters[i]}') || ");
                        stringBuilder.Append($"contains(@@@AzureQueryFilterPropertyName, '{filters[i].ToLower()}') || ");
                        stringBuilder.Append($"contains(@@@AzureQueryFilterPropertyName, '{filters[i].ToUpper()}')) && ");
                    }
                    else if (i == filters.Length - 1)
                    {
                        Filters = $"{Filters} && ({filters[i]} || {filters[i].ToLower()} || {filters[i].ToUpper()})";
                        stringBuilder.Append($"(contains(@@@AzureQueryFilterPropertyName, '{filters[i]}') || ");
                        stringBuilder.Append($"contains(@@@AzureQueryFilterPropertyName, '{filters[i].ToLower()}') || ");
                        stringBuilder.Append($"contains(@@@AzureQueryFilterPropertyName, '{filters[i].ToUpper()}'))");
                    }
                    else
                    {
                        Filters = $"{Filters} && ({filters[i]} || {filters[i].ToLower()} || {filters[i].ToUpper()})";
                        stringBuilder.Append($"(contains(@@@AzureQueryFilterPropertyName, '{filters[i]}') || ");
                        stringBuilder.Append($"contains(@@@AzureQueryFilterPropertyName, '{filters[i].ToLower()}') || ");
                        stringBuilder.Append($"contains(@@@AzureQueryFilterPropertyName, '{filters[i].ToUpper()}')) && ");
                    }
                }
                string totalFilter = stringBuilder.ToString();
                AzureQueryFilters = totalFilter.EndsWith(" && ") ?
                                        totalFilter.Remove(totalFilter.Length - 4, 4) :
                                        totalFilter;
            }
            else
            {
                AzureQueryFilters = null;
                Filters = string.Empty;
            }
        }
    }
}
