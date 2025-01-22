namespace SAOV.CLI.AzTools.Components
{
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Modules.Account.Entities;
    using Spectre.Console;

    internal static class ModuleHeader
    {
        internal static void Show(string path)
        {
            AzAccountShowEntity azAccountShowEntity = CommandHelper.Run<AzAccountShowEntity>(AzCommands.Account_Show, [], false, false);
            string filters = string.IsNullOrWhiteSpace(Program.Filters) ? string.Empty : Program.Filters;
            string subscription = azAccountShowEntity == null ?
                                    string.Empty : 
                                    $"[green]{azAccountShowEntity?.Name} ([Magenta1]{azAccountShowEntity?.Id}[/])[/]";
            Grid grid = new();
            grid.AddColumn();
            grid.AddRow([
                new Markup($"[navy]Subscription [/][yellow]: [/]{subscription}").LeftJustified()
            ]);
            grid.AddRow([
                new Markup($"[navy]Module [/][yellow]: [/][orange3]{path}[/]").LeftJustified()
            ]);
            grid.AddRow([
                new Markup($"[navy]Filter [/][yellow]: [/][red]{filters}[/]").RightJustified().LeftJustified()                
            ]);
            AnsiConsole.Clear();
            AnsiConsole.Write(new Panel(grid)
            {
                Header = new PanelHeader($"[Turquoise2]SAOV Azure Tools in [40]{Environment.UserName}[red]@[/]{Environment.MachineName}[/][/]")
            });
            AnsiConsole.WriteLine();
        }
    }
}
