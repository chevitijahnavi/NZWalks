using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;
//using static System.Net.Mime.MediaTypeNames;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepositoey;
        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepositoey = imageRepository;
        }

        //POST:/api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);
            if (ModelState.IsValid)
            {
                //Convert DTO to Domain model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription
                };
                

                //User repository to upload image

                await imageRepositoey.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }

            return BadRequest(ModelState);

        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                //ModelState.AddModelError() is used to add a validation error to the current request.
                ModelState.AddModelError("file", "Unsuppoerted file Extension");
            }
            if(request.File.Length> 10485760)
            {
                ModelState.AddModelError("file", "File size is more that 10MB, please upload a smaller size file");
            }
        }
    }
}