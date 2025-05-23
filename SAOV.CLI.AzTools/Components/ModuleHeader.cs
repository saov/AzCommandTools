﻿namespace SAOV.CLI.AzTools.Components
{
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Modules.Account.Entities;
    using Spectre.Console;
    using System.Reflection;

    internal static class ModuleHeader
    {
        internal static void Show(string path)
        {
            AzAccountShowEntity azAccountShowEntity = CommandHelper.Run<AzAccountShowEntity>(AzCommands.Account_Show, [], false, false);
            string filters = string.IsNullOrWhiteSpace(AzCommand.Filters) ? string.Empty : AzCommand.Filters;
            string subscription = azAccountShowEntity == null ?
                                    string.Empty : 
                                    $"[green]{azAccountShowEntity?.Name} ([Magenta1]{azAccountShowEntity?.Id}[/])[/]";
            Grid grid = new();
            grid.AddColumn();
            grid.AddRow([
                new Markup($"[navy]Subscription [/][yellow]: [/]{subscription}").LeftJustified()
            ]);
            grid.AddRow([
                new Markup($"[navy]Module [/][yellow]: [/][orange3]{path}[/]").LeftJustified()
            ]);
            grid.AddRow([
                new Markup($"[navy]Azure Filter [/][yellow]: [/][red]{filters}[/]").RightJustified().LeftJustified()
            ]);
            grid.AddRow([
                new Markup($"[navy]Kubernetes Context [/][yellow]: [/][40]{AzCommand.KubernetesCurrentContext}[/]").RightJustified().LeftJustified()
            ]);
            grid.AddRow([
                new Markup($"[navy]Kubernetes NameSpace [/][yellow]: [/][40]{AzCommand.KubernetesCurrentNameSpace}[/]").RightJustified().LeftJustified()
            ]);
            AnsiConsole.Clear();
            AnsiConsole.Write(new Panel(grid)
            {
                Header = new PanelHeader($"[Turquoise2]SAOV Azure Tools [Gold1]v{Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString()}[/] in [40]{Environment.UserName}[red]@[/]{Environment.MachineName}[/][/]")
            });
            AnsiConsole.WriteLine();
        }
    }
}
