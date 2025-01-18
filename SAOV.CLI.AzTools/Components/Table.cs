using Spectre.Console;

namespace SAOV.CLI.AzTools.Components
{
    internal static class Table
    {
        internal static void Show(string caption, List<KeyValuePair<Markup, Justify>> columns, List<List<Markup>> rows, bool expand = false)
        {
            Spectre.Console.Table table = new Spectre.Console.Table()
              .Border(TableBorder.Square)
              .Title(($"[blue]{caption}[/]"))
              .BorderColor(Color.Grey);
            columns.ForEach(column =>
            {
                table.AddColumn(new TableColumn(column.Key).Alignment(column.Value));
            });
            rows.ForEach(row =>
            {
                table.AddRow(row);
            });
            if (expand)
            {
                table.Expand();
            }
            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }
    }
}
