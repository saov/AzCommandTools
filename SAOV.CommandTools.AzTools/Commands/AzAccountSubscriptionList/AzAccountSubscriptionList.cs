namespace SAOV.CommandTools.AzTools.Commands.AzAccountSubscriptionList
{
    using SAOV.CommandTools.AzTools.Helpers;
    using Spectre.Console;

    internal static class AzAccountSubscriptionList
    {
        public static bool Get()
        {
            AzAccountSubscriptionListEntity[] azAccountSubscriptionListEntity = [];
            AnsiConsole.Status()
            .AutoRefresh(true)
            .Spinner(Spinner.Known.Default)
            .SpinnerStyle(Style.Parse("green bold"))
            .Start("[yellow]Getting data[/]", (Action<StatusContext>)(ctx =>
            {
                azAccountSubscriptionListEntity = JsonHelper.GetEntity<AzAccountSubscriptionListEntity[]>(AzHelper.GetAzureInfo(AzCommands.AzAccountSubscriptionList));
            }));
            if (azAccountSubscriptionListEntity != null)
            {
                Map(azAccountSubscriptionListEntity);
                var confirmation = AnsiConsole.Prompt(
                new TextPrompt<bool>("Do you want to change Subscription?")
                        .AddChoice(true)
                        .AddChoice(false)
                        .DefaultValue(false)
                        .WithConverter(choice => choice ? "y" : "n"));
                if (confirmation)
                {
                    SetSubscription(azAccountSubscriptionListEntity);
                }
                return true;
            }
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Markup("[red]Verify that you are authenticated in Azure.[/]"));
            AnsiConsole.WriteLine();
            return false;
        }

        private static void Map(AzAccountSubscriptionListEntity[] azAccountSubscriptionListEntity)
        {
            var tableProperties = new Table()
                .Centered()
                .Border(TableBorder.None)
                .Caption(($"[blue]Total Subscriptions {azAccountSubscriptionListEntity.Length}[/]"))
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddColumn(new TableColumn(string.Empty).LeftAligned());
            azAccountSubscriptionListEntity.OrderBy(t => t.DisplayName).ToList().ForEach(item =>
            {
                string stateColor = item.State == "Enabled" ? "40" : "red";
                tableProperties.AddRow($"[93]{item.DisplayName}[/]", $"[yellow]{item.SubscriptionId}[/]", $"[{stateColor}]{item.State}[/]");
            });
            tableProperties.AddEmptyRow();
            var tablePrincipal = new Table()
                .Border(TableBorder.Square)
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn(new Markup("[aqua]Azure Subscriptions[/]")).Centered())
                .AddRow(tableProperties);
            AnsiConsole.Write(tablePrincipal);
        }

        private static void SetSubscription(AzAccountSubscriptionListEntity[] azAccountSubscriptionListEntity)
        {
            List<string> choices = [];
            azAccountSubscriptionListEntity.Where(t => t.State == "Enabled").OrderBy(t => t.DisplayName).ToList().ForEach(item => { choices.Add($"[purple]{item.DisplayName} ([yellow]{item.SubscriptionId}[/])[/]"); });
            choices.Add($"[93](x) ([yellow]Cancel[/])[/]");
            string subscription = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select a subscription to assign.[/]")
                .PageSize(5)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(choices)
                .HighlightStyle(Style.Plain.Background(Color.Grey))
                .EnableSearch());
            if (subscription != "[93](x) ([yellow]Cancel[/])[/]")
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                string subscriptionId = azAccountSubscriptionListEntity.Where(t => t.State == "Enabled" && subscription.Contains(t.SubscriptionId))
                                .OrderBy(t => t.DisplayName)
                                .Select(t => t.SubscriptionId)
                                .First();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                AnsiConsole.Status()
                    .AutoRefresh(true)
                    .Spinner(Spinner.Known.Default)
                    .SpinnerStyle(Style.Parse("green bold"))
                    .Start("[yellow]Getting data[/]", ctx =>
                    {
                        (bool Success, string Output) result = AzHelper.GetAzureInfo(AzCommands.AzAccountSetSubscription.Replace("@@@", subscriptionId));
                        string stateColor = result.Success ? "40" : "red";
                        string message = result.Success ? "success" : "not success";
                        AnsiConsole.WriteLine();
                        AnsiConsole.Write(new Markup($"[93]The subscription change was [/][{stateColor}]{message}[/]"));
                        AnsiConsole.WriteLine();
                    });
            }
        }
    }
}
