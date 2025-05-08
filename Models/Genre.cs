using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Models
{
    [Table("Genre")]
    [Index("Name", Name = "UQ__Genre__737584F6892E8E73", IsUnique = true)]
    public partial class Genre
    {
        public Genre()
        {
            Books = new HashSet<Book>();
        }

        [Key]
        [Column("GenreID")]
        public int GenreId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        [ForeignKey("GenreId")]
        [InverseProperty("Genres")]
        public virtual ICollection<Book> Books { get; set; }
    }
}
