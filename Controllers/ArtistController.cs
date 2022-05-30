using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySpace.Models;
using System.Data.Entity;

namespace MySpace.Controllers
{
    public class ArtistController : Controller
    {
        MySpaceDBEntities DB = new MySpaceDBEntities();

        [ArtistAccess]
        public ActionResult EditProfile ()
        {
            Artist artist = DB.Artists.Where(a => a.UserId == OnlineUsers.CurrentUserId).First();
            return View(artist);
        }

        [ArtistAccess, HttpPost]
        public ActionResult EditProfile(Artist artiste, string AvatarImageData)
        {
            Artist currentArtist = DB.Artists.Find(artiste.Id);
            DB.Entry(currentArtist).State = EntityState.Modified;

            currentArtist.Description = artiste.Description;
            currentArtist.Name = artiste.Name;

            if (AvatarImageData != null && AvatarImageData != "")
                currentArtist.SaveImage(AvatarImageData);
            
            DB.SaveChanges();
            return RedirectToAction("Index", "MySpace");
        }
    }
}