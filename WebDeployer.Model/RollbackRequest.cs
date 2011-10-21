using System;
using System.ComponentModel.DataAnnotations;

namespace WebDeployer.Model
{
    public class RollbackRequest : Request
    {
        public int RollbackRequestId { get; set; }
        public string SourceDirectory { get; set; }
    }
}
