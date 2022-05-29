using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MySpace.Models
{
    public partial class ArtistRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public bool Approved { get; set; }
    }
}