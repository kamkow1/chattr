using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace chattr.Shared.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }
        
        public DateTime SendDate { get; set; }

        #region foreign keys

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }

        [ForeignKey("Chat")]
        public int ChatId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        #endregion

        #region navigation properties

        [JsonIgnore]
        public virtual Message Parent { get; set; }

        [JsonIgnore]
        public virtual Chat Chat { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        #endregion
    }
}
