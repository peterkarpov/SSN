using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class MessagesViewModel
    {
        public Profile Profile { get; set; }

        public List<Talk> Talks { get; set; }
    }
}