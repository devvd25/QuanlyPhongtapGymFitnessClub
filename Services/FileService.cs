using Microsoft.AspNetCore.Http;
using QuanlyPhongtapGymFitnessClub.Models;
using QuanlyPhongtapGymFitnessClub.Repositories;
using static QuanlyPhongtapGymFitnessClub.DTOs.FileUploadDbDto;

namespace QuanlyPhongtapGymFitnessClub.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        // Gioi han dung luong toi da 10 MB (10 * 1024 * 1024 bytes)
        private const long MaxFileSizeBytes = 10 * 1024 * 1024;

        // Danh sach duoi file hop le: anh (.jpg, .jpeg, .png, .gif, .webp), tai lieu .pdf va .docx
        private static readonly string[] _allowedExtensions =
        {
            ".jpg", ".jpeg", ".png", ".gif", ".webp",
            ".pdf",
            ".docx"
        };

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<FileResponseDto> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File tai len khong hop le hoac khong co du lieu.");
            }

            // 1. Kiem tra gioi han dung luong toi da 10MB
            if (file.Length > MaxFileSizeBytes)
            {
                var fileSizeMb = Math.Round((double)file.Length / (1024 * 1024), 2);
                throw new ArgumentException($"Dung luong file ({fileSizeMb}MB) vuot qua gioi han cho phep (toi da 10MB).");
            }

            // 2. Kiem tra dinh dang duoi file: chi cho phep anh, pdf, docx
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
            {
                throw new ArgumentException("Dinh dang file khong duoc ho tro. He thong chi cho phep tai len file anh (.jpg, .jpeg, .png, .gif, .webp), tai lieu .pdf va .docx.");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var storedFile = new StoredFile
            {
                FileName = Path.GetFileName(file.FileName),
                ContentType = file.ContentType,
                Content = memoryStream.ToArray(),
                Length = file.Length
            };

            var createdFile = await _fileRepository.AddAsync(storedFile);

            return new FileResponseDto
            {
                Id = createdFile.Id,
                FileName = createdFile.FileName,
                Size = createdFile.Length,
                UploadedAt = createdFile.UploadedAt
            };
        }

        public async Task<FileDownloadDto?> GetFileForDownloadAsync(int id)
        {
            var file = await _fileRepository.GetByIdAsync(id);
            if (file == null) return null;

            return new FileDownloadDto
            {
                Content = file.Content,
                ContentType = file.ContentType,
                FileName = file.FileName
            };
        }
    }
}
