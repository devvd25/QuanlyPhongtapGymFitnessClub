using Microsoft.AspNetCore.Mvc;
using QuanlyPhongtapGymFitnessClub.Services;
using static QuanlyPhongtapGymFitnessClub.DTOs.FileUploadDbDto;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Quan ly File (Upload va Download truc tiep tu Database SQL Server)
    /// Su dung mo hinh 3 lop: Controller -> Service -> Repository
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        // POST api/files/upload
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<ActionResult<FileResponseDto>> Upload([FromForm] UploadFileDto dto)
        {
            try
            {
                var result = await _fileService.UploadFileAsync(dto.File);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET api/files/download/{id}
        [HttpGet("download/{id:int}")]
        public async Task<IActionResult> Download(int id)
        {
            var fileDto = await _fileService.GetFileForDownloadAsync(id);
            if (fileDto == null)
            {
                return NotFound(new { message = $"Khong tim thay file co Id = {id}" });
            }

            return File(fileDto.Content, fileDto.ContentType, fileDto.FileName);
        }
    }
}
