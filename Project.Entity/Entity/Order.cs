using Project.Entity.Abstract;
using Project.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.Entity
{
    public class Order:BaseEntity
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
            ShipperStatus = ShipStatus.NotShipped;
        }
        public int OrderNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ShipStatus ShipperStatus { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public int? ShipperId { get; set; }
        public int UserId { get; set; }
        public Shipper Shipper { get; set; }
        public AppUser User { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
