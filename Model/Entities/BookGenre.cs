using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Model.Entities
{
    [Table("BookGenre")]
    public partial class BookGenre
    {
        [Key]
        [Column("BookGenreID")]
        public int BookGenreId { get; set; }
        [Column("BookID")]
        public int BookId { get; set; }
        [Column("GenreID")]
        public int GenreId { get; set; }

        [ForeignKey("BookId")]
        [InverseProperty("BookGenres")]
        public virtual Book Book { get; set; } = null!;
        [ForeignKey("GenreId")]
        [InverseProperty("BookGenres")]
        public virtual Genre Genre { get; set; } = null!;
    }
}
