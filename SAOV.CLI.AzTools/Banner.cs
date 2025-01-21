namespace SAOV.CLI.AzTools
{
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Modules.AzureCli.Entities;
    using Spectre.Console;
    using System.Reflection;
    using static System.Net.Mime.MediaTypeNames;

    internal static class Banner
    {
        internal static void Show()
        {
            AzCliVersionEntity azVersionEntity = CommandHelper.Run<AzCliVersionEntity>(AzCommands.AzureCli_Version, [], false, false);
            Table tableAzCli = new Table()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn(string.Empty).RightAligned())
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddRow(new Markup("[93]Az-Cli [yellow]:[/][/]"), new Markup($"[40]{azVersionEntity?.AzureCli}[/]"))
                .AddRow(new Markup("[93]Az-Core [yellow]:[/][/]"), new Markup($"[40]{azVersionEntity?.AzureCliCore}[/]"))
                .AddRow(new Markup("[93]Az-Telemetry [yellow]:[/][/]"), new Markup($"[40]{azVersionEntity?.AzureCliTelemetry}[/]"))
                .AddRow(new Markup($"[93]Query Filter [yellow]:[/][/]"), new Markup($"[red][Turquoise2]\"[/]{Program.Filters}[Turquoise2]\"[/][/]"))
                .HideHeaders()
                .HideFooters()
                .HideRowSeparators()
                .Centered();
            Table tableEnvironment = new Table()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn(string.Empty).RightAligned())
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddRow(new Markup($"[Turquoise2]User-Name [yellow]:[/][/]"), new Markup($"[40]{Environment.UserName}[/]"))
                .AddRow(new Markup($"[Turquoise2]Machine-Name [yellow]:[/][/]"), new Markup($"[40]{Environment.MachineName}[/]"))
                .AddRow(new Markup($"[Turquoise2]OS-Version [yellow]:[/][/]"), new Markup($"[40]{Environment.OSVersion}[/]"))
                .AddRow(new Markup($"[Turquoise2]dotnet-Version [yellow]:[/][/]"), new Markup($"[40]{Environment.Version}[/]"))
                .HideHeaders()
                .HideFooters()
                .HideRowSeparators()
                .Centered();
            Table tableAbout = new Table()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn(string.Empty))
                .AddRow(new Markup("[Magenta1]Powered by C#[/]").Centered())
                .AddRow(new Markup($"[red]v{Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString()}[/]").Centered())
                .AddRow(new Markup("[Turquoise2]saov@outlook.com[/]").Centered())
                .AddRow(new Markup($"[Gold1]{DateTime.Now.Year}[/]").Centered())
                .HideHeaders()
                .HideFooters()
                .HideRowSeparators()
                .Centered();
            Table tableInfo = new Table()
                .Border(TableBorder.None)
                .AddColumn(string.Empty)
                .AddColumn(string.Empty)
                .AddColumn(string.Empty)
                .AddRow(tableEnvironment, tableAzCli, tableAbout)
                .HideHeaders()
                .HideFooters()
                .HideRowSeparators()
                .Centered();
            Table tableBanner = new Table()
                .Border(TableBorder.Square)
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn(new FigletText("SAOV Azure Tools").Centered().Color(Color.Turquoise2)).Centered())
                .AddRow(tableInfo)
                .Centered();
            AnsiConsole.Write(tableBanner);
            AnsiConsole.WriteLine();
        }
    }
}
