namespace SAOV.CommandTools.AzTools.Commands.AzKeyVault
{
    using SAOV.CommandTools.AzTools.Helpers;
    using Spectre.Console;

    internal class AzKeyVaultList
    {
        public static bool Get()
        {
            AzKeyVaultEntity[] azKeyVaultEntity = [];
            AnsiConsole.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Default)
                .SpinnerStyle(Style.Parse("green bold"))
                .Start("[yellow]Getting data[/]", (Action<StatusContext>)(ctx =>
                {
                    azKeyVaultEntity = JsonHelper.GetEntity<AzKeyVaultEntity[]>(AzHelper.GetAzureInfo(AzCommands.AzKeyVaultList));
                }));
            if (azKeyVaultEntity != null)
            {
                Map(azKeyVaultEntity);
                return true;
            }
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Markup("[red]Verify that you are authenticated in Azure.[/]"));
            AnsiConsole.WriteLine();
            return false;
        }

        private static void Map(AzKeyVaultEntity[] azKeyVaultEntity)
        {
            var tableProperties = new Table()
                .Centered()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddColumn(new TableColumn(string.Empty).LeftAligned());
            azKeyVaultEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
            {
                tableProperties.AddRow($"[93]{item.Name}[/]", $"[yellow]{item.ResourceGroup}[/]", $"{item.Location}[/]");
            });
            tableProperties.AddEmptyRow();
            var tablePrincipal = new Table()
                .Border(TableBorder.Square)
                .Caption(($"[blue]Total KeyVaults {azKeyVaultEntity.Length}[/]"))
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn(new Markup("[aqua]Azure KeyVaults[/]")).Centered())
                .AddRow(tableProperties);
            AnsiConsole.Write(tablePrincipal);
        }
    }
}
