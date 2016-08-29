namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Friends
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FriendId { get; set; }

        public Guid ProfileId { get; set; }

        public Guid subscriberId { get; set; }

        public virtual User User { get; set; }
    }
}
