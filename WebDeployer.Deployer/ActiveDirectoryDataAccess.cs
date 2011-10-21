using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;

namespace WebDeployer.Deployer
{
    public class ActiveDirectoryDataAccess : IDisposable
    {
        private readonly PrincipalContext context;

        public ActiveDirectoryDataAccess()
        {
            context = new PrincipalContext(ContextType.Domain); 
        }

        public string GetUserEmailAddressBySamAccountName(string samAccountName)
        {
            var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, samAccountName);

            return user != null ? user.EmailAddress : null;
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }
    }
}
