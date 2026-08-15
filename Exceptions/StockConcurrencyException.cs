namespace ECommerceApi.Exceptions
{
    public class StockConcurrencyException : Exception
    {
        public StockConcurrencyException()
       : base("The product stock was changes by another request. Pleasy try again.")
        {
        }

    }
}
