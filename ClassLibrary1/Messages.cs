namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Messages
    {
        [Key]
        public Guid MessageId { get; set; }

        public Guid from { get; set; }

        public Guid to { get; set; }

        public DateTime creationTime { get; set; }

        public string text { get; set; }

        public Guid? TalkId { get; set; }

        public virtual Profiles Profiles { get; set; }

        public virtual Profiles Profiles1 { get; set; }
    }
}
