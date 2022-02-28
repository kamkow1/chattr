using System.Collections.Generic;
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

        #region navigation properites

        public virtual ICollection<Message> Messages { get; set; }

        #endregion
    }
}