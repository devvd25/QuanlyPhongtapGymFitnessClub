namespace QuanlyPhongtapGymFitnessClub.Models
{
    public class StoredFile
    {
        public int Id { get; set; }
        public required string FileName { get; set; }      // Tên file gốc (vd: avatar.png)
        public required string ContentType { get; set; }   // Kiểu MIME (vd: image/png, application/pdf)
        public required byte[] Content { get; set; }       // Dữ liệu nhị phân của file
        public long Length { get; set; }                   // Kích thước (bytes)
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
