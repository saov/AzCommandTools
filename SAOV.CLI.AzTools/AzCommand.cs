namespace SAOV.CLI.AzTools
{
    using SAOV.CLI.AzTools.Components;
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using SAOV.CLI.AzTools.Modules.Account;
    using SAOV.CLI.AzTools.Modules.ACR;
    using SAOV.CLI.AzTools.Modules.APIM;
    using SAOV.CLI.AzTools.Modules.AzureCli;
    using SAOV.CLI.AzTools.Modules.AzureCli.Entities;
    using SAOV.CLI.AzTools.Modules.Docker;
    using SAOV.CLI.AzTools.Modules.KeyVault;
    using SAOV.CLI.AzTools.Modules.Kubernetes;
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
        internal static bool AZSDebug;
        internal static string? KubernetesCurrentContext;
        internal static string? KubernetesCurrentNameSpace;

        public sealed class Settings : CommandSettings
        {
            [Description("Resource Search Filter.")]
            [CommandArgument(0, "[filter]")]
            public string? Filter { get; set; }

            [Description("Flag For Debug.")]
            [CommandArgument(1, "[aZSDebug]")]
            public string? AZSDebug { get; set; }
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.Filter))
            {
                SetFilter(settings.Filter);
            }
            if (!string.IsNullOrWhiteSpace(settings.AZSDebug))
            {
                AZSDebug = true;
            }
            AzCliVersionEntity? azVersionEntity = CommandHelper.Run<AzCliVersionEntity>(AzCommands.AzureCli_Version, [], false, false);
            if (azVersionEntity != null)
            {
                List<string> choices = [];
                bool run = true;
                Enum.GetValues<MainMenu>().ToList().ForEach(item => { choices.Add(item.ToString()); });
                while (run)
                {
                    AnsiConsole.Clear();
                    Banner.Show();
                    string selectionPromptValue = SelectionPrompt.Show(choices);
                    _ = Enum.TryParse(selectionPromptValue, out MainMenu mainMenuOption);
                    run = mainMenuOption switch
                    {
                        MainMenu.Docker => Docker.Show(),
                        MainMenu.Kubernetes => Kubernetes.Show(),
                        MainMenu.AzureCli => AzureCli.Show(),
                        MainMenu.Account => Account.Show(),
                        MainMenu.ACR => ACR.Show(),
                        MainMenu.APIM => APIM.Show(),
                        MainMenu.KeyVault => KeyVault.Show(),
                        MainMenu.ResourceGroup => ResourceGroup.Show(),
                        MainMenu.Vnet => Vnet.Show(),
                        MainMenu.QueryFilters => QueryFilters(),
                        MainMenu.NavigationMap => NavigationMap.GetNavigationMap(),
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
            string filter = TextPrompt.Show("[93]Filter for AzCli Search ([40]Space for multi filters[/])[/] [yellow]: [/]", (value) => { return true; }, string.Empty, true);
            SetFilter(filter);
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
