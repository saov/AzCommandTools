namespace SAOV.CLI.AzTools.Components
{
    using Spectre.Console;

    internal static class ModuleHeader
    {
        internal static void Show(string path)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Panel(new TextPath(path)
            {
                RootStyle = new Style(Color.Yellow),
                SeparatorStyle = new Style(Color.Yellow),
                StemStyle = new Style(Color.Green),
                LeafStyle = new Style(Color.Green),
                Justification = Justify.Center
            })
            {
                Header = new PanelHeader("[aqua]SAOV Azure Tools[/]")
            }.Expand());
            AnsiConsole.WriteLine();
        }
    }
}
