namespace Project.WEB.Models
{
    public class CartItem
    {
        public CartItem()
        {
            Quantity = 1;
        }
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get { return Quantity * UnitPrice; } }

    }
}
