namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comments
    {
        [Key]
        public Guid CommentId { get; set; }

        public Guid ProfileId { get; set; }

        public DateTime creationTime { get; set; }

        [Required]
        public string text { get; set; }

        public Guid TargetId { get; set; }
    }
}
