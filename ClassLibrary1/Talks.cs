namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Talks
    {
        [Key]
        public Guid TalkId { get; set; }

        public Guid Profile1Id { get; set; }

        public Guid Profile2Id { get; set; }

        public virtual Profiles Profiles { get; set; }

        public virtual Profiles Profiles1 { get; set; }
    }
}
