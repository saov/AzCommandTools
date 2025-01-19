namespace SAOV.CLI.AzTools.Components
{
    using Spectre.Console;

    internal static class Status
    {
        internal static T Show<T>(string message, Func<T> function)
        {
            if (function== null)
            {
                throw new NullReferenceException();
            }
            object result = null;
            AnsiConsole.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Default)
                .SpinnerStyle(Style.Parse("green bold"))
                .Start($"[yellow]{message}[/]", (Action<StatusContext>)(ctx =>
                {
                    result = function();
                }));
            return (T)result;
        }
    }
}
