using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.DeliciousFood.Core.Model
{
    public class FoodUser : IdentityUser<long>
    {
        public DateOnly MembershipExpireAt { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
