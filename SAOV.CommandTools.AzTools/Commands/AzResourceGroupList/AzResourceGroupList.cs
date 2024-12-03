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
                var confirmation = AnsiConsole.Prompt(
                    new TextPrompt<bool>("Do you want to get Resources?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(false)
                    .WithConverter(choice => choice ? "y" : "n"));
                if (confirmation)
                {
                    GetResources(azResourceGroupListEntity);
                }
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
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddColumn(new TableColumn(string.Empty).LeftAligned())
                .AddColumn(new TableColumn(string.Empty).LeftAligned()
            );
            azResourceGroupListEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
            {
                string stateColor = item.ProvisioningState == "Succeeded" ? "40" : "red";
                tableProperties.AddRow($"[93]{item.Name}[/]", $"[yellow]{item.Location}[/]", $"[{stateColor}]{item.ProvisioningState}[/]");
            });
            tableProperties.AddEmptyRow();
            Table tablePrincipal = new Table()
                .Border(TableBorder.Square)
                .Caption(($"[blue]Total Resource Groups {azResourceGroupListEntity.Length}[/]"))
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn(new Markup("[aqua]Azure ResourceGroups[/]")).Centered())
                .AddRow(tableProperties);
            AnsiConsole.Write(tablePrincipal);
        }

        private static void GetResources(AzResourceGroupListEntity[] azResourceGroupListEntity)
        {
            List<string> choices = [];
            azResourceGroupListEntity.Where(t => t.ProvisioningState == "Succeeded").OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add(item.Name); });
            choices.Add($"[93](x) ([yellow]Cancel[/])[/]");
            Func<string, string> displaySelector = str =>
            {
                return $"[purple]{str}[/]";
            };
            string resource = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select a ResourceGroup to get Resources.[/]")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(choices)
                .UseConverter(displaySelector)
                .HighlightStyle(Style.Plain.Background(Color.Grey))
                .EnableSearch()
            );
            if (resource != "[93](x) ([yellow]Cancel[/])[/]")
            {
                AnsiConsole.Status()
                    .AutoRefresh(true)
                    .Spinner(Spinner.Known.Default)
                    .SpinnerStyle(Style.Parse("green bold"))
                    .Start("[yellow]Getting data[/]", ctx =>
                    {
                        AzResourceGroupResourcesEntity[] azResourceGroupResourcesEntity = JsonHelper.GetEntity<AzResourceGroupResourcesEntity[]>(AzHelper.GetAzureInfo(AzCommands.AzResourceGroupResources.Replace("@@@", resource)));
                        MapResources(azResourceGroupResourcesEntity, resource);
                    });
            }
        }

        private static void MapResources(AzResourceGroupResourcesEntity[] azResourceGroupResourcesEntity, string title)
        {
            Table tableProperties = new Table()
                .Centered()
                .Border(TableBorder.Square)
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn("Name").LeftAligned())
                .AddColumn(new TableColumn("Location").Centered())
                .AddColumn(new TableColumn("Type").LeftAligned())
                .AddColumn(new TableColumn("ProvisioningState").Centered());
            azResourceGroupResourcesEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
            {
                string provisioningStateColor = item.ProvisioningState == "Succeeded" ? "40" : "red";
                tableProperties.AddRow($"[93]{item.Name}[/]", $"[yellow]{item.Location}[/]", $"[purple]{item.Type}[/]", $"[{provisioningStateColor}]{item.ProvisioningState}[/]");
            });
            tableProperties.AddEmptyRow();
            Table tablePrincipal = new Table()
                .Border(TableBorder.Square)
                .Caption(($"[blue]Total Resources {azResourceGroupResourcesEntity.Length}[/]"))
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn(new Markup($"[aqua]{title}[/]")).Centered())
                .AddRow(tableProperties);
            AnsiConsole.Write(tablePrincipal);
        }
    }
}
