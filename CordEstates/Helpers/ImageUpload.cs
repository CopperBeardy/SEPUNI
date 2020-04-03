using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Helpers
{
    public  class ImageUpload
    {
   

        public  class ImageUploadWrapper : IImageUploadWrapper
        {


            public  string Upload(IFormFile file, IHostEnvironment hostEnvironment)
            {
                var dir = hostEnvironment.ContentRootPath;
                using (var filestream = new FileStream(Path.Combine(dir, "wwwroot", "images", file.FileName), FileMode.Create, FileAccess.Write))
                {

                    file.CopyTo(filestream);
                }
                return $"images/{file.FileName}";
            }

        }

        public interface IImageUploadWrapper
        {
            string Upload(IFormFile file, IHostEnvironment hostEnvironment);
        }

    }
}
