using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace WebDeployer.Data
{
    public class WebDeployerContextSettings
    {
        public static string ConnectionStringName
        {
            get
            {
                return "WebDeployer";
            }
        }
    }
}
