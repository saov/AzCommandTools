namespace SAOV.CommandTools.AzTools.Views
{
    using Spectre.Console;

    internal static class Welcome
    {
        internal static void Get()
        {
            Table table = new Table()
                .Border(TableBorder.Square)
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn(new FigletText("SAOV Azure Tools").Centered().Color(Color.Aqua)).Centered())
                .AddRow(AboutDetails.Get(TableBorder.None, new TableColumn(string.Empty)).Centered());
            AnsiConsole.Write(table);
            Menu.Get();
        }
    }
}
