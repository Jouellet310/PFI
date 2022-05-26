using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySpace.Models;

namespace MySpace.Controllers
{
    
    [AdminAccess]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult ManageArtistRequests()
        {
            return View();
        }
    }
}