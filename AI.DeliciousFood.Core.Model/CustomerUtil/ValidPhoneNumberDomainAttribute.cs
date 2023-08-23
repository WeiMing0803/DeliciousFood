using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AI.DeliciousFood.Core.Model.CustomerUtil
{
    public class ValidPhoneNumberDomainAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string pattern = @"^1[3-9]\d{9}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(value.ToString());
        }
    }
}
