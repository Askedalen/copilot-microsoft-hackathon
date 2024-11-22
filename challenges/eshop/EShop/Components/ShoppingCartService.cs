namespace EShop.Components
{
    public class ShoppingCartService
    {
        private readonly List<AutoPart> _cartItems = new();

        public event Action OnChange;

        public void AddToCart(AutoPart part)
        {
            _cartItems.Add(part);
            NotifyStateChanged();
        }

        public List<AutoPart> GetCartItems()
        {
            return _cartItems;
        }

        public decimal GetTotalPrice()
        {
            return _cartItems.Sum(item => item.Price);
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}