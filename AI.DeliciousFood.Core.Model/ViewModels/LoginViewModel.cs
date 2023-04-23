using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace AI.DeliciousFood.Core.Model.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        /// <summary>
        ///  扩展登录（AuthenticationScheme的命名空间是Microsoft.AspNetCore.Authentication;）
        /// </summary>
        public IList<AuthenticationScheme> ExtermalLogins { get; set; }
    }
}
