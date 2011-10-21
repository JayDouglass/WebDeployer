using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebDeployer.Model;
using System.Net;
using System.Diagnostics;
using System.IO;
using Tools.Network;

namespace WebDeployer.Deployer
{
    public class WebApplicationDeploymentService
    {
        private readonly PowerShellScriptRunner scriptRunner;

        private readonly string scriptWorkingDirectory;
        private readonly string deploymentScriptName;
        private readonly string backupScriptName;
        private readonly int timeoutMilliseconds;

        private readonly string userName;
        private readonly string password;
        private readonly string domain;

        public WebApplicationDeploymentService(string scriptDirectory, string deploymentScriptName, string backupScriptName,
                                               int timeoutMilliseconds, string userName, string domain, string password)
        {
            this.scriptRunner = new PowerShellScriptRunner();

            this.scriptWorkingDirectory = scriptDirectory;
            this.deploymentScriptName = deploymentScriptName;
            this.backupScriptName = backupScriptName;
            this.timeoutMilliseconds = timeoutMilliseconds;

            this.userName = userName;
            this.domain = domain;
            this.password = password;          
        }

        public WebApplicationDeploymentService()
            : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WebApplicationDeploymentServiceSettings.ScriptDirectory),
                   WebApplicationDeploymentServiceSettings.DeploymentScriptName,
                   WebApplicationDeploymentServiceSettings.BackupScriptName,
                   WebApplicationDeploymentServiceSettings.ScriptTimeoutMilliseconds,
                   WebApplicationDeploymentServiceSettings.ScriptUserName,
                   WebApplicationDeploymentServiceSettings.ScriptDomain,
                   WebApplicationDeploymentServiceSettings.ScriptPassword)
        { }

        public bool TestUsernamePassword()
        {
            // test domain/username/password valid
            Impersonator impersonator = null;
            try
            {
                impersonator = new Impersonator(this.userName, this.domain, this.password);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (impersonator != null)
                {
                    impersonator.Dispose();
                    impersonator = null;
                }
            }

            return true;
        }

        class TestForAccessResult
        {
            public TestForAccessResult()
            {
                this.CanAccessAllRequiredDirectories = true;
            }

            public bool CanAccessAllRequiredDirectories { get; set; }
            public string[] DeniedDirectories { get; set; }
        }

        private TestForAccessResult TestDirectoryAccess(DeploymentRequest request)
        {
            var target = request.DeploymentTarget;

            var requiredDirectories = new []
            {
                target.SourceDirectory,
                target.TargetDirectory,
                target.BackupDirectory
            };

            var deniedDirectories = new List<String>();

            var canAccessAllRequiredDirectories = true;

            using (new Impersonator(this.userName, this.domain, this.password))
            {
                foreach (var directory in requiredDirectories)
                {
                    var isAccessGranted = DirectoryAccessChecker.CheckDirectoryAccess(directory);
                    if (!isAccessGranted) 
                    { 
                        canAccessAllRequiredDirectories = false;
                        deniedDirectories.Add(directory);
                    }
                }
            }

            return new TestForAccessResult
            {
                CanAccessAllRequiredDirectories = canAccessAllRequiredDirectories,
                DeniedDirectories = deniedDirectories.ToArray()
            };
        }

        public DeploymentResult Deploy(DeploymentRequest request)
        {
            var log = new StringBuilder();
            var errors = new StringBuilder();
            var output = new StringBuilder();
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            log.AppendFormat("WebDeployer log --- {0} {1}\n\n", DateTime.Now.ToLongDateString(), DateTime.Now.ToShortTimeString());
            log.AppendFormat("Deploying {0} to {1}\n\n", request.DeploymentTarget.Application.Name, request.DeploymentTarget.Name);

            var target = request.DeploymentTarget;

            // test username & password
            var isValidScriptExecutionAccount = TestUsernamePassword();
            if (!isValidScriptExecutionAccount)
            {
                log.AppendFormat("ERROR: Failed to impersonate script account: '{0}'\n", this.userName);
                return new DeploymentResult
                {
                    Success = false,
                    Log = log.ToString(),
                };
            }
            log.AppendLine("SUCCESS: Impersonated script account");

            // test directory access
            var testDirectoryAccessResult = TestDirectoryAccess(request);
            if (!testDirectoryAccessResult.CanAccessAllRequiredDirectories)
            {
                log.AppendFormat("ERROR: Can not access all required directories for this deployment request: {0}\n",
                                 String.Join(", ", testDirectoryAccessResult.DeniedDirectories));
                return new DeploymentResult
                {
                    Success = false,
                    Log = log.ToString(),
                };
            }
            log.AppendLine("SUCCESS: Can access all required directories");

            // backup target directory to backups directory
            log.AppendFormat("Backing up deployment target directory: {0} to {1}\n", target.TargetDirectory, target.BackupDirectory);
            var backupScriptResult = Backup(target);
            output.Append(backupScriptResult.StandardOutput);
            errors.Append(backupScriptResult.StandardError);
            if (errors.Length > 0 || backupScriptResult.ExecutedWithinTimeout == false || backupScriptResult.ExitCode >= 8)
            {
                log.AppendLine("ERROR: Could not back up target directory (robocopy exit code >= 8 or timeout exceeeded)");
                return new DeploymentResult
                {
                    Success = false,
                    Log = log.ToString(),
                    StandardError = errors.ToString(),
                    StandardOutput = output.ToString()
                };
            }

            // deploy application
            log.AppendFormat("Copying web application to IIS: {0} to {1}\n", target.SourceDirectory, target.TargetDirectory);
            var scriptCommand = String.Format(@".\{0} -source '{1}' -target '{2}'",
                                this.deploymentScriptName, target.SourceDirectory, target.TargetDirectory);
            if (!String.IsNullOrEmpty(target.Application.ExcludeFiles))
            {
                scriptCommand += String.Format(" -excludefiles '{0}'", target.Application.ExcludeFiles);
            }
            if (!String.IsNullOrEmpty(target.Application.ExcludeDirectories))
            {
                scriptCommand += String.Format(" -excludedirectories '{0}'", target.Application.ExcludeDirectories);
            }
            log.AppendFormat("Script command: {0}\n", scriptCommand);
            var deploymentScriptResult = this.scriptRunner.RunScript(this.userName, this.password, this.domain,
                                                                     scriptCommand, this.scriptWorkingDirectory, this.timeoutMilliseconds);
            output.Append(deploymentScriptResult.StandardOutput);
            errors.Append(deploymentScriptResult.StandardError);
            if (errors.Length > 0 || backupScriptResult.ExecutedWithinTimeout == false || backupScriptResult.ExitCode >= 8)
            {
                log.AppendLine("ERROR: Could not deploy web application to IIS (robocopy exit code >= 8 or timeout exceeeded)");
                return new DeploymentResult
                {
                    Success = false,
                    Log = log.ToString(),
                    StandardError = errors.ToString(),
                    StandardOutput = output.ToString()
                };
            }

            stopwatch.Stop();

            log.AppendFormat("SUCCESS: Successfully deployed application.  Elapsed seconds: {0}", stopwatch.Elapsed.TotalSeconds);

            var result = new DeploymentResult
            {
                Success = true,
                Log = log.ToString(),
                StandardOutput = output.ToString(),
                StandardError = errors.ToString(),
                Elapsed = stopwatch.Elapsed,
                ExitCode = deploymentScriptResult.ExitCode,
            };
           
            File.WriteAllText(Path.Combine(target.TargetDirectory, @"deployment-output.log"), result.StandardOutput);

            return result;            
        }

        public void Rollback(DeploymentTarget target)
        {

        }

        /// <summary>
        /// Copy all files from target directory to 
        /// a new directory in the deployment target's 
        /// backup directory.
        /// Name new backup directory yyyy-mm-dd-hh-mm
        /// </summary>
        public PowerShellScriptResult Backup(DeploymentTarget target)
        {
            // make sortable directory name, replace reserved : with -
            var newDirectoryName = DateTime.Now.ToString("s").Replace(":", "-");
            var fullPathToBackupDirectory = Path.Combine(target.BackupDirectory, newDirectoryName);

            using (new Impersonator(this.userName, this.domain, this.password))
            {
                Directory.CreateDirectory(fullPathToBackupDirectory);
            }

            var scriptCommand = String.Format(@".\{0} -source '{1}' -target '{2}'",
                                this.backupScriptName, target.TargetDirectory, fullPathToBackupDirectory);
            var scriptResult = this.scriptRunner.RunScript(this.userName, this.password, this.domain,
                                    scriptCommand, this.scriptWorkingDirectory, this.timeoutMilliseconds);

            return scriptResult;
        }

    }
}
