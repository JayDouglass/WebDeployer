using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebDeployer.Model
{
    public class Application
    {
        public int ApplicationId { get; set; }
        [MaxLength(64)] 
        public string Name { get; set; }
        public bool Active { get; set; }
        [MaxLength(256)] 
        public string ExcludedFiles { get; set; }
        [MaxLength(256)]
        public string ExcludedDirectories { get; set; }

        public virtual ICollection<DeploymentTarget> DeploymentTargets { get; set; }
        public virtual ICollection<LogEntry> LogEntries { get; set; }
    }
}
