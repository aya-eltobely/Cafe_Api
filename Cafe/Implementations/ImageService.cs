using Cafe.DTOs;
using Cafe.Interfaces;
using Cafe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Implementations
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDBContext Context;

        public ImageService(ApplicationDBContext _Context)
        {
            Context = _Context;
        }

        //public int UploadImage(IFormFile? file)
        //{
        //    if (file == null || file.Length == 0)
        //        return 0;

        //    using (var memoryStream = new MemoryStream())
        //    {
        //        file.CopyTo(memoryStream);
        //        var image = new Image { FileName = file.FileName, ContentType= file.ContentType , ImageData = memoryStream.ToArray() };

        //        Context.Images.Add(image);
        //        Context.SaveChanges();

        //        return image.Id;
        //    }
        //}

        //public ImageDTO GetImage(int id)
        //{
        //    var image = Context.Images.FirstOrDefault(i => i.Id == id);

        //    if (image == null)
        //        return new ImageDTO();

        //    return new ImageDTO() { ImageData = image.ImageData , FileName = image.FileName , ContentType = image.ContentType };// "image/jpeg"); // Adjust content type based on your image type
        //}
    }
}
