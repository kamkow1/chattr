using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace chattr.Shared.Models
{
    public class User
    {
        public User()
        {
            this.GroupChats = new HashSet<GroupChat>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<GroupChat> GroupChats { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

        // self referecing relationship - user has many friends, which are also users
        public virtual User Self { get; set; }

        public virtual ICollection<User> Friends { get; set; }
    }
}