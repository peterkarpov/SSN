namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Images
    {
        [Key]
        public Guid ImageId { get; set; }

        public Guid? PhotobookId { get; set; }

        public byte[] ImageData { get; set; }

        [StringLength(50)]
        public string ImageMimeType { get; set; }

        [StringLength(50)]
        public string fileName { get; set; }

        public DateTime? DateOfLoad { get; set; }

        public virtual Photobooks Photobooks { get; set; }
    }
}
