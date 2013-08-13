using BootstrapMvcSample.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace TheFlow.Site.Controllers
{
    public class DevelopController : BootstrapBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
