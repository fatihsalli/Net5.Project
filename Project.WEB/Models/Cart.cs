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

        public void DeleteItem(int id)
        {
            if (_myCart.ContainsKey(id))
            {
                _myCart.Remove(id);
                return;
            }
        }

        public void DecreaseItem(int id)
        {
            _myCart[id].Quantity--;
            if (_myCart[id].Quantity<=0)
            {
                DeleteItem(id);
                return;
            }
        }

        public void IncreaseItem(int id)
        {
            _myCart[id].Quantity++;
        }



    }
}
