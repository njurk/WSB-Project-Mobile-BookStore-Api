using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Models
{
    [Table("Book")]
    [Index("Isbn", Name = "UQ__Book__447D36EA866A09CF", IsUnique = true)]
    public partial class Book
    {
        public Book()
        {
            OrderItems = new HashSet<OrderItem>();
            Reviews = new HashSet<Review>();
            Genres = new HashSet<Genre>();
            Users = new HashSet<User>();
        }

        [Key]
        [Column("BookID")]
        public int BookId { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string Title { get; set; } = null!;
        [Column("AuthorID")]
        public int AuthorId { get; set; }
        [Column("ISBN")]
        [StringLength(20)]
        [Unicode(false)]
        public string Isbn { get; set; } = null!;
        public string Description { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Language { get; set; } = null!;
        public int Quantity { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string ImageUrl { get; set; } = null!;
        public int YearPublished { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        public int NumberOfPages { get; set; }

        [ForeignKey("AuthorId")]
        [InverseProperty("Books")]
        public virtual Author Author { get; set; } = null!;
        [InverseProperty("Book")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        [InverseProperty("Book")]
        public virtual ICollection<Review> Reviews { get; set; }

        [ForeignKey("BookId")]
        [InverseProperty("Books")]
        public virtual ICollection<Genre> Genres { get; set; }
        [ForeignKey("BookId")]
        [InverseProperty("Books")]
        public virtual ICollection<User> Users { get; set; }
    }
}
