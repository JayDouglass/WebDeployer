using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDeployer.Model
{
    public class LogEntry
    {
        public int LogEntryId { get; set; }
        public DateTime Created { get; set; }
        public string Activity { get; set; }
        public String User { get; set; }

        public int ApplicationId { get; set; }
        public virtual Application Application { get; set; }
    }
}
