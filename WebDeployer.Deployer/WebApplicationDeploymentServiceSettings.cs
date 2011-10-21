using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace WebDeployer.Deployer
{
    public class WebApplicationDeploymentServiceSettings
    {
        public static string ScriptUserName
        {
            get
            {
                return WebConfigurationManager.AppSettings["ScriptUserName"];
            }
        }

        public static string ScriptPassword
        {
            get
            {
                return WebConfigurationManager.AppSettings["ScriptPassword"];
            }
        }

        public static string ScriptDomain
        {
            get
            {
                return WebConfigurationManager.AppSettings["ScriptDomain"];
            }
        }

        public static string ScriptDirectory
        {
            get
            {
                return WebConfigurationManager.AppSettings["ScriptDirectory"];
            }
        }

        public static string DeploymentScriptName
        {
            get
            {
                return WebConfigurationManager.AppSettings["DeploymentScriptName"];
            }
        }

        public static string BackupScriptName
        {
            get
            {
                return WebConfigurationManager.AppSettings["BackupScriptName"];
            }
        }

        public static int ScriptTimeoutMilliseconds
        {
            get
            {
                return int.Parse(WebConfigurationManager.AppSettings["ScriptTimeoutMilliseconds"]);
            }
        }   
    }
}
