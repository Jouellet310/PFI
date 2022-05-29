using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySpace.Models;


namespace MySpace.Controllers
{
    [UserAccess]
    public class MyspaceController : Controller
    {
        MySpaceDBEntities DB = new MySpaceDBEntities();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DB.Dispose();
            }
            base.Dispose(disposing);
        }
        public void RenewArtistsSerialNumber()
        {
            HttpRuntime.Cache["ArtistsSerialNumber"] = Guid.NewGuid().ToString();
            OnlineUsers.RenewSerialNumber();
        }
        public string GetArtistsSerialNumber()
        {
            if (HttpRuntime.Cache["ArtistsSerialNumber"] == null)
            {
                RenewArtistsSerialNumber();
            }
            return (string)HttpRuntime.Cache["ArtistsSerialNumber"];
        }
        public void SetLocalArtistsSerialNumber()
        {
            Session["ArtistsSerialNumber"] = GetArtistsSerialNumber();
        }
        public bool IsArtistsUpToDate()
        {
            return ((string)Session["ArtistsSerialNumber"] == (string)HttpRuntime.Cache["ArtistsSerialNumber"]);
        }
        // GET: Myspace
        public ActionResult Index()
        {
            SetLocalArtistsSerialNumber();
            return View();
        }

        public ActionResult GetArtists(bool forceRefresh = false)
        {
            if (forceRefresh || !IsArtistsUpToDate())
            {
                SetLocalArtistsSerialNumber();
                return PartialView("GetArtists", DB.Artists.Where(a => a.Approved && !a.User.Blocked).OrderBy(a => a.Name).ToList());
            }
            return null;
        }


        public ActionResult ArtistDetails(int artistId)
        {
            var artist = DB.Artists.Find(artistId);
            if (artist != null)
            {
                SetLocalArtistsSerialNumber();
                DB.AddArtistVisit(artistId);
                return View("ArtistDetails", artist);
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetArtistDetails( int artistId, bool forceRefresh = false)
        {
            if (forceRefresh || !IsArtistsUpToDate())
            {
                SetLocalArtistsSerialNumber();
                return PartialView("GetArtistDetails", DB.Artists.Find(artistId));
            }
            return null;
        }

        public ActionResult AddLike(int artistId)
        {
            SetLocalArtistsSerialNumber();
            DB.AddLike(artistId);
            return View();
        }

        public ActionResult RemoveLike(int artistId)
        {
            SetLocalArtistsSerialNumber();
            DB.RemoveLike(artistId);
            return View();
        }

        public ActionResult AddMessage(int artistId,string text)
        {
            SetLocalArtistsSerialNumber();
            DB.AddMessage(artistId, text);
            return View();
        }

        public ActionResult RemoveMessage(int messageId)
        {
            SetLocalArtistsSerialNumber();
            DB.RemoveMessage(messageId);
            return View();
        }
    }
}