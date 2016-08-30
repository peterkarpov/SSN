namespace ESN3.WebUI.Controllers
{
    using Domain.Entities;
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Domain;
    using System.Configuration;

    public class ImageController : Controller
    {
        private IRepository repository;

        public ImageController(IRepository repo)
        {
            repository = repo;
        }

        public FileContentResult GetImage(Guid? ImageId)
        {

            if (ImageId == null) return GetDefault();
            
            Image image = repository.Images
                .FirstOrDefault(i => i.ImageId == ImageId);

            if (image != null)
            {
                return File(image.ImageData, image.ImageMimeType);
            }
            else
            {
                return GetDefault();
            }
        }

        internal FileContentResult GetDefault()
        {
            FileStream fs = default(FileStream);
            BinaryReader br = default(BinaryReader);
            byte[] img = default(byte[]);
            string defaultFilePath = ConfigurationManager.AppSettings["rootPath"];
            string filePath = defaultFilePath + @"\App_Data\camera_200.png";

            try
            {
                fs = System.IO.File.OpenRead(filePath);

                br = new BinaryReader(fs);

                img = br.ReadBytes((int)fs.Length);
            }
            catch
            {
                return null;
            }
            finally
            {
                br?.Close();
                fs?.Close();
            }
            return File(img, "image/jpeg");
        }

        

    }
}