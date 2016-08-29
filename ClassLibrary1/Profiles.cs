namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Profiles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Profiles()
        {
            Messages = new HashSet<Messages>();
            Messages1 = new HashSet<Messages>();
            Talks = new HashSet<Talks>();
            Talks1 = new HashSet<Talks>();
        }

        [Key]
        public Guid ProfileId { get; set; }

        [StringLength(50)]
        public string fName { get; set; }

        public DateTime? dob { get; set; }

        public int? Gender { get; set; }

        public byte[] ImageData { get; set; }

        [StringLength(50)]
        public string ImageMimeType { get; set; }

        [StringLength(50)]
        public string lName { get; set; }

        [StringLength(50)]
        public string mName { get; set; }

        public Guid? AvatarImageId { get; set; }

        [StringLength(50)]
        public string country { get; set; }

        [StringLength(50)]
        public string language { get; set; }

        public string Annotation { get; set; }

        public int? rStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Messages> Messages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Messages> Messages1 { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Talks> Talks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Talks> Talks1 { get; set; }
    }
}
