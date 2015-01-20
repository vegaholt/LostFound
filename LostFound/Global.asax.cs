﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LostFound
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
      ModelBinders.Binders.DefaultBinder = new PerpetuumSoft.Knockout.KnockoutModelBinder();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
