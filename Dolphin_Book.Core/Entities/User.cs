using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Dolphin_Book.Core.Entities
{
    public class User: IdentityUser
    {
        public double Budget { get; set; }
        public string Fullname { get; set; }
    }
}
