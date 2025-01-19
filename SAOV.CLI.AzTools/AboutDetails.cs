namespace SAOV.CLI.AzTools
{
    using Spectre.Console;

    internal static class AboutDetails
    {
        internal static Table Get(TableBorder tableBorder, TableColumn tableColumn, bool isHeader)
        {
            return new Table()
                .Border(tableBorder)
                .BorderColor(Color.Grey)
                .AddColumn(tableColumn)
                .AddRow(new Markup("[Magenta1]Powered by C#[/]").Centered())
                .AddRow(new Markup("[red]v2.0.0[/]").Centered())
                .AddRow(new Markup("[Turquoise2]saov@outlook.com[/]").Centered())
                .AddRow(new Markup($"[Gold1]{DateTime.Now.Year}[/]").Centered());
        }
    }
}
