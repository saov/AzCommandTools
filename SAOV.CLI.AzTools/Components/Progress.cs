namespace SAOV.CLI.AzTools.Components
{
    using SAOV.CLI.AzTools.Helpers;
    using Spectre.Console;

    internal class Progress
    {
        internal static List<T> Show<T>(string messageTask, int steps, string command, List<Dictionary<string, string>> itemsWithParamentersCommand)
        {
            List<T> results = [];
            AnsiConsole.Progress()
                .AutoClear(true)
                .Columns(
                [
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn().Style(Color.Yellow),
                    new SpinnerColumn().Style(Color.Green)
                ])
                .Start(ctx =>
                {
                    ProgressTask progressTask = ctx.AddTask($"[93]{messageTask}[/]");
                    while (!ctx.IsFinished)
                    {
                        for (int i = 0; i < itemsWithParamentersCommand.Count; i++)
                        {
                            results.Add(CommandHelper.Run<T>(command, itemsWithParamentersCommand[i], true));
                            progressTask.Increment(i + 1 != itemsWithParamentersCommand.Count ? 100 / itemsWithParamentersCommand.Count : 100);
                        }
                    }
                });
            return results;
        }
    }
}
