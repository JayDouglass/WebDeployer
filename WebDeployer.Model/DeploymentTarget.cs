using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebDeployer.Model
{
    public class DeploymentTarget
    {
        public int DeploymentTargetId { get; set; }
        [MaxLength(64)] 
        public string Name { get; set; }
        [MaxLength(128)]
        public string URL { get; set; } 
        public bool RequiresApproval { get; set; }
        public bool SendEmailNotifications { get; set; }

        [MaxLength(128)]
        public string SourceDirectory { get; set; }
        [MaxLength(128)]
        public string TargetDirectory { get; set; }
        [MaxLength(128)]
        public string BackupDirectory { get; set; }
       
        public int ApplicationId { get; set; }
        public virtual Application Application { get; set; }
    }
}
