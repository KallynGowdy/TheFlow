﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using TheFlow.Site;

[assembly: PreApplicationStartMethod(typeof(MvcApplication), "EnableForms")]

namespace TheFlow.Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void EnableForms()
        {
            FormsAuthentication.EnableFormsAuthentication(null);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BootstrapSupport.BootstrapBundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);
            
            //TheFlow.API.WebApiConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}