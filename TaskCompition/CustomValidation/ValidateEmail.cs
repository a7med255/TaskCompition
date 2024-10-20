using System.ComponentModel.DataAnnotations;

namespace TaskCompition.CustomValidation
{
    public class ValidateEmail : ValidationAttribute
    {
        readonly string allowedDomain;
        public ValidateEmail(string domain)
        {
            allowedDomain=domain;
        }

        public override bool IsValid(object? value)
        {
            string[] strings = value.ToString().Split('@');
            return strings[1].ToUpper() == allowedDomain.ToUpper();
        }
    }
}
