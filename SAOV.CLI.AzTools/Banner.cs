namespace SAOV.CLI.AzTools
{
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Modules.AzureCli.Entities;
    using Spectre.Console;

    internal static class Banner
    {
        internal static void Show()
        {
            AzCliVersionEntity azVersionEntity = CommandHelper.Run<AzCliVersionEntity>(AzCommands.AzureCli_Version, [], false, false);
            Table tableAzCli = new Table()
                .Centered()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn(string.Empty).RightAligned())
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddRow(new Markup("[93]Az-Cli [yellow]:[/][/]"), new Markup($"[40]{azVersionEntity?.AzureCli}[/]"))
                .AddRow(new Markup("[93]Az-Core [yellow]:[/][/]"), new Markup($"[40]{azVersionEntity?.AzureCliCore}[/]"))
                .AddRow(new Markup("[93]Az-Telemetry [yellow]:[/][/]"), new Markup($"[40]{azVersionEntity?.AzureCliTelemetry}[/]"))
                .AddRow(new Markup($"[93]Current Query Filter [yellow]:[/][/]"), new Markup($"[red][Turquoise2]\"[/]{Program.Filters}[Turquoise2]\"[/][/]"));
            Table tableInfo = new Table()
               .Border(TableBorder.None)
               .AddColumn(string.Empty)
               .AddColumn(string.Empty)
               .AddRow(AboutDetails.Get(TableBorder.None, new TableColumn(string.Empty), true).Centered(), tableAzCli)
               .Centered();
            Table tableBanner = new Table()
               .Border(TableBorder.Square)
               .BorderColor(Color.Grey)
               .AddColumn(new TableColumn(new FigletText("SAOV Azure Tools").Centered().Color(Color.Turquoise2)).Centered())
               .AddRow(tableInfo);
            AnsiConsole.Write(tableBanner);
            AnsiConsole.WriteLine();
        }
    }
}
