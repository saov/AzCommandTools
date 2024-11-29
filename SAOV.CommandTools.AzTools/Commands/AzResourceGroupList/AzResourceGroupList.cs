namespace SAOV.CommandTools.AzTools.Commands.AzResourceGroupList
{
    using SAOV.CommandTools.AzTools.Helpers;
    using Spectre.Console;

    internal class AzResourceGroupList
    {
        public static bool Get()
        {
            AzResourceGroupListEntity[] azResourceGroupListEntity = [];
            AnsiConsole.Status()
            .AutoRefresh(true)
            .Spinner(Spinner.Known.Default)
            .SpinnerStyle(Style.Parse("green bold"))
            .Start("[yellow]Getting data[/]", (Action<StatusContext>)(ctx =>
            {
                azResourceGroupListEntity = JsonHelper.GetEntity<AzResourceGroupListEntity[]>(AzHelper.GetAzureInfo(AzCommands.AzResourceGroupList));
            }));
            if (azResourceGroupListEntity != null)
            {
                Map(azResourceGroupListEntity);
                return true;
            }
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Markup("[red]Verify that you are authenticated in Azure.[/]"));
            AnsiConsole.WriteLine();
            return false;
        }

        private static void Map(AzResourceGroupListEntity[] azResourceGroupListEntity)
        {
            Table tableProperties = new Table()
                .Centered()
                .Border(TableBorder.None)
                .Caption(($"[blue]Total Resource Groups {azResourceGroupListEntity.Length}[/]"))
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddColumn(new TableColumn(string.Empty).LeftAligned());
            azResourceGroupListEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
            {
                string stateColor = item.ProvisioningState == "Succeeded" ? "40" : "red";
                tableProperties.AddRow($"[93]{item.Name}[/]", $"[yellow]{item.Location}[/]", $"[{stateColor}]{item.ProvisioningState}[/]");
            });
            tableProperties.AddEmptyRow();
            Table tablePrincipal = new Table()
            .Border(TableBorder.Square)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn(new Markup("[aqua]Azure ResourceGroups[/]")).Centered())
            .AddRow(tableProperties);
            AnsiConsole.Write(tablePrincipal);
        }
    }
}
