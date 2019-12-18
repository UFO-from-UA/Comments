using Comments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Comments
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //protected void Application_BeginRequest()
        //{
        //    string IPAddress = IPHelper.GetIPAddress(Request.ServerVariables["HTTP_VIA"],
        //                                                     Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
        //                                                     Request.ServerVariables["REMOTE_ADDR"]);

        //}
    }
}
