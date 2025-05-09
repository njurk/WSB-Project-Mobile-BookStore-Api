using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Model.Entities
{
    [Table("Collection")]
    public partial class Collection
    {
        [Key]
        [Column("CollectionID")]
        public int CollectionId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [Column("BookID")]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        [InverseProperty("Collections")]
        public virtual Book Book { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("Collections")]
        public virtual User User { get; set; } = null!;
    }
}
