using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebDeployer.Data;
using WebDeployer.Model;
using WebDeployer.Deployer;

namespace WebDeployer.Web
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private readonly WebDeployerContext context = new WebDeployerContext();
        private readonly WebApplicationDeploymentService service = new WebApplicationDeploymentService();

        protected void Page_Load(object sender, EventArgs e)
        {
            var pendingDeployments = context.Requests.Where(request => request.Processed == null);
            gridPendingDeployments.DataSource = pendingDeployments.ToArray();
            gridPendingDeployments.DataBind();
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            var request = context.Requests.Find(1);
            //service.Deploy(request);

        }


    }
}