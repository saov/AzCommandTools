namespace SAOV.CLI.AzTools.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Text;

    internal static class AzHelper
    {
        internal static (bool Success, string Output) GetAzureInfo(string command)
        {
            if (AzCommand.AZSDebug)
            {
                string fileName = $"azs_Commands-{DateTime.Now:yyyyMMdd}.txt";
                File.AppendAllText(fileName, $"{command}\n");
            }
            using Process process = new();
            process.StartInfo = GetProcessStartInfo(command);
            process.Start();
            process.WaitForExit(30000);
            StringBuilder sb = new();
            string output = string.Empty;
            while ((output = process.StandardOutput.ReadLine()) != null)
            {
                sb.AppendLine(output);
            }
            if (process.ExitCode != 0)
            {
                sb = new();
                sb.Append(process.StandardError.ReadToEnd());
            }
            return (process.ExitCode == 0, sb.ToString());
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
                            @$"-c ""{command.Replace("\"", "\\\"")}""" :
                            throw new Exception("Unknown OperatingSystem.");
        }
    }
}
