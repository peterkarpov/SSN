namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Content")]
    public partial class Content
    {
        public Guid ContentId { get; set; }

        [StringLength(50)]
        public string type { get; set; }

        [StringLength(50)]
        public string annotation { get; set; }
    }
}
