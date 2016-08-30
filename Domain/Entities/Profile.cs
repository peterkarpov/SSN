using Domain.Entities.MetaData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [MetadataType(typeof(ProfileMetaData))]

    public class Profile
    {
        public Guid? ProfileId { get; set; }

        [Display(Name = "First Name")]
        public string fName { get; set; }

        [Display(Name = "Date of Birthday")]
        public DateTime? dob { get; set; }
        
        [Display(Name = "Gender")]
        public Gender? Gender { get; set; } // RadioButtonFor

        [Display(Name = "Last Name")]
        public string lName { get; set; }
        [Display(Name = "Middle Name")]
        public string mName { get; set; }

        [Display(Name = "Avatar Image")]
        public Guid? AvatarImageId { get; set; }

        [Display(Name = "Country")]
        [Range(typeof(int), "0", "1")]
        public Country? country { get; set; } // EnumDropDownListFor

        [Display(Name = "Language")]
        public Language[] language
        {
            get
            {
                return JsonConvert.DeserializeObject<Language[]>(languageForDB ?? "");
            }
            set
            {
                languageForDB = JsonConvert.SerializeObject(value);
            }
        } // ListBoxFor

        public string languageForDB { get; set; }

        [Display(Name = "Annotation")]
        public string Annotation { get; set; } // TextAreaFor

        [Display(Name = "Relationship")]
        [Range(typeof(int), "0", "2")]
        public RelationShipStatus? rStatus { get; set; } // DropDownListFor

        [Display(Name = "Show Email")]
        public bool showEmail { get; set; } // CheckBoxFor

        [ForeignKey("ProfileId")]
        internal virtual User User { get; set; }

        public virtual List<Friend> Friends { get; set; }

        //public virtual string login { get { return User.login; } }
        public virtual List<Image> Images { get; set; }
        public virtual List<Photobook> Photobooks { get; set; }
        public virtual List<News> News { get; set; }

        //public virtual List<Friend> Friends { get; set; }  //best? //subscriber? //member?

        //CONSTRAINT [Users_UserId] FOREIGN KEY ([ProfileId]) REFERENCES [dbo].[Users] ([UserId])
        internal virtual ICollection<Message> Messages { get; set; }
        internal virtual ICollection<Message> MessagesTo { get; set; }
        internal virtual ICollection<Talk> Talks1 { get; set; }
        internal virtual ICollection<Talk> Talks2 { get; set; }
    }

    public enum Language
    {
        russian,
        english,
        japanese,
        french,
        german,
    }

    public enum Country
    {
        Russia,
        England,
    }

    public enum Gender
    {
        none,
        male,
        female,
    }

    public enum RelationShipStatus
    {
        none,
        maried,
        friend,
    }

}
