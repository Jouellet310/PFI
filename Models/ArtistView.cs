using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace MySpace.Models
{
    [MetadataType(typeof(ArtistView))]
    public partial class Artist 
    {
        [NotMapped]
        public static ImageGUIDReference ImageReference =
            new ImageGUIDReference(@"/ImagesData/ArtistImage/", @"default.jpg", false);

        public void SaveImage(string imageData)
        {
            string newGUID = ImageReference.SaveImage(imageData, MainPhotoGUID);
            MainPhotoGUID = newGUID;
        }

        public string GetImageUrl()
            => ImageReference.MakeUrl(MainPhotoGUID);
    }


    public class ArtistView
    {

    }
}