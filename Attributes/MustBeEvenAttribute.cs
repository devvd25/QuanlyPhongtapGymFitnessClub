using System.ComponentModel.DataAnnotations;

namespace QuanlyPhongtapGymFitnessClub.Attributes
{
    /// <summary>
    /// Custom Validation Attribute: Kiá»ƒm tra má»™t property sá»‘ nguyĂªn pháº£i lĂ  sá»‘ cháºµn (BĂ i thá»±c hĂ nh Buá»•i 4 - Slide 18)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MustBeEvenAttribute : ValidationAttribute
    {
        public MustBeEvenAttribute()
        {
            ErrorMessage = "GiĂ¡ trá»‹ pháº£i lĂ  má»™t sá»‘ nguyĂªn cháºµn.";
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

            return new ValidationResult("GiĂ¡ trá»‹ pháº£i lĂ  kiá»ƒu sá»‘ nguyĂªn.");
        }
    }
}
