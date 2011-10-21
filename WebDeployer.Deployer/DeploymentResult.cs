using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDeployer.Deployer
{
    public class DeploymentResult
    {
        public string Log { get; set; }
        public bool Success { get; set; }
        public string StandardOutput { get; set; }
        public string StandardError { get; set; }
        public TimeSpan Elapsed { get; set; }
        public int ExitCode { get; set; }
    }
}
