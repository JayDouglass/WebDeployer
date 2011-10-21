using System.ComponentModel.DataAnnotations;
using System;

namespace WebDeployer.Model
{
    public abstract class Request
    {
        public int RequestId { get; set; }
        [MaxLength(128)]
        public string Reason { get; set; }
        [MaxLength(64)]
        public string RequestedBy { get; set; }
        [MaxLength(64)]
        public string ApprovedBy { get; set; }
        public DateTime Requested { get; set; }
        public DateTime? When { get; set; }
        public bool Approved { get; set; }
        public DateTime? Processed { get; set; }

        public int DeploymentTargetId { get; set; }
        public virtual DeploymentTarget DeploymentTarget { get; set; }
    }
}
