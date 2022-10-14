using Project.Entity.Entity;
using System.Collections.Generic;

namespace Project.WEB.Models
{
    public static class StockCheck
    {

        public static Dictionary<int, int> stockCheck = new();

        public static bool GetStockCheck(int stock,int productId)
        {
            if (stockCheck.ContainsKey(productId))
            {
                int newStock = stockCheck[productId];

                if (newStock-1<0)
                {
                    return false;
                }
                else
                {
                    stockCheck[productId] = newStock-1;
                    return true;
                }
            }
            else
            {
                stockCheck.Add(productId, stock-1);
                return true;
            }
        }



    }
}
