using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreApi.Model.Entities
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

        [Column("Street", TypeName = "nvarchar(255)")]
        public string Street { get; set; } = null!;

        [Column("City", TypeName = "nvarchar(255)")]
        public string City { get; set; } = null!;

        [Column("PostalCode", TypeName = "nvarchar(20)")]
        public string PostalCode { get; set; } = null!;

        [Column("OrderStatusID")]
        public int OrderStatusId { get; set; }

        [ForeignKey("OrderStatusId")]
        [InverseProperty("Orders")]
        public virtual OrderStatus OrderStatus { get; set; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("Orders")]
        public virtual User User { get; set; } = null!;

        public int DeliveryTypeId { get; set; }

        [ForeignKey("DeliveryTypeId")]
        [InverseProperty("Orders")]
        public virtual DeliveryType DeliveryType { get; set; } = null!;

        [InverseProperty("Order")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
