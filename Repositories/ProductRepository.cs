using testblackduck.Models;

namespace testblackduck.Repositories;

public class ProductRepository : IProductRepository
{
    // In-memory storage for demo purposes
    private readonly List<Product> _products = new();
    private int _nextId = 1;

    public ProductRepository()
    {
        // Seed with some sample data
        _products.AddRange(new[]
        {
            new Product { Id = _nextId++, Name = "Laptop", Description = "Gaming laptop", Price = 1299.99m, Stock = 10 },
            new Product { Id = _nextId++, Name = "Mouse", Description = "Wireless gaming mouse", Price = 79.99m, Stock = 25 },
            new Product { Id = _nextId++, Name = "Keyboard", Description = "Mechanical keyboard", Price = 149.99m, Stock = 15 }
        });
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult(_products.AsEnumerable());
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<Product> CreateAsync(Product product)
    {
        product.Id = _nextId++;
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;
        _products.Add(product);
        return Task.FromResult(product);
    }

    public Task<Product?> UpdateAsync(int id, Product product)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Id == id);
        if (existingProduct == null)
            return Task.FromResult<Product?>(null);

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        return Task.FromResult<Product?>(existingProduct);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return Task.FromResult(false);

        _products.Remove(product);
        return Task.FromResult(true);
    }

    public Task<bool> ExistsAsync(int id)
    {
        var exists = _products.Any(p => p.Id == id);
        return Task.FromResult(exists);
    }
}