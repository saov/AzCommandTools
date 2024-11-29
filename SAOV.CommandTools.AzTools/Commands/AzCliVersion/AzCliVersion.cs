namespace SAOV.CommandTools.AzTools.Commands.AzCliVersion
{
    using SAOV.CommandTools.AzTools.Commands.AzVersion;
    using SAOV.CommandTools.AzTools.Helpers;
    using Spectre.Console;

    internal static class AzCliVersion
    {
        public static bool Get()
        {
            AzCliVersionEntity? azVersionEntity = null;
            AnsiConsole.Status()
            .AutoRefresh(true)
            .Spinner(Spinner.Known.Default)
            .SpinnerStyle(Style.Parse("green bold"))
            .Start("[yellow]Getting data[/]", ctx =>
            {
                azVersionEntity = JsonHelper.GetEntity<AzCliVersionEntity>(AzHelper.GetAzureInfo(AzCommands.AzVersion));
            });
            if (azVersionEntity != null)
            {
                Map(azVersionEntity);
                return true;
            }
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Markup("[red]Verify that you are authenticated in Azure.[/]"));
            AnsiConsole.WriteLine();
            return false;
        }

        public static void Map(AzCliVersionEntity azVersionEntity)
        {
            Table tableProperties = new Table()
                .Centered()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddColumn(new TableColumn(string.Empty).RightAligned())
                .AddRow(new Markup("[93]Cli[/]").LeftJustified(), new Markup($"[40]{azVersionEntity?.AzureCli}[/]").Centered())
                .AddRow(new Markup("[93]Core[/]").LeftJustified(), new Markup($"[40]{azVersionEntity?.AzureCliCore}[/]").Centered())
                .AddRow(new Markup("[93]Telemetry[/]").LeftJustified(), new Markup($"[40]{azVersionEntity?.AzureCliTelemetry}[/]").Centered());
            Table tablePrincipal = new Table()
                .Border(TableBorder.Square)
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn(new Markup("[aqua]Azure Cli Installed[/]")).Centered())
                .AddRow(tableProperties);
            AnsiConsole.Write(tablePrincipal);
        }
    }
}
