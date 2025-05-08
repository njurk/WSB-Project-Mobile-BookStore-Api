using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Models
{
    [Table("OrderStatus")]
    [Index("Name", Name = "UQ__OrderSta__737584F67B77D4DE", IsUnique = true)]
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        [Column("OrderStatusID")]
        public int OrderStatusId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        [InverseProperty("OrderStatus")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
