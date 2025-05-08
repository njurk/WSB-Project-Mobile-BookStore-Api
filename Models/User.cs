using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Models
{
    [Table("User")]
    [Index("Username", Name = "UQ__User__536C85E4551E1A01", IsUnique = true)]
    [Index("Email", Name = "UQ__User__A9D10534202E2F01", IsUnique = true)]
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
            Books = new HashSet<Book>();
        }

        [Key]
        [Column("UserID")]
        public int UserId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Username { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        public string Email { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string? Street { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string? City { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string? PostalCode { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Order> Orders { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Review> Reviews { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Users")]
        public virtual ICollection<Book> Books { get; set; }
    }
}
