using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrueVote.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
    }
}
