using Microsoft.AspNetCore.Http;

namespace QuanlyPhongtapGymFitnessClub.DTOs
{
    public class FileUploadDbDto
    {
        public class UploadFileDto
        {
            public required IFormFile File { get; set; }
        }

        public class FileResponseDto
        {
            public int Id { get; set; }
            public string FileName { get; set; } = string.Empty;
            public long Size { get; set; }
            public DateTime UploadedAt { get; set; }
        }

        public class FileDownloadDto
        {
            public required byte[] Content { get; set; }
            public required string ContentType { get; set; }
            public required string FileName { get; set; }
        }
    }
}
