namespace SAOV.CLI.AzTools.Modules.Kubernetes
{
    using SAOV.CLI.AzTools.Components;
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using Spectre.Console;
    using System;

    internal static class Kubernetes
    {
        public static bool Show()
        {
            List<string> choices = [];
            bool run = false;
            Enum.GetValues<KubernetesMenu>().ToList().ForEach(item => { choices.Add(item.ToString()); });
            while (!run)
            {
                ModuleHeader.Show("/Kubernetes");
                string selectionPromptValue = SelectionPrompt.Show(choices);
                _ = Enum.TryParse(selectionPromptValue, out KubernetesMenu azMenu);
                run = azMenu switch
                {
                    KubernetesMenu.AssignContext => AssignContext(),
                    KubernetesMenu.SetNameSpaces => SetNameSpaces(),
                    KubernetesMenu.DecodeSecret => DecodeSecret(),
                    KubernetesMenu.DeletePodsNotRunning => DeletePodsNotRunning(),
                    KubernetesMenu.Exit => true,
                    _ => false
                };
            }
            return run;
        }

        internal static bool AssignContext()
        {
            string pathKubernetesContexts = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".kube");
            string[] contexts = Directory.GetFiles(pathKubernetesContexts);
            Dictionary<string, string> allContexts = [];
            List<string> choices = [];
            foreach (string context in contexts)
            {
                if (context != Path.Combine(pathKubernetesContexts, "config"))
                {
                    string? contextFile = File.ReadLines(context).Where(line => line.StartsWith("current-context:")).FirstOrDefault();
                    if (contextFile != null)
                    {
                        string contextName = $"{contextFile.Replace("current-context:", string.Empty)} ({Path.GetFileName(context)})";
                        allContexts.Add(contextName, context);
                        choices.Add(contextName);
                    }
                }
            }
            choices = choices.OrderBy(t => t).ToList();
            choices.Add(AzCommands.Choise_Cancel);
            bool showChoises = true;
            while (showChoises)
            {
                AnsiConsole.Clear();
                string moduleName = "/Kubernetes/AssignContext";
                ModuleHeader.Show(moduleName);
                string contextSelected = SelectionPrompt.Show(choices);
                if (contextSelected == AzCommands.Choise_Cancel)
                {
                    showChoises = false;
                    return true;
                }
                string pathContextSelected = allContexts.Where(t => t.Key == contextSelected).First().Value;
                File.Copy(pathContextSelected, Path.Combine(pathKubernetesContexts, "config"), true);
                AzCommand.KubernetesCurrentNameSpace = string.Empty;
                AzCommand.KubernetesCurrentContext = CommandHelper.Run<string>(AzCommands.Kubernetes_CurrentContext, [], showStandardError: false, outputIsPlainText: true)?.Replace("\r\n", string.Empty)
                                                                                                                                                                            .Replace("\r", string.Empty)
                                                                                                                                                                            .Replace("\n", string.Empty);
                showChoises = false;
                AnsiConsole.Write(new Markup("[40]Success operation.[/]"));
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                AnsiConsole.WriteLine();
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        internal static bool SetNameSpaces()
        {
            string[]? kubernetesNameSpaces = CommandHelper.Run<string>(AzCommands.Kubernetes_ListNameSpaces, [], showStandardError: false, outputIsPlainText: true)?
                                                    .Replace("\r", string.Empty)
                                                    .Split("\n", StringSplitOptions.RemoveEmptyEntries);
            List<string> choices = [];
            if (kubernetesNameSpaces != null)
            {
                choices.AddRange(kubernetesNameSpaces);
                choices = choices.OrderBy(t => t).ToList();
            }
            choices.Add(AzCommands.Choise_Cancel); bool showChoises = true;
            while (showChoises)
            {
                AnsiConsole.Clear();
                string moduleName = "/Kubernetes/SetNameSpaces";
                ModuleHeader.Show(moduleName);
                string nameSpaceSelected = SelectionPrompt.Show(choices);
                if (nameSpaceSelected == AzCommands.Choise_Cancel)
                {
                    showChoises = false;
                    return true;
                }
                showChoises = false;
                AzCommand.KubernetesCurrentNameSpace = nameSpaceSelected;
                AnsiConsole.Clear();
                ModuleHeader.Show(moduleName);
                AnsiConsole.Write(new Markup("[40]Success operation.[/]"));
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();
                AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
                AnsiConsole.WriteLine();
                _ = Console.ReadKey();
                return true;
            }
            return false;
        }

        internal static bool DecodeSecret()
        {
            string templateCommand = "kubectl get secret @@@SecretName -n @@@NameSpeaceName -o go-template='{{range $k,$v := .data}}{{printf \"%s: \" $k}}{{if not $v}}{{$v}}{{else}}{{$v | base64decode}}{{end}}{{\"\\n\"}}{{end}}'";
            string[]? kubernetesNameSpaces = CommandHelper.Run<string>(AzCommands.Kubernetes_ListNameSpaces, [], showStandardError: false, outputIsPlainText: true)?
                                        .Replace("\r", string.Empty)
                                        .Split("\n", StringSplitOptions.RemoveEmptyEntries);
            List<string> choices = [];
            if (kubernetesNameSpaces != null)
            {
                choices.AddRange(kubernetesNameSpaces);
                choices = choices.OrderBy(t => t).ToList();
            }
            choices.Add(AzCommands.Choise_Cancel);
            bool showChoises = true;
            while (showChoises)
            {
                AnsiConsole.Clear();
                string moduleName = "/Kubernetes/DecodeSecret";
                ModuleHeader.Show(moduleName);
                string nameSpaceSelected = SelectionPrompt.Show(choices);
                if (nameSpaceSelected == AzCommands.Choise_Cancel)
                {
                    showChoises = false;
                    return true;
                }
                List<string> choicesSecrets = [];
                string[]? kubernetesSecrets = CommandHelper.Run<string>(AzCommands.Kubernetes_ListSecrets, new() { { "@@@NameSpaceName", nameSpaceSelected } }, showStandardError: false, outputIsPlainText: true)?
                                                           .Replace("\r", string.Empty)
                                                           .Split("\n", StringSplitOptions.RemoveEmptyEntries);
                if (kubernetesSecrets != null)
                {
                    choicesSecrets.AddRange(kubernetesSecrets);
                    choicesSecrets = choicesSecrets.OrderBy(t => t).ToList();
                }
                choicesSecrets.Add(AzCommands.Choise_Cancel);
                bool showChoisesSecrets = true;
                while (showChoisesSecrets)
                {
                    AnsiConsole.Clear();
                    moduleName = "/Kubernetes/DecodeSecret";
                    ModuleHeader.Show(moduleName);
                    string secretSelected = SelectionPrompt.Show(choicesSecrets);
                    if (secretSelected == AzCommands.Choise_Cancel)
                    {
                        showChoisesSecrets = false;
                        return true;
                    }
                    templateCommand = templateCommand.Replace("@@@SecretName", secretSelected)
                                                     .Replace("@@@NameSpeaceName", nameSpaceSelected);
                    AnsiConsole.Clear();
                    ModuleHeader.Show(moduleName);
                    AnsiConsole.WriteLine();
                    AnsiConsole.Write(new Markup($"[40]Command :[/]"));
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine();
                    AnsiConsole.Write(new Markup($"[Magenta1]{templateCommand}[/]"));
                    AnsiConsole.WriteLine();
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
            return false;
        }

        internal static bool DeletePodsNotRunning()
        {
            string[]? kubernetesNameSpaces = CommandHelper.Run<string>(AzCommands.Kubernetes_ListNameSpaces, [], showStandardError: false, outputIsPlainText: true)?
                                                          .Replace("\r", string.Empty)
                                                          .Split("\n", StringSplitOptions.RemoveEmptyEntries);
            List<string> choices = [];
            if (kubernetesNameSpaces != null)
            {
                choices.AddRange(kubernetesNameSpaces);
                choices = choices.OrderBy(t => t).ToList();
            }
            choices.Add(AzCommands.Choise_Cancel); bool showChoises = true;
            while (showChoises)
            {
                AnsiConsole.Clear();
                string moduleName = "/Kubernetes/DeletePodsNotRunning";
                ModuleHeader.Show(moduleName);
                string nameSpaceSelected = SelectionPrompt.Show(choices);
                if (nameSpaceSelected == AzCommands.Choise_Cancel)
                {
                    showChoises = false;
                    return true;
                }
                return CommandHelper.Run(AzCommands.Kubernetes_DeletePodsNotRunning, new() { { "@@@NameSpaceName", nameSpaceSelected } });
            }
            return false;
        }
    }
}
