namespace SAOV.CommandTools.AzTools.Commands.AzAccountShow
{
    using SAOV.CommandTools.AzTools.Helpers;
    using Spectre.Console;

    internal static class AzAccountShow
    {
        internal static bool Get()
        {
            AzAccountShowEntity? azAccountShowEntity = null;
            AnsiConsole.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Default)
                .SpinnerStyle(Style.Parse("green bold"))
                .Start("[yellow]Getting data[/]", ctx =>
                {
                    azAccountShowEntity = JsonHelper.GetEntity<AzAccountShowEntity>(AzHelper.GetAzureInfo(AzCommands.AccountShow));
                });
            if (azAccountShowEntity != null)
            {
                Map(azAccountShowEntity);
                return true;
            }
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Markup("[red]Verify that you are authenticated in Azure.[/]"));
            AnsiConsole.WriteLine();
            return false;
        }

        private static void Map(AzAccountShowEntity azAccountShowEntity)
        {
            Table tableProperties = new Table()
                .Centered()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddColumn(new TableColumn(string.Empty).RightAligned())
                .AddRow(new Markup("[93]Id[/]").LeftJustified(), new Markup($"[40]{azAccountShowEntity.Id}[/]").Centered())
                .AddRow(new Markup("[93]Name[/]").LeftJustified(), new Markup($"[40]{azAccountShowEntity.Name}[/]").Centered())
                .AddRow(new Markup("[93]State[/]").LeftJustified(), new Markup($"[40]{azAccountShowEntity.State}[/]").Centered())
                .AddRow(new Markup("[93]TenantDisplayName[/]").LeftJustified(), new Markup($"[40]{azAccountShowEntity.TenantDisplayName}[/]").Centered())
                .AddRow(new Markup("[93]TenantId[/]").LeftJustified(), new Markup($"[40]{azAccountShowEntity.TenantId}[/]").Centered())
                .AddRow(new Markup("[93]UserName[/]").LeftJustified(), new Markup($"[40]{azAccountShowEntity.UserName}[/]").Centered())
                .AddRow(new Markup("[93]UserType[/]").LeftJustified(), new Markup($"[40]{azAccountShowEntity.UserType}[/]").Centered());
            Table tablePrincipal = new Table()
                .Border(TableBorder.Square)
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn(new Markup("[aqua]Azure Account Info[/]")).Centered())
                .AddRow(tableProperties);
            AnsiConsole.Write(tablePrincipal);
        }
    }
}
