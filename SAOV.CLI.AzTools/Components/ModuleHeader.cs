namespace SAOV.CLI.AzTools.Components
{
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Modules.Account.Entities;
    using Spectre.Console;

    internal static class ModuleHeader
    {
        internal static void Show(string path)
        {
            Grid grid = new();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddRow([
                new Text("Subscription", new Style(40)).Centered(),
                new Text("Module", new Style(40)).Centered(),
                new Text("Filter", new Style(40)).Centered()
            ]);
            AzAccountShowEntity azAccountShowEntity = CommandHelper.Run<AzAccountShowEntity>(AzCommands.Account_Show, [], false, false);
            string filters = string.IsNullOrWhiteSpace(Program.Filters) ? string.Empty : Program.Filters;
            string subscription = $"[93]{azAccountShowEntity?.Name}{Environment.NewLine}([Magenta1]{azAccountShowEntity?.Id}[/])[/]";
            grid.AddRow([

                new Markup(azAccountShowEntity == null ? string.Empty : subscription).LeftJustified(),
                new Markup($"[yellow]{path}[/]").Centered(),
                new Markup($"[red]{filters}[/]").RightJustified()
            ]);
            AnsiConsole.Clear();
            AnsiConsole.Write(new Panel(grid)
            {
                Header = new PanelHeader("[Turquoise2]SAOV Azure Tools[/]")
            });
            AnsiConsole.WriteLine();
        }
    }
}
