using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class AddImageViewModel
    {
        public Image Image { get; set; }
        public List<SelectListItem> SelectListPhotobook { get; set; }
    }
}