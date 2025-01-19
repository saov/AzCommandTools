namespace SAOV.CLI.AzTools.Helpers
{
    using System.Diagnostics;

    internal static class AzHelper
    {
        internal static (bool Success, string Output) GetAzureInfo(string command)
        {
            using Process process = new();
            process.StartInfo = GetProcessStartInfo(command);
            process.Start();
            process.WaitForExit(10000);
            string output = process.StandardOutput.ReadToEnd();
            if (process.ExitCode != 0)
            {
                output = process.StandardError.ReadToEnd();
            }
            return (process.ExitCode == 0, output);
        }

        private static ProcessStartInfo GetProcessStartInfo(string command)
        {
            return new()
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = GetShellOperatingSystem(),
                Arguments = GetShellCommand(command)
            };
        }

        private static string GetShellOperatingSystem()
        {
            return OperatingSystem.IsWindows() ?
                   Path.Combine(Environment.SystemDirectory, "cmd.exe") :
                   OperatingSystem.IsLinux() ?
                    "bash" :
                    throw new Exception("Unknown OperatingSystem.");
        }

        private static string GetShellCommand(string command)
        {
            return OperatingSystem.IsWindows() ?
                   @$"/c ""{command}""" :
                   OperatingSystem.IsLinux() ?
                    @$"-c ""{command.Replace("\"", "'")}""" :
                    throw new Exception("Unknown OperatingSystem.");
        }
    }
}
