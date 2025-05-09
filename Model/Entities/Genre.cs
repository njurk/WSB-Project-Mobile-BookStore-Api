using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Model.Entities
{
    [Table("Genre")]
    [Index("Name", Name = "UQ__Genre__737584F6892E8E73", IsUnique = true)]
    public partial class Genre
    {
        public Genre()
        {
            BookGenres = new HashSet<BookGenre>();
        }

        [Key]
        [Column("GenreID")]
        public int GenreId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        [InverseProperty("Genre")]
        public virtual ICollection<BookGenre> BookGenres { get; set; }
    }
}
