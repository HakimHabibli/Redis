using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisProject.Models;

namespace RedisProject.Controllers;

public class ProductController : Controller
{
    private readonly IDistributedCache _cache;
    private const string CacheKey = "products";

    public ProductController(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<IActionResult> Index()
    {
        var cached = await _cache.GetStringAsync(CacheKey);
        List<Product> products;

        if (!string.IsNullOrEmpty(cached))
        {
            products = JsonSerializer.Deserialize<List<Product>>(cached);
        }
        else
        {
            products = new List<Product>
        {
            new Product { Id = 1, Name = "Phone" },
            new Product { Id = 2, Name = "Laptop" }
        };

            var jsonData = JsonSerializer.Serialize(products);
            await _cache.SetStringAsync(CacheKey, jsonData);
        }

        return View(products);
    }
}
