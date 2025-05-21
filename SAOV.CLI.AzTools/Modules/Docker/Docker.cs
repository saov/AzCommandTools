namespace SAOV.CLI.AzTools.Modules.Docker
{
    using SAOV.CLI.AzTools.Components;
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using Spectre.Console;
    using System;

    internal static class Docker
    {
        public static bool Show()
        {
            List<string> choices = [];
            bool run = false;
            Enum.GetValues<DockerMenu>().ToList().ForEach(item => { choices.Add(item.ToString()); });
            while (!run)
            {
                ModuleHeader.Show("/Docker");
                string selectionPromptValue = SelectionPrompt.Show(choices);
                _ = Enum.TryParse(selectionPromptValue, out DockerMenu azMenu);
                run = azMenu switch
                {
                    DockerMenu.RemoveAllUnusedContainersNetworksImagesAndVolumes => RemoveAllUnusedContainersNetworksImagesAndVolumes(),
                    DockerMenu.Exit => true,
                    _ => false
                };
            }
            return run;
        }

        internal static bool RemoveAllUnusedContainersNetworksImagesAndVolumes()
        {
            AnsiConsole.Clear();
            string moduleName = "/Docker/RemoveAllUnusedContainersNetworksImagesAndVolumes";
            ModuleHeader.Show(moduleName);
            string? result = CommandHelper.Run<string>(AzCommands.Docker_SystemPrune, [], showStandardError: false, outputIsPlainText: true);
            AnsiConsole.Clear();
            ModuleHeader.Show(moduleName);
            AnsiConsole.Write(new Markup($"[Magenta1]{result}[/]"));
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Markup("[40]Command Generated Successfully.[/]"));
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
            AnsiConsole.WriteLine();
            _ = Console.ReadKey();
            return true;
        }
    }
}
