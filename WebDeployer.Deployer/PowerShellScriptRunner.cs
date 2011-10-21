using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace WebDeployer.Deployer
{
    // On returning exitcode from powershell:
    // http://powershell.com/cs/blogs/tips/archive/2009/05/18/returning-exit-code-from-script.aspx
    // http://thepowershellguy.com/blogs/posh/archive/2008/05/20/hey-powershell-guy-how-can-i-run-a-powershell-script-from-cmd-exe-and-return-an-errorlevel.aspx
    public class PowerShellScriptRunner
    { 
        public PowerShellScriptResult RunScript(string userName, string password, string domain,
                                                string scriptCommand, string scriptDirectory, int timeoutMilliseconds)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                UserName = userName,
                Domain = domain,
                Password = password.ToSecureString(),
                Arguments = scriptCommand + "; exit $LASTEXITCODE",
                WorkingDirectory = scriptDirectory,
                UseShellExecute = false,
                ErrorDialog = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var errors = new StringBuilder();
            var output = new StringBuilder();

            var process = Process.Start(processStartInfo);

            process.ErrorDataReceived += (sender, errorLine) => { if (errorLine.Data != null) errors.AppendLine(errorLine.Data); };
            process.OutputDataReceived += (sender, outputLine) => { if (outputLine.Data != null) output.AppendLine(outputLine.Data); };

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            var executedWithinTimeout = process.WaitForExit(timeoutMilliseconds);
            var elapsed = process.ExitTime - process.StartTime;

            var result = new PowerShellScriptResult
            {
                StandardOutput = output.ToString(),
                StandardError = errors.ToString(),
                Elapsed = elapsed,
                ExitCode = process.HasExited ? process.ExitCode : -1,
                ExecutedWithinTimeout = executedWithinTimeout
            };

            return result;
        }
    }
}
