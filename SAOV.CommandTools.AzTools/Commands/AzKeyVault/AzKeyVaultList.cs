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
                bool run = true;
                Map(azKeyVaultEntity);
                if (azKeyVaultEntity.Length > 0)
                {
                    while (run)
                    {
                        bool confirmation = AnsiConsole.Prompt(
                            new TextPrompt<bool>("Do you want to obtain the secrets of any KeyVault?")
                            .AddChoice(true)
                            .AddChoice(false)
                            .DefaultValue(false)
                            .WithConverter(choice => choice ? "y" : "n"));
                        if (confirmation)
                        {
                            GetSecrets(azKeyVaultEntity);
                        }
                        else
                        {
                            run = false;
                        }
                    }
                }
                return true;
            }
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Markup("[red]Verify that you are authenticated in Azure.[/]"));
            AnsiConsole.WriteLine();
            return false;
        }

        private static void GetSecrets(AzKeyVaultEntity[] azKeyVaultEntity)
        {
            List<string> choices = [];
            azKeyVaultEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add(item.Name); });
            choices.Add($"[93](x) ([yellow]Cancel[/])[/]");
            Func<string, string> displaySelector = str =>
            {
                return $"[purple]{str}[/]";
            };
            string keyVaultName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select a KeyVault to View Secrets.[/]")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(choices)
                .UseConverter(displaySelector)
                .HighlightStyle(Style.Plain.Background(Color.Grey))
                .EnableSearch()
            );
            if (keyVaultName != "[93](x) ([yellow]Cancel[/])[/]")
            {
                AnsiConsole.Status()
                    .AutoRefresh(true)
                    .Spinner(Spinner.Known.Default)
                    .SpinnerStyle(Style.Parse("green bold"))
                    .Start("[yellow]Getting data[/]", ctx =>
                    {
                        AzKeyVaultSecretEntity[] azKeyVaultSecretEntity = JsonHelper.GetEntity<AzKeyVaultSecretEntity[]>(AzHelper.GetAzureInfo(AzCommands.AzKeyVaultSecretsList.Replace("@@@", keyVaultName)));
                        if (azKeyVaultSecretEntity != null)
                        {
                            MapSecrets(keyVaultName, azKeyVaultSecretEntity);
                        }
                    });
            }
        }

        private static void GetValueSecret(string keyVaultName, string keyVaultSecret)
        {
            var result = AzHelper.GetAzureInfo(AzCommands.AzGetKeyVaultSecretValue.Replace("@@@KeyVault", keyVaultName).Replace("@@@SecretName", keyVaultSecret));
            if (result.Success)
            {
                var tableProperties = new Table()
                    .Centered()
                    .Border(TableBorder.None)
                    .AddColumn(new TableColumn(string.Empty).LeftAligned());
                tableProperties.AddRow($"[yellow]{result.Output}[/]");
                var tablePrincipal = new Table()
                    .Border(TableBorder.Square)
                    .BorderColor(Color.Grey)
                    .AddColumn(new TableColumn(new Markup($"[aqua]Azure KeyVault Value ([93]{keyVaultName}[/]/[40]{keyVaultSecret}[/])[/]")).Centered())
                    .AddRow(tableProperties);
                AnsiConsole.Write(tablePrincipal);
            }
        }

        private static void MapSecrets(string keyVaultName, AzKeyVaultSecretEntity[] azKeyVaultSecretEntity)
        {
            Func<string, string> displaySelector = str =>
            {
                return $"[purple]{str}[/]";
            };
            List<string> choicesSecrets = [];
            azKeyVaultSecretEntity.Where(t => t.Enabled).OrderBy(t => t.Name).ToList().ForEach(item => { choicesSecrets.Add(item.Name); });
            choicesSecrets.Add($"[93](x) ([yellow]Cancel[/])[/]");
            bool run = true;
            while (run)
            {
                string keyVaultSecret = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select a KeyVaultSecret to View value.[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(choicesSecrets)
                    .UseConverter(displaySelector)
                    .HighlightStyle(Style.Plain.Background(Color.Grey))
                    .EnableSearch()
                );
                if (keyVaultSecret != "[93](x) ([yellow]Cancel[/])[/]")
                {
                    GetValueSecret(keyVaultName, keyVaultSecret);
                }
                else
                {
                    run = false;
                }
            }
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
                tableProperties.AddRow($"[93]{item.Name}[/]", $"[40]{item.ResourceGroup}[/]", $"[yellow]{item.Location}[/]");
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
