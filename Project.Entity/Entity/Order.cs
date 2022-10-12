using Project.Entity.Abstract;
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
        }
        public string OrderNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public AppUser User { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
