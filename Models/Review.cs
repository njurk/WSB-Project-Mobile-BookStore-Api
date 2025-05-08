using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Models
{
    [Table("Review")]
    public partial class Review
    {
        [Key]
        [Column("ReviewID")]
        public int ReviewId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [Column("BookID")]
        public int BookId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }

        [ForeignKey("BookId")]
        [InverseProperty("Reviews")]
        public virtual Book Book { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("Reviews")]
        public virtual User User { get; set; } = null!;
    }
}
