using System.ComponentModel.DataAnnotations;

namespace QuanlyPhongtapGymFitnessClub.Attributes
{
    /// <summary>
    /// Custom Validation Attribute: Kiá»ƒm tra Ä‘á»™ tuá»•i náº±m trong khoáº£ng quy Ä‘á»‹nh (máº·c Ä‘á»‹nh 18 Ä‘áº¿n 65 tuá»•i cho HLV vĂ  NhĂ¢n viĂªn)
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
            ErrorMessage = $"Äá»™ tuá»•i pháº£i náº±m trong khoáº£ng tá»« {_minAge} Ä‘áº¿n {_maxAge} tuá»•i.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Cho phĂ©p null náº¿u khĂ´ng cĂ³ [Required]
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
                return new ValidationResult("GiĂ¡ trá»‹ ngĂ y sinh hoáº·c Ä‘á»™ tuá»•i khĂ´ng há»£p lá»‡.");
            }

            if (age < _minAge || age > _maxAge)
            {
                return new ValidationResult(ErrorMessage ?? $"Äá»™ tuá»•i pháº£i tá»« {_minAge} Ä‘áº¿n {_maxAge} tuá»•i.");
            }

            return ValidationResult.Success;
        }
    }
}
