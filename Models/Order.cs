using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Models
{
    [Table("Order")]
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        [Key]
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }
        [Column("OrderStatusID")]
        public int OrderStatusId { get; set; }

        [ForeignKey("OrderStatusId")]
        [InverseProperty("Orders")]
        public virtual OrderStatus OrderStatus { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("Orders")]
        public virtual User User { get; set; } = null!;
        [InverseProperty("Order")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
