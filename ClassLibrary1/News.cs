namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        public Guid NewsId { get; set; }

        public Guid? ProfileId { get; set; }

        public DateTime creationTime { get; set; }

        public string theme { get; set; }
    }
}
