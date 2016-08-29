using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class GalleryViewModel
    {
        public Guid ImageIdCurrent { get; set; }
        public Guid ImageIdPrev { get; set; }
        public Guid ImageIdNext { get; set; }

        public Guid PhotobookId { get; set; }

        //?
        public List<Guid> Images { get; set; }
    }
}