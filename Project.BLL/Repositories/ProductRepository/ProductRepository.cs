using Project.DAL.Context;
using Project.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Repositories.ProductRepository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ProjectContext context) : base(context)
        {

        }
    }
}
