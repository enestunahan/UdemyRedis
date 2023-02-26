using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache= distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            Product product = new Product { Id = 1, Name = "Kalem", Price = 20 };

            string jsonProduct  = JsonConvert.SerializeObject(product);

            await _distributedCache.SetStringAsync("product:1", jsonProduct, options);

            return View();
        }

        public async Task<IActionResult> Show()
        {
            string jsonProduct = await _distributedCache.GetStringAsync("product:1");

            Product product  =  JsonConvert.DeserializeObject<Product>(jsonProduct);

            ViewBag.Product = product;

            return View();
        }


        public async Task<IActionResult> Remove()
        {
            await _distributedCache.RemoveAsync("product:1");
            return View();
        }
    }
}
