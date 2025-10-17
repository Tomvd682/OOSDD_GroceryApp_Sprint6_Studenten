using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAll() => _productRepository.GetAll();

        public Product? Get(int id) => _productRepository.Get(id);

        public Product Add(Product item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            if (string.IsNullOrWhiteSpace(item.Name)) throw new ArgumentException("Name is verplicht.", nameof(item.Name));
            if (item.Price < 0) throw new ArgumentException("Price kan niet negatief zijn.", nameof(item.Price));
            if (item.Stock < 0) throw new ArgumentException("Stock kan niet negatief zijn.", nameof(item.Stock));

            // Doorzetten naar repo
            return _productRepository.Add(item);
        }

        public Product? Update(Product item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            return _productRepository.Update(item);
        }

        public Product? Delete(Product item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            return _productRepository.Delete(item);
        }
    }
}
