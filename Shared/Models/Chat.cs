using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace chattr.Shared.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        
        public string Topic { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        #region navigation properties

        public virtual ICollection<User> Members { get; set; }

        #endregion
    }
}
