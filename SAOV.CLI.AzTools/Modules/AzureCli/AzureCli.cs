namespace SAOV.CLI.AzTools.Modules.AzureCli
{
    using SAOV.CLI.AzTools.Components;
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using SAOV.CLI.AzTools.Modules.AzureCli.Entities;
    using Spectre.Console;

    internal static class AzureCli
    {
        public static bool Show()
        {
            List<string> choices = [];
            bool run = false;
            Enum.GetValues<AzureCliMenu>().ToList().ForEach(item => { choices.Add(item.ToString()); });
            while (!run)
            {
                ModuleHeader.Show("/AzureCli");
                string selectionPromptValue = SelectionPrompt.Show(choices);
                _ = Enum.TryParse(selectionPromptValue, out AzureCliMenu azMenu);
                run = azMenu switch
                {
                    AzureCliMenu.AzCliUpgrade => AzCliUpgrade(),
                    AzureCliMenu.GetExtensionsInstalledList => GetExtensionsInstalledList(),
                    AzureCliMenu.GetExtensionsAvailableList => GetExtensionsAvailableList(),
                    AzureCliMenu.UpdateExtension => UpdateExtension(),
                    AzureCliMenu.AddExtension => AddExtension(),
                    AzureCliMenu.RemoveExtension => RemoveExtension(),
                    AzureCliMenu.Exit => true,
                    _ => false
                };
            }
            return run;
        }

        internal static bool AzCliUpgrade()
        {
            ModuleHeader.Show("/AzureCli/AzCliUpgrade");
            return CommandHelper.Run(AzCommands.AzureCli_Upgrade, []);
        }

        internal static bool GetExtensionsInstalledList()
        {
            string moduleName = "/AzureCli/GetExtensionsInstalledList";
            ModuleHeader.Show(moduleName);
            AzCliExtensionEntity[] azCliExtensionEntity = GetExtensionsInstalledListData();
            if (azCliExtensionEntity != null)
            {
                List<KeyValuePair<Markup, Justify>> columns =
                [
                    new(new("Name"), Justify.Left),
                    new(new("Version"), Justify.Center)
                ];
                List<List<Markup>> rows = [];
                azCliExtensionEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                {
                    rows.Add([new($"[93]{item.Name}[/]"), new($"[green]{item.Version}[/]")]);
                });
                string titleResult = $"[aqua]Azure Installed Extensions([40]{rows.Count}[/])[/]";
                FormatResults.Show<AzCliExtensionEntity[]>(azCliExtensionEntity, new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows), moduleName.Replace("/", "_"));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        internal static bool GetExtensionsAvailableList()
        {
            string moduleName = "/AzureCli/GetExtensionsAvailableList";
            ModuleHeader.Show(moduleName);
            AzCliExtensionEntity[] azCliExtensionEntity = GetExtensionsAvailableListData();
            if (azCliExtensionEntity != null)
            {
                List<KeyValuePair<Markup, Justify>> columns =
                [
                    new(new("Name"), Justify.Left),
                    new(new("Summary"), Justify.Left),
                    new(new("Version"), Justify.Center)
                ];
                List<List<Markup>> rows = [];
                azCliExtensionEntity.OrderBy(t => t.Name).ToList().ForEach(item =>
                {
                    rows.Add([new($"[93]{item.Name}[/]"), new($"[yellow]{item.Summary}[/]"), new($"[green]{item.Version}[/]")]);
                });
                string titleResult = $"[aqua]Azure Available Extensions([40]{rows.Count}[/])[/]";
                FormatResults.Show<AzCliExtensionEntity[]>(azCliExtensionEntity, new(titleResult), Components.Table.Show(true, titleResult, string.Empty, columns, rows), moduleName.Replace("/", "_"));
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        internal static bool UpdateExtension()
        {
            ModuleHeader.Show("/AzureCli/UpdateExtension");
            AzCliExtensionEntity[] azCliExtensionEntity = GetExtensionsInstalledListData();
            List<string> choices = [];
            azCliExtensionEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add($"{item.Name}({item.Version})"); });
            choices.Add($"[93](x) [yellow]Cancel[/][/]");
            if (azCliExtensionEntity != null)
            {
                string extension = SelectionPrompt.Show(choices);
                if (extension != "[93](x) [yellow]Cancel[/][/]")
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string extensionName = azCliExtensionEntity.Where(t => extension.Contains(t.Name))
                                .OrderBy(t => t.Name)
                                .Select(t => t.Name)
                                .First();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    return CommandHelper.Run(AzCommands.AzureCli_ExtensionUpdate, new() { { "@@@ExtensionName", extensionName } });
                }
            }
            return false;
        }

        internal static bool AddExtension()
        {
            ModuleHeader.Show("/AzureCli/AddExtension");
            AzCliExtensionEntity[] azCliExtensionEntity = GetExtensionsAvailableListData();
            List<string> choices = [];
            azCliExtensionEntity.ToList().ForEach(item => { choices.Add($"({item.Name}){item.Summary}({item.Version})"); });
            choices.Add($"[93](x) [yellow]Cancel[/][/]");
            if (azCliExtensionEntity != null)
            {
                string extension = SelectionPrompt.Show(choices);
                if (extension != "[93](x) [yellow]Cancel[/][/]")
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string extensionName = azCliExtensionEntity.Where(t => extension.Contains(t.Summary))
                                .OrderBy(t => t.Summary)
                                .Select(t => t.Name)
                                .First();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    return CommandHelper.Run(AzCommands.AzureCli_ExtensionAdd, new() { { "@@@ExtensionName", extensionName } });
                }
            }
            return false;
        }

        internal static bool RemoveExtension()
        {
            ModuleHeader.Show("/AzureCli/RemoveExtension");
            AzCliExtensionEntity[] azCliExtensionEntity = GetExtensionsInstalledListData();
            List<string> choices = [];
            azCliExtensionEntity.OrderBy(t => t.Name).ToList().ForEach(item => { choices.Add($"{item.Name}({item.Version})"); });
            choices.Add($"[93](x) [yellow]Cancel[/][/]");
            if (azCliExtensionEntity != null)
            {
                string extension = SelectionPrompt.Show(choices);
                if (extension != "[93](x) [yellow]Cancel[/][/]")
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string extensionName = azCliExtensionEntity.Where(t => extension.Contains(t.Name))
                                .OrderBy(t => t.Name)
                                .Select(t => t.Name)
                                .First();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    return CommandHelper.Run(AzCommands.AzureCli_ExtensionRemove, new() { { "@@@ExtensionName", extensionName } });
                }
            }
            return false;
        }

        private static AzCliExtensionEntity[] GetExtensionsInstalledListData()
        {
            return CommandHelper.Run<AzCliExtensionEntity[]>(AzCommands.AzureCli_ExtensionsInstalledList, []);
        }

        private static AzCliExtensionEntity[] GetExtensionsAvailableListData()
        {
            string command = AzCommands.AzureCli_ExtensionsAvailableList;
            command = !string.IsNullOrWhiteSpace(AzCommand.AzureQueryFilters) ?
                                                                                command.Replace("@@@AzureQueryFilter", AzCommand.AzureQueryFilters).Replace("@@@AzureQueryFilterPropertyName", "name") :
                                                                                command.Replace("@@@AzureQueryFilter", "");
            return CommandHelper.Run<AzCliExtensionEntity[]>(command, []);
        }
    }
}
