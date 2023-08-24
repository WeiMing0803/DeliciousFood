using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace AI.DeliciousFood.Core.Model.ViewModels
{
    public class LogOnViewModel
    {
        [Required(ErrorMessage ="请输入邮箱地址")]
        [EmailAddress]
        [Display(Name = "邮箱地址")]
        public string Email { get; set; }

        [Required(ErrorMessage ="请输入用户名")]        
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="请输入密码")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Required(ErrorMessage = "请输入确认密码")]
        [Compare("Password", ErrorMessage = "密码与确认密码不一致，请重新输入！")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }

        [Required(ErrorMessage = "请输入手机号")]
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///  扩展登录（AuthenticationScheme的命名空间是Microsoft.AspNetCore.Authentication;）
        /// </summary>
        //public IList<AuthenticationScheme> ExtermalLogins { get; set; }
    }
}
