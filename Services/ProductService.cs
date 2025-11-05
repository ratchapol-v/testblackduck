using testblackduck.Models;
using testblackduck.Repositories;

namespace testblackduck.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        // Add business logic here (validation, etc.)
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required");

        if (product.Price < 0)
            throw new ArgumentException("Product price cannot be negative");

        return await _productRepository.CreateAsync(product);
    }

    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
        // Add business logic here
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required");

        if (product.Price < 0)
            throw new ArgumentException("Product price cannot be negative");

        var exists = await _productRepository.ExistsAsync(id);
        if (!exists)
            return null;

        return await _productRepository.UpdateAsync(id, product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Product>> GetProductsInStockAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Where(p => p.Stock > 0);
    }

    public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var products = await _productRepository.GetAllAsync();
        return products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
    }
}