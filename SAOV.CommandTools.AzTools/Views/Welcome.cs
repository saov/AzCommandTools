namespace SAOV.CommandTools.AzTools.Views
{
    using Spectre.Console;

    internal static class Welcome
    {
        internal static void Get()
        {
            var tablePrincipal = new Table()
                .Border(TableBorder.Square)
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn(new FigletText("SAOV Azure Tools").Centered().Color(Color.Aqua)).Centered())
                .AddRow(new Markup("[40]SAOV Azure Tools[/]").Centered())
                .AddRow(new Markup("[red]Powered by C#[/]").Centered())
                .AddRow(new Markup("[white]v1.0.0[/]").Centered())
                .AddRow(new Markup("[yellow]saov@outlook.com[/]").Centered())
                .AddRow(new Markup($"[93]{DateTime.Now.Year}[/]").Centered());
            AnsiConsole.Write(tablePrincipal);
            Menu.Get();
        }
    }
}
