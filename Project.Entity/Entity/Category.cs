using Project.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity.Entity
{
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public List<Product> Products { get; set; }
    }
}
