using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace AI.DeliciousFood.Core.Model.ViewModels
{
    public class MenuViewModel
    {
        public string RecipeName { get; set; }

        public string FinishedPicture { get; set; }
    }
}
