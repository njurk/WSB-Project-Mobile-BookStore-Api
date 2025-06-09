using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Model.Entities
{
    public class DeliveryType
    {
        [Key]
        public int DeliveryTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Fee { get; set; }

        [InverseProperty("DeliveryType")]
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }

}
