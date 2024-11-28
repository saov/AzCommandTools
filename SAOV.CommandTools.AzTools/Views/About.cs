namespace SAOV.CommandTools.AzTools.Views
{
    using Spectre.Console;
    using System;

    internal static class About
    {
        internal static bool Get()
        {
            var table1Properties = new Table()
                .Centered()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn(string.Empty).Centered())
                .AddRow(new Markup("[40]SAOV Azure Tools[/]").Centered())
                .AddRow(new Markup("[red]Powered by C#[/]").Centered())
                .AddRow(new Markup("[white]v1.0.0[/]").Centered())
                .AddRow(new Markup("[yellow]saov@outlook.com[/]").Centered())
                .AddRow(new Markup($"[93]{DateTime.Now.Year}[/]").Centered());
            var tablePrincipal = new Table()
                .Border(TableBorder.Square)
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn(new Markup("[aqua]About[/]")).Centered())
                .AddRow(table1Properties);
            AnsiConsole.Write(tablePrincipal);
            return true;
        }
    }
}
