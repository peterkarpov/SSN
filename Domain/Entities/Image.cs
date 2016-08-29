using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Image
    {
        [Key]
        public Guid ImageId { get; set; }
        public Guid PhotobookId { get; set; }
        public Guid ProfileId { get; set; }

        [Required(ErrorMessage = "ImageData is null")]
        public byte[] ImageData { get; set; }

        [Required(ErrorMessage = "ImageMimeType is null")]
        [StringLength(50)]
        public string ImageMimeType { get; set; }

        [Display(Name = "File Name")]
        [StringLength(50)]
        public string fileName { get; set; }
        public DateTime DateOfLoad { get; set; }
        
        public virtual Profile Profile { get; set; }
        public virtual Photobook Photobook { get; set; }

    }
}
