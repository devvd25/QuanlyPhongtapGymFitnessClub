using Microsoft.AspNetCore.Http;
using static QuanlyPhongtapGymFitnessClub.DTOs.FileUploadDbDto;

namespace QuanlyPhongtapGymFitnessClub.Services
{
    public interface IFileService
    {
        Task<FileResponseDto> UploadFileAsync(IFormFile file);
        Task<FileDownloadDto?> GetFileForDownloadAsync(int id);
    }
}

