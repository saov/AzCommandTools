namespace SAOV.CommandTools.AzTools.Commands.AzLogin
{
    using SAOV.CommandTools.AzTools.Helpers;
    using Spectre.Console;

    internal static class AzLogin
    {
        public static bool Get()
        {
            AzLoginEntity[] azLoginEntity = [];
            AnsiConsole.WriteLine();
            var tenantId = AnsiConsole.Prompt(new TextPrompt<string>("[93]Provide the [40]tenant id[/] :[/]")
                .Validate(tenantId => !string.IsNullOrWhiteSpace(tenantId) ?
                    ValidationResult.Success() :
                    ValidationResult.Error("[red]Field is required.[/]")
            ));
            var serviceIdentity = AnsiConsole.Prompt(new TextPrompt<string>("[93]Provide the [40]service identity[/] :[/]")
                .Validate(serviceIdentity => !string.IsNullOrWhiteSpace(tenantId) ?
                    ValidationResult.Success() :
                    ValidationResult.Error("[red]Field is required.[/]")
            ));
            var secret = AnsiConsole.Prompt(new TextPrompt<string>("[93]Provide the [40]secret[/] :[/]").Secret()
                .Validate(secret => !string.IsNullOrWhiteSpace(tenantId) ?
                    ValidationResult.Success() :
                    ValidationResult.Error("[red]Field is required.[/]")
            ));
            AnsiConsole.Status()
            .AutoRefresh(true)
            .Spinner(Spinner.Known.Default)
            .SpinnerStyle(Style.Parse("green bold"))
            .Start("[yellow]Getting data[/]", (Action<StatusContext>)(ctx =>
            {
                azLoginEntity = JsonHelper.GetEntity<AzLoginEntity[]>(AzHelper.GetAzureInfo(AzCommands.AzLogin.Replace("@@@User", serviceIdentity).Replace("@@@Tenant", tenantId).Replace("@@@Secret", secret)));
            }));
            if (azLoginEntity != null)
            {
                Map(azLoginEntity);
                return true;
            }
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Markup("[red]Unsuccessful login.[/]"));
            AnsiConsole.WriteLine();
            return true;
        }

        private static void Map(AzLoginEntity[] azLoginEntity)
        {
        Table tableProperties = new Table()
            .Centered()
            .Border(TableBorder.Square)
            .BorderColor(Color.Grey)
            .Caption(($"[blue]Total Subscriptions {azLoginEntity.Length}[/]"))
            .AddColumn(new TableColumn("Name").LeftAligned())
            .AddColumn(new TableColumn("Id").Centered())
            .AddColumn(new TableColumn("State").Centered())
            .AddColumn(new TableColumn("IsDefault").Centered());
        azLoginEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
        {
            string stateColor = item.State == "Enabled" ? "40" : "red";
            string isDefault = item.IsDefault ? "40" : "red";
            tableProperties.AddRow($"[93]{item.Name}[/]", $"[yellow]{item.Id}[/]", $"[{stateColor}]{item.State}[/]", $"[{isDefault}]{item.IsDefault}[/]");
        });
        tableProperties.AddEmptyRow();
            Table tablePrincipal = new Table()
            .Border(TableBorder.Square)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn(new Markup("[aqua]Azure Subscriptions[/]")).Centered())
            .AddRow(tableProperties);
        AnsiConsole.Write(tablePrincipal);
        }
    }
}
