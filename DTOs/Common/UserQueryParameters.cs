namespace QuanlyPhongtapGymFitnessClub.DTOs.Common
{
    public class UserQueryParameters
    {
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        // Search (Từ khóa tìm chung: Username, Email, FullName...)
        public string? SearchTerm { get; set; }

        // Filter (Lọc theo trường cụ thể)
        public string? Role { get; set; }
        //public bool? IsActive { get; set; }

        // Sort
        public string? SortBy { get; set; } // Ví dụ: "username", "email", "createdat"
        public bool IsDescending { get; set; } = false;
    }
}

