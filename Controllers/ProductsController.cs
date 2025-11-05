using Microsoft.AspNetCore.Mvc;
using testblackduck.Models;
using testblackduck.Services;

namespace testblackduck.Controllers;

/// <summary>
/// Products management API controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>A list of all products</returns>
    /// <response code="200">Returns the list of products</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get a specific product by ID
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <returns>The requested product</returns>
    /// <response code="200">Returns the requested product</response>
    /// <response code="404">If the product is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound($"Product with ID {id} not found");

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with ID {ProductId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="product">The product to create</param>
    /// <returns>The created product</returns>
    /// <response code="201">Returns the newly created product</response>
    /// <response code="400">If the product data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        try
        {
            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        try
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, product);
            if (updatedProduct == null)
                return NotFound($"Product with ID {id} not found");

            return Ok(updatedProduct);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with ID {ProductId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
                return NotFound($"Product with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID {ProductId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get products that are in stock
    /// </summary>
    [HttpGet("in-stock")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsInStock()
    {
        try
        {
            var products = await _productService.GetProductsInStockAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products in stock");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get products within a price range
    /// </summary>
    [HttpGet("price-range")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByPriceRange(
        [FromQuery] decimal minPrice = 0,
        [FromQuery] decimal maxPrice = decimal.MaxValue)
    {
        try
        {
            var products = await _productService.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products by price range");
            return StatusCode(500, "Internal server error");
        }
    }
}