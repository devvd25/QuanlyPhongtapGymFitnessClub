using System.ComponentModel.DataAnnotations;

namespace Buoi2_WebAPI.Attributes
{
    /// <summary>
    /// Custom Validation Attribute: Kiểm tra một property số nguyên phải là số chẵn (Bài thực hành Buổi 4 - Slide 18)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MustBeEvenAttribute : ValidationAttribute
    {
        public MustBeEvenAttribute()
        {
            ErrorMessage = "Giá trị phải là một số nguyên chẵn.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (int.TryParse(value.ToString(), out int intValue))
            {
                if (intValue % 2 != 0)
                {
                    return new ValidationResult(ErrorMessage);
                }
                return ValidationResult.Success;
            }

            return new ValidationResult("Giá trị phải là kiểu số nguyên.");
        }
    }
}
