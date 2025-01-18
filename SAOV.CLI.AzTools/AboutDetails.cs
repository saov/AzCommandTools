using Spectre.Console;

namespace SAOV.CLI.AzTools
{
    internal static class AboutDetails
    {
        internal static Table Get(TableBorder tableBorder, TableColumn tableColumn, bool isHeader)
        {
            return new Table()
                .Border(tableBorder)
                .BorderColor(Color.Grey)
                .AddColumn(tableColumn)
                .AddRow(new Markup("[40]Powered by C#[/]").Centered())
                .AddRow(new Markup("[93]v2.0.0[/]").Centered())
                .AddRow(new Markup("[yellow]saov@outlook.com[/]").Centered())
                .AddRow(new Markup($"[red]{DateTime.Now.Year}[/]").Centered());
        }
    }
}
