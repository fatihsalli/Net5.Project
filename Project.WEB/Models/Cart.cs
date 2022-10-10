using System.Collections.Generic;
using System.Linq;

namespace Project.WEB.Models
{
    public class Cart
    {
        public Dictionary<int, CartItem> _myCart = new();

        public List<CartItem> Mycart 
        { 
            get 
            {
                return _myCart.Values.ToList();   
            } 
        }

        public void AddItem(CartItem item)
        {
            if (_myCart.ContainsKey(item.Id))
            {
                _myCart[item.Id].Quantity++;
                return;
            }
            else
            {
                _myCart.Add(item.Id, item);
            }
        }



    }
}
