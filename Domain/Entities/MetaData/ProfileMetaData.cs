using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MetaData
{
    internal partial class ProfileMetaData
    {
        [Key]
        public Guid? ProfileId { get; set; }


        [StringLength(50)]
        public string fName { get; set; }


        [StringLength(50)]
        public string lName { get; set; }


        [StringLength(50)]
        public string mName { get; set; }
        

    }
}
