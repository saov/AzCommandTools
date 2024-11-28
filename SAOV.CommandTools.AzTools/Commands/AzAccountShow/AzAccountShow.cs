namespace SAOV.CommandTools.AzTools.Commands.AzAccountShow
{
    using SAOV.CommandTools.AzTools.Helpers;
    using Spectre.Console;

    internal static class AzAccountShow
    {
        internal static bool Get()
        {
            AnsiConsole.Status()
            .AutoRefresh(true)
            .Spinner(Spinner.Known.Default)
            .SpinnerStyle(Style.Parse("green bold"))
            .Start("[yellow]Getting data[/]", ctx =>
            {
                ctx.Spinner(Spinner.Known.Earth);
                ctx.SpinnerStyle(Style.Parse("green"));
                AzAccountShowEntity azAccountShowEntity = JsonHelper.GetEntity<AzAccountShowEntity>(AzHelper.GetAzureInfo(AzCommands.AccountShow));
                ctx.Spinner(Spinner.Known.Default);
                ctx.SpinnerStyle(Style.Parse("green bold"));
                var table1Properties = new Table()
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
                var tablePrincipal = new Table()
                    .Border(TableBorder.Square)
                    .BorderColor(Color.Grey)
                    .AddColumn(new TableColumn(new Markup("[aqua]Azure-Account-Show[/]")).Centered())
                    .AddRow(table1Properties);
                AnsiConsole.Write(tablePrincipal);
            });
            return true;
        }
    }
}
