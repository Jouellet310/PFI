using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySpace.Models;
using System.Data.Entity;

namespace MySpace.Controllers
{
    [ArtistAccess]
    public class ArtistController : Controller
    {
        MySpaceDBEntities DB = new MySpaceDBEntities();
        
        public ActionResult EditProfile ()
        {
            Artist artist = DB.Artists.Where(a => a.UserId == OnlineUsers.CurrentUserId).First();
            return View(artist);
        }

        [HttpPost]
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

        [PropagateModelStateFromTempData]
        public ActionResult Videos ()
        {
            return View(DB.Videos);
        }

        [RestoreModelStateFromTempData]
        public ActionResult AddVideo()
        {
            return View();
        }

        [HttpPost, SetTempDataModelState]
        public ActionResult AddVideo (Video video)
        {
            if (video.YoutubeLink == null || video.YoutubeLink == "")
                ModelState.AddModelError("YoutubeLink", "Le lien youtube est obligatoire!");
            if (video.Title == null || video.Title == "")
                ModelState.AddModelError("Title", "Le titre est obligatoire!");

            // parse link
            const string youtubeVideoRequest = "https://www.youtube.com/watch?v=";
            string ytId = "";
            if (video.YoutubeLink.Contains(youtubeVideoRequest))
            {
                ytId = video.YoutubeLink.Replace(youtubeVideoRequest, "");
                if (ytId.IndexOf("&") > -1)
                    ytId = ytId.Substring(0, ytId.IndexOf("&"));
            }
            else
                ModelState.AddModelError("YoutubeLink", "Le lien que vous avez entrer n'est pas valide");

            if (ytId != "")
            {
                video.ArtistId = DB.Artists.Where(a => a.UserId == OnlineUsers.CurrentUserId).First().Id;
                video.Creation = DateTime.Now;
                video.YoutubeLink = ytId;
            }
            else
                ModelState.AddModelError("YoutubeLink", "L'url n'a pas ete parser correctement");

            if (!ModelState.IsValid)
                return RedirectToAction("Videos");

            DB.Videos.Add(video);
            DB.SaveChanges();

            return RedirectToAction("Videos");
        }

        public ActionResult RemoveVideo(int id)
        {
            Video video = DB.Videos.Find(id);
            DB.Videos.Remove(video);
            DB.SaveChanges();
            return RedirectToAction("Videos");
        }

        public ActionResult SendMail ()
        {
            

            return View();
        }

        [HttpPost]
        public ActionResult SendMail(string Message)
        {
            Artist artiste = DB.Artists.Where(a => a.UserId == OnlineUsers.CurrentUserId).First();
            var followers = DB.List_Followers(DB.Artists.Where(a => a.UserId == OnlineUsers.CurrentUserId).First().Id)
                .ToList();

            foreach (User user in followers)
            {
                Gmail.SMTP.SendEmail(user.GetFullName(), user.Email, "Message de votre artiste preferer - " + artiste.Name, Message);
            }

            return RedirectToAction("Index", "MySpace");
        }
    }
}