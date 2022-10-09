using Project.DAL.Context;
using Project.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Repositories.OrderRepository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ProjectContext context) : base(context)
        {

        }
    }
}
