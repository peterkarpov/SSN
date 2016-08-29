using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class PhotobookViewModel
    {
        public List<Photobook> Photobooks { get; set; }
        public Profile Profile { get; set; }
        public List<Image> Images { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}