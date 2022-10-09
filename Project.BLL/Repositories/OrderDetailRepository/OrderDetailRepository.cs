using Project.DAL.Context;
using Project.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Repositories.OrderDetailRepository
{
    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ProjectContext context) : base(context)
        {

        }
    }
}
