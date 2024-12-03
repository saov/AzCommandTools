namespace SAOV.CommandTools.AzTools.Views
{
    using Spectre.Console;

    internal static class AboutDetails
    {
        internal static Table Get(TableBorder tableBorder, TableColumn tableColumn, bool isHeader)
        {
            Table table = new Table()
                .Border(tableBorder)
                .BorderColor(Color.Grey)
                .AddColumn(tableColumn)
                .AddRow(new Markup("[40]Powered by C#[/]").Centered())
                .AddRow(new Markup("[white]v1.0.0[/]").Centered());
            if (!isHeader)
            {
                table.AddRow(new Markup("[yellow]saov@outlook.com[/]").Centered());
            }
            table.AddRow(new Markup($"[red]{DateTime.Now.Year}[/]").Centered());
            return table;
        }
    }
}
