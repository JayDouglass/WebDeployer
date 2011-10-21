using WebDeployer.Deployer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebDeployer.Model;
using System.IO;

namespace WebDeployer.Test
{
    
    
    /// <summary>
    ///This is a test class for WebApplicationDeploymentServiceTest and is intended
    ///to contain all WebApplicationDeploymentServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WebApplicationDeploymentServiceTest
    {
        private const string ScriptBaseDirectory = @"C:\Users\Jay\Documents\Visual Studio 2010\Projects\WebDeployer\WebDeployer.Web\";
        private const string TestsBaseDirectory = @"C:\Users\Jay\Documents\Visual Studio 2010\Projects\WebDeployer\Tests\ccdeploy\Cosmo\";


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Backup
        ///</summary>
        [TestMethod()]
        public void BackupTest()
        {
            var scriptDirectory = @"DeploymentScripts";
            var fullPathToScripts = Path.Combine(ScriptBaseDirectory, scriptDirectory);

            WebApplicationDeploymentService target = new WebApplicationDeploymentService(fullPathToScripts, "deploy.ps1", "backup.ps1", 1000 * 60 * 120,
                                                                                         "Jay", "", "4uze8ata");

            Application application = new Application
            {
                Name = "Cosmo"
            };

            DeploymentTarget target1 = new DeploymentTarget
            {
                BackupDirectory = Path.Combine(TestsBaseDirectory + @"\QA", @"Backups"),
                TargetDirectory = @"C:\Users\Jay\Documents\Visual Studio 2010\Projects\WebDeployer\Tests\QAwebserver\Cosmo"
            };
            target.Backup(target1);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Deploy
        ///</summary>
        [TestMethod()]
        public void DeployTest()
        {
            var scriptDirectory = @"DeploymentScripts";
            var fullPathToScripts = Path.Combine(ScriptBaseDirectory, scriptDirectory);

            WebApplicationDeploymentService service = new WebApplicationDeploymentService(fullPathToScripts, "deploy.ps1", "backup.ps1", 1000 * 60 * 120,
                                                                                         "Jay", "", "4uze8ata");

            Application application = new Application
            {
                Name = "Cosmo",
                ExcludeFiles = "*exclude*.* Web.config",
                ExcludeDirectories = "Log PDFs"
            };

            DeploymentTarget target = new DeploymentTarget
            {
                BackupDirectory = Path.Combine(TestsBaseDirectory + @"\PROD", @"Backups"),
                TargetDirectory = @"C:\Users\Jay\Documents\Visual Studio 2010\Projects\WebDeployer\Tests\PRODwebserver\Cosmo",
                SourceDirectory = Path.Combine(TestsBaseDirectory + @"\PROD", @"Deploy"),
                Application = application,
                Name = "PROD"
            };

            DeploymentRequest request = new DeploymentRequest
            {
                DeploymentTarget = target
            };

            var result = service.Deploy(request);

            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
