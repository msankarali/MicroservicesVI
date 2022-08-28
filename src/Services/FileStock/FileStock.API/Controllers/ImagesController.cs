using Common;
using FileStock.API.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Results;

namespace FileStock.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImagesController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> SaveImageAsync(IFormFile formFile, CancellationToken cancellationToken)
        {
            var fileExtension = Path.GetExtension(formFile.FileName);

            if (
                string.IsNullOrEmpty(fileExtension) ||
                fileExtension.ToLower() != ".jpg" ||
                fileExtension.ToLower() != ".png" ||
                fileExtension.ToLower() != ".bmp")
            {
                return CreateActionResultInstance(ApiResponse<NoContent>.Fail(400, "Not a valid file"));
            }

            if (formFile != null && formFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", formFile.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await formFile.CopyToAsync(stream, cancellationToken);

                return CreateActionResultInstance(
                    ApiResponse<ImageDto>.Success(
                        data: new ImageDto
                        {
                            Url = $"images/{formFile.FileName}"
                        },
                        statusCode: 200)
                    );
            }

            return CreateActionResultInstance(ApiResponse<ImageDto>.Fail(400, "Not image"));
        }

        [HttpDelete]
        public IActionResult DeleteImage(string fileUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileUrl);
            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(ApiResponse<NoContent>.Fail(404, "Image not found"));
            }

            System.IO.File.Delete(path);

            return CreateActionResultInstance(ApiResponse<NoContent>.Success(204));
        }
    }
}