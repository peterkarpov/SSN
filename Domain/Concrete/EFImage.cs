using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public partial class EFRepository : IRepository
    {
        public IEnumerable<Image> Images
        {
            get
            {
                return Context.Images;
            }
        }
        
        public bool SaveImage(Image image)
        {

            if (Context.Images.Find(image.ImageId) == null)
            {
                if (image.ImageId == default(Guid)) image.ImageId = Guid.NewGuid();
                if (image.DateOfLoad == default(DateTime)) image.DateOfLoad = DateTime.Now;
                Context.Images.Add(image);
            }
            else
            {
                var dbEntry = Context.Images.Find(image.ImageId);
                if (dbEntry != null)
                {
                    dbEntry.ImageId = image.ImageId;
                    dbEntry.PhotobookId = image.PhotobookId;
                    dbEntry.ImageData = image.ImageData;
                    dbEntry.ImageMimeType = image.ImageMimeType;
                    dbEntry.fileName = image.fileName;
                    dbEntry.DateOfLoad = image.DateOfLoad;
                    dbEntry.ProfileId = image.ProfileId;

                }
            }
            return Context.SaveChanges() > 0;
        }

        public bool DeleteImage(Guid ImageId)
        {
            var dbEntry = Context.Images.Find(ImageId);
            if (dbEntry != null)
            {
                Context.Images.Remove(dbEntry);
            }
            return Context.SaveChanges() > 0;
        }
    }
}
