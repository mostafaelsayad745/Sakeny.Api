using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sakeny.Entities
{
    [Table("FEATURES_TBL")]
    [Index("FeaturesName", Name = "FEATURES_INDEX", IsUnique = true)]
    public partial class FeaturesTbl
    {
        [Key]
        [Column("FEATURES_ID", TypeName = "numeric(18, 0)")]
        public decimal FeaturesId { get; set; }
        [Column("FEATURES_NAME")]
        [StringLength(300)]
        public string? FeaturesName { get; set; }
    }
}
