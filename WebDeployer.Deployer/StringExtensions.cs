using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDeployer.Deployer
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string into a SecureString
        /// </summary>
        public static System.Security.SecureString ToSecureString(this String source)
        {
            var secureString = new System.Security.SecureString();
            foreach (Char c in source)
                secureString.AppendChar(c);

            return secureString;
        }
    }
}
