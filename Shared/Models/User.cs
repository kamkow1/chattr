using System.ComponentModel.DataAnnotations;

namespace chattr.Shared.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }
    }
}