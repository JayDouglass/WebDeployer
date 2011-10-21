using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebDeployer.Model
{
    public class DeploymentRequest : Request
    {
        public int DeploymentRequestId { get; set; }
    }
}
