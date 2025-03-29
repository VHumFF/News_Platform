using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News_Platform.Services.Interfaces;

namespace News_Platform.Controllers
{
    public class FileController : Controller
    {
        private readonly IS3Service _s3Service;

        public FileController(IS3Service s3Service)
        {
            _s3Service = s3Service;
        }

        [Authorize]
        [HttpGet("presigned-url")]
        public IActionResult GetPresignedUrl(string extension = "jpg")
        {
            try
            {
                if (!long.TryParse(User.FindFirst("role")?.Value, out long role))
                {
                    return Unauthorized(new { error = "Invalid authentication token." });
                }

                if (role != 1)
                {
                    return Forbid("Only journalists can upload images.");
                }

                var allowedExtensions = new HashSet<string> { "jpg", "jpeg", "png", "gif" };
                extension = extension.ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest(new { error = "Invalid image type. Only JPG, JPEG, PNG, and GIF are allowed." });
                }

                string folder = "uploads/images";
                string fileName = $"{Guid.NewGuid()}_{DateTime.UtcNow:yyyyMMddHHmmss}.{extension}";
                string objectKey = $"{folder}/{fileName}";
                TimeSpan expiration = TimeSpan.FromMinutes(15);

                string presignedUrl = _s3Service.GeneratePresignedUrl(objectKey, expiration, $"image/{extension}");

                return Ok(new { url = presignedUrl, fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred while generating the presigned URL." });
            }
        }


    }
}
