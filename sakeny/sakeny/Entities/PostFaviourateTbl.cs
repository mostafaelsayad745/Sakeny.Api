﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace sakeny.Entities
{
    [Table("POST_FAV_TBL")]
    public class PostFaviourateTbl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("POST_FAV_ID", TypeName = "numeric(18, 0)")]
        public decimal PostFavId { get; set; }

        [Column("POST_ID", TypeName = "numeric(18, 0)")]
        public decimal PostId { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty("POST_FAV_TBL")]
        public virtual PostsTbl? Post { get; set; }
    }
}
