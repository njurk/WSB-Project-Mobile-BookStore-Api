using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Model.Entities
{
    [Table("Cart")]
    public partial class Cart
    {
        [Key]
        [Column("CartID")]
        public int CartId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [Column("BookID")]
        public int BookId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("BookId")]
        [InverseProperty("Carts")]
        public virtual Book Book { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("Carts")]
        public virtual User User { get; set; } = null!;
    }
}
