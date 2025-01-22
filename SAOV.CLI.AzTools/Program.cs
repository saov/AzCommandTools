namespace SAOV.CLI.AzTools
{
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using SAOV.CLI.AzTools.Modules.Account;
    using SAOV.CLI.AzTools.Modules.AzureCli;
    using SAOV.CLI.AzTools.Modules.AzureCli.Entities;
    using SAOV.CLI.AzTools.Modules.KeyVault;
    using SAOV.CLI.AzTools.Modules.ResourceGroup;
    using Spectre.Console;
    using System.Text;

    internal class Program
    {
        internal static string? AzureQueryFilters;
        internal static string? Filters;

        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
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
                        MainMenu.QueryFilters => QueryFilters(),
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
        }

        internal static bool QueryFilters()
        {
            AnsiConsole.Clear();
            Banner.Show();
            AnsiConsole.Write(new Markup("[red]Filter is Case Sensitive or Lower Case or UpperCase.[/]"));
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
            string filter = Components.TextPrompt.Show("[93]Filter for AzCli Search ([40]Space for multi filters[/])[/] [yellow]: [/]", (value) => { return true; }, "", true);
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] filters = filter.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                StringBuilder stringBuilder = new();
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
                AzureQueryFilters = stringBuilder.ToString();
            }
            else
            {
                AzureQueryFilters = null;
                Filters = string.Empty;
            }
            return true;
        }
    }
}
