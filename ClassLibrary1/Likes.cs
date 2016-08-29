namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Likes
    {
        [Key]
        public Guid LikeId { get; set; }

        public Guid TargetId { get; set; }

        public Guid ProfileId { get; set; }
    }
}
