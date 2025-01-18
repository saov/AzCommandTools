using Spectre.Console;

namespace SAOV.CLI.AzTools
{
    internal static class Banner
    {
        internal static void Show()
        {
            Table table = new Table()
               .Border(TableBorder.Square)
               .BorderColor(Color.Grey)
               .AddColumn(new TableColumn(new FigletText("SAOV Azure Tools").Centered().Color(Color.Aqua)).Centered())
               .AddRow(AboutDetails.Get(TableBorder.None, new TableColumn(string.Empty), true).Centered());
            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }
    }
}
