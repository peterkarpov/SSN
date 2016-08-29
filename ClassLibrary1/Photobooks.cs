namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Photobooks
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Photobooks()
        {
            Images = new HashSet<Images>();
        }

        [Key]
        public Guid PhotobookId { get; set; }

        public Guid? ProfileId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public int viewForFriend { get; set; }

        public int viewForAll { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Images> Images { get; set; }
    }
}
