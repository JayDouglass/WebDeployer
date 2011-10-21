using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using WebDeployer.Model;

namespace WebDeployer.Data
{
    public class WebDeployerDbInitializer : DropCreateDatabaseIfModelChanges<WebDeployerContext>
    {      
        protected override void Seed(WebDeployerContext context)
        {
            // Applications
            var applications = new List<Application>
            {
                new Application { Active = true, Name = "Cosmetology" },
                new Application { Active = true, Name = "APERS" }
            };
            applications.ForEach(a => context.Applications.Add(a));
            context.SaveChanges();

            // Deployment Targets
            var targets = new List<DeploymentTarget>
            {
                new DeploymentTarget { ApplicationId = applications[0].ApplicationId, Name = "QA", RequiresApproval = false, 
                                       SourceDirectory = "asdasdf", TargetDirectory = "fdgad", SendEmailNotifications = true}                                       
            };
            targets.ForEach(t => context.DeploymentTargets.Add(t));
            context.SaveChanges();

            // Deployment Requests
            var requests = new List<DeploymentRequest>
            {
                new DeploymentRequest { DeploymentTarget = targets[0], Approved = true, Requested = DateTime.Parse("7/26/2011") }
            };
            requests.ForEach(r => context.Requests.Add(r));
            context.SaveChanges();
        }
    }
}
