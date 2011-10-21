using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using WebDeployer.Model;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebDeployer.Data
{
    public class WebDeployerContext : DbContext
    {
        public DbSet<Application> Applications { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<DeploymentTarget> DeploymentTargets { get; set; }
        public DbSet<LogEntry> Log { get; set; }

        public WebDeployerContext()
            : base(WebDeployerContextSettings.ConnectionStringName)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>().Map(cfg => cfg.ToTable("Log"));

            //modelBuilder.Entity<RollbackRequest>().HasRequired<DeploymentTarget>(r => r.Target).WithRequiredDependent().WillCascadeOnDelete(false);
            //modelBuilder.Entity<DeploymentRequest>().HasRequired<DeploymentTarget>(r => r.Target).WithRequiredDependent().WillCascadeOnDelete(false);
        }
    }
}

//modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 


/*
public DbSet<Individual> Individuals { get; set; }
public DbSet<School> Schools { get; set; }
public DbSet<Address> Addresses { get; set; }
*/

/*
modelBuilder.Entity<Address>().Map(cfg => cfg.ToTable("Addresses"))
                              .Map(cfg => 
                                  { 
                                      cfg.Properties(a => a.AddressId); 
                                      cfg.ToTable("IndividualAddresses"); 
                                  })
                              .Map(cfg =>
                                  {
                                      cfg.Properties(a => a.AddressId);
                                      cfg.ToTable("SchoolAddresses");
                                  });
  
modelBuilder.Entity<Individual>().HasMany<Address>(i => i.Addresses)
                                 .WithOptional()
                                 .Map(cfg =>
                                 {
                                    cfg.MapKey("IndividualId");
                                    cfg.ToTable("IndividualAddresses");
                                 });


modelBuilder.Entity<School>().HasMany<Address>(i => i.Addresses)
                             .WithOptional()
                             .Map(cfg => 
                                 {
                                    cfg.MapKey("SchoolId");
                                    cfg.ToTable("SchoolAddresses");
                                 });
*/
