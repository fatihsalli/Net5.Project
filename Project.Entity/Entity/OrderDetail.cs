using Project.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.Entity
{
    public class OrderDetail:BaseEntity
    {
        public decimal UnitPrice { get; set; }
        public int Count { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }


    }
}
