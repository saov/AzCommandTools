namespace SAOV.CommandTools.AzTools.Commands.AzLogOut
{
    using SAOV.CommandTools.AzTools.Helpers;
    using Spectre.Console;

    internal class AzLogOut
    {
        public static bool Get()
        {
            (bool Success, string Output) result = AzHelper.GetAzureInfo(AzCommands.AzLogout);
            if (!result.Success)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.Write(new Markup("[red]It was not possible to log out.[/]"));
                AnsiConsole.WriteLine();
                return false;
            }
            return true;
        }
     }
}
