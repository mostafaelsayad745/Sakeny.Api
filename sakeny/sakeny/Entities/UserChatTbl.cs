using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sakeny.Entities
{
    [Table("USER_CHAT_TBL")]
    public partial class UserChatTbl
    {
        [Key]
        [Column("USER_CHAT_ID", TypeName = "numeric(18, 0)")]
        public decimal UserChatId { get; set; }

        [Column("USER_CHAT_DATE", TypeName = "date")]
        public DateTime? UserChatDate { get; set; }

        [Column("USER_CHAT_TIME")]
        public TimeSpan? UserChatTime { get; set; }

        [Column("USER_CHAT_TYPE")]
        public string? UserChatType { get; set; } // what do you mean of the prop

        [Column("USER_CHAT_TEXT")]
        public string? UserChatText { get; set; }

        [Column("USER_CHAT_IMAGE", TypeName = "image")]
        public byte[]? UserChatImage { get; set; }

        // modification : here is the return vlaue must be the user table (UsersTbl)
        // modification : in this class must be a forign key to the (UsersTbl)

        [Column("USER_CHAT_FROM")]
        public string? UserChatFrom { get; set; }
        [Column("USER_CHAT_TO")]
        public string? UserChatTo { get; set; }
    }
}
