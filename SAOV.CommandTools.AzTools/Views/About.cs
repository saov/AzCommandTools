namespace SAOV.CommandTools.AzTools.Views
{
    using Spectre.Console;

    internal static class About
    {
        internal static bool Get()
        {
            AnsiConsole.Write(AboutDetails.Get(TableBorder.Square, new TableColumn("[aqua]About SAOV Azure Tools...[/]"), false));
            return true;
        }
    }
}
