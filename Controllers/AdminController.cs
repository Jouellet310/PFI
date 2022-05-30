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
        MySpaceDBEntities DB = new MySpaceDBEntities();

        public ActionResult ManageArtistRequests()
        {
            return View(DB.List_UnacceptedArtist());
        }

        public ActionResult ListArtists ()
        {
            return View(DB.List_Artists());
        }

        public ActionResult AcceptArtist (ArtistRequest request)
        {
            if (ModelState.IsValid)
                DB.AcceptArtist(request);

            return RedirectToAction("ManageArtistRequests");
        }

        public ActionResult DeclineArtist(ArtistRequest request)
        {
            if (ModelState.IsValid)
                DB.DeclineArtist(request);

            return RedirectToAction("ManageArtistRequests");
        }
    }
}