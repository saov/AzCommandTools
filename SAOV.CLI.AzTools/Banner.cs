namespace SAOV.CLI.AzTools
{
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Modules.AzureCli.Entities;
    using Spectre.Console;

    internal static class Banner
    {
        internal static void Show()
        {
            AzCliVersionEntity azVersionEntity = CommandHelper.Run<AzCliVersionEntity>(AzCommands.AzureCli_Version, []);
            Table tableAzCli = new Table()
                .Centered()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn(string.Empty).RightAligned())
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddRow(new Markup("[93]Az-Cli :[/]"), new Markup($"[40]{azVersionEntity.AzureCli}[/]"))
                .AddRow(new Markup("[93]Az-Core :[/]"), new Markup($"[40]{azVersionEntity.AzureCliCore}[/]"))
                .AddRow(new Markup("[93]Az-Telemetry :[/]"), new Markup($"[40]{azVersionEntity.AzureCliTelemetry}[/]"));
            Table tableInfo = new Table()
               .Border(TableBorder.None)
               .AddColumn(string.Empty)
               .AddColumn(string.Empty)
               .AddRow(AboutDetails.Get(TableBorder.None, new TableColumn(string.Empty), true).Centered(), tableAzCli)
               .Centered();
            Table tableBanner = new Table()
               .Border(TableBorder.Square)
               .BorderColor(Color.Grey)
               .AddColumn(new TableColumn(new FigletText("SAOV Azure Tools").Centered().Color(Color.Aqua)).Centered())
               .AddRow(tableInfo);
            AnsiConsole.Write(tableBanner);
            AnsiConsole.WriteLine();
        }
    }
}
