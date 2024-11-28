namespace SAOV.CommandTools.AzTools.Commands
{
    using SAOV.CommandTools.AzTools.Helpers;

    internal static class ClearScreen
    {
        internal static void GetCommand()
        {
            AzHelper.GetAzureInfo(OperatingSystem.IsWindows() ?
                                    AzCommands.ClearScreenWindows :
                                    OperatingSystem.IsLinux() ?
                                        AzCommands.ClearScreenLinux :
                                        throw new Exception("Unknown OperatingSystem."));
        }
    }
}
