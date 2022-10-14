using Project.BLL.Repositories.OrderRepository;
using Project.Entity.Abstract;
using Project.Entity.Entity;
using System;
using System.Linq;

namespace Project.WEB.Models
{
    public class CompleteCart
    {
        public Order AddOrder(AppUser user, Cart cart, int number)
        {
            Order order = new();
            order.User = user;
            order.OrderNumber = number;

            decimal totalPrice = 0;
            foreach (CartItem item in cart.Mycart)
            {
                totalPrice += item.SubTotal;
            }

            order.TotalPrice = totalPrice;
            return order;
        }

        public int RandomNumber()
        {
            Random rnd = new Random();
            int number = rnd.Next(1, 10000);
            return number; 
        }

        public bool RandomNumberCheck(IQueryable<Order> list,int number)
        {
            if (list.Where(x=> x.OrderNumber==number).Count()>0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
