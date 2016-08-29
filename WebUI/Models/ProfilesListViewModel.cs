using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class ProfilesListViewModel
    {
        public IEnumerable<Profile> Profiles { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public Profile Profile { get; set; }    // for Friends List
        public SearchProfileListViewModel sProfile { get; set; }
    }
}