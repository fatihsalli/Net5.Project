using Project.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.Entity
{
    public class Shipper:BaseEntity
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public List<Order> Orders { get; set; }


    }
}
