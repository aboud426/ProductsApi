using Microsoft.AspNetCore.Mvc;
using ProductsApi.DTOs;
using ProductsApi.Services;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductService service,
        ILogger<ProductsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // ------------------------
    //       CREATE
    // ------------------------
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDTO dto)
    {
        var product = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // ------------------------
    //       GET BY ID
    // ------------------------
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _service.GetByIdAsync(id);
        return Ok(product);
    }

    // ------------------------
    //       UPDATE
    // ------------------------
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDTO dto)
    {
        var product = await _service.UpdateAsync(id, dto);
        return Ok(product);
    }

    // ------------------------
    //       DELETE
    // ------------------------
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    // ------------------------
    //       LIST
    // ------------------------
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? keyword, 
        [FromQuery] string? category, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] bool includeDeleted = false)
    {
        var result = await _service.ListAsync(keyword, category, page, pageSize, includeDeleted);
        return Ok(result);
    }
}
