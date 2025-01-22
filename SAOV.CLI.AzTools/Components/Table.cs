namespace SAOV.CLI.AzTools.Components
{
    using Spectre.Console;

    internal static class Table
    {
        internal static Spectre.Console.Table Show(bool tableBorder, string title, string caption, List<KeyValuePair<Markup, Justify>> columns, List<List<Markup>> rows, bool expand = false)
        {
            Spectre.Console.Table table = new Spectre.Console.Table()
              .Border(tableBorder ? TableBorder.Square : TableBorder.None)
              .Title($"{title}")
              .Caption($"{caption}")
              .BorderColor(Color.Grey)
              .ShowRowSeparators();
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
            return table;
        }
    }
}
