using SAOV.CLI.AzTools.Helpers;
using SAOV.CLI.AzTools.Modules.AzureCli.Entities;
using Spectre.Console;

namespace SAOV.CLI.AzTools
{
    internal static class Banner
    {
        internal static void Show()
        {
            AzCliVersionEntity azVersionEntity = CommandHelper.Run<AzCliVersionEntity>(AzCommands.AzureCli_Version, []);
            Table tableAzCli = new Table()
                .Centered()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddColumn(new TableColumn(string.Empty).RightAligned())
                .AddRow(new Markup("[93]Az Cli[/]").LeftJustified(), new Markup($"[40]{azVersionEntity.AzureCli}[/]").Centered())
                .AddRow(new Markup("[93]Az Core[/]").LeftJustified(), new Markup($"[40]{azVersionEntity.AzureCliCore}[/]").Centered())
                .AddRow(new Markup("[93]Az Telemetry[/]").LeftJustified(), new Markup($"[40]{azVersionEntity.AzureCliTelemetry}[/]").Centered());
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
