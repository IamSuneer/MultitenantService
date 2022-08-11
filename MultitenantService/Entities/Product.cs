using MultitenantService.Contracts;

namespace MultitenantService.Entities
{
    public class Product : Base, ITenant
    {
        public Product(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
        protected Product()
        {

        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public string TenantId { get; set; }
    }
}
