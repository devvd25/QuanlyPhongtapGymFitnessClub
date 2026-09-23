using System.ComponentModel.DataAnnotations;

namespace Buoi2_WebAPI.Attributes
{
    /// <summary>
    /// Custom Validation Attribute: Kiểm tra độ tuổi nằm trong khoảng quy định (mặc định 18 đến 65 tuổi cho HLV và Nhân viên)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GymAgeValidationAttribute : ValidationAttribute
    {
        private readonly int _minAge;
        private readonly int _maxAge;

        public GymAgeValidationAttribute(int minAge = 18, int maxAge = 65)
        {
            _minAge = minAge;
            _maxAge = maxAge;
            ErrorMessage = $"Độ tuổi phải nằm trong khoảng từ {_minAge} đến {_maxAge} tuổi.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Cho phép null nếu không có [Required]
            }

            int age;

            if (value is DateTime birthDate)
            {
                var today = DateTime.Today;
                age = today.Year - birthDate.Year;
                if (birthDate.Date > today.AddYears(-age)) age--;
            }
            else if (int.TryParse(value.ToString(), out int parsedAge))
            {
                age = parsedAge;
            }
            else
            {
                return new ValidationResult("Giá trị ngày sinh hoặc độ tuổi không hợp lệ.");
            }

            if (age < _minAge || age > _maxAge)
            {
                return new ValidationResult(ErrorMessage ?? $"Độ tuổi phải từ {_minAge} đến {_maxAge} tuổi.");
            }

            return ValidationResult.Success;
        }
    }
}
