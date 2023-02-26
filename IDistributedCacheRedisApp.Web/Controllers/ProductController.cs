using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

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


            _distributedCache.SetString("deneme", "enes",options);
            await _distributedCache.SetStringAsync("deneme2", "tunahan", options);

            return View();
        }

        public async Task<IActionResult> Show()
        {
            var name  = _distributedCache.GetString("deneme");
            var asyncName = await _distributedCache.GetStringAsync("deneme2");
            
            ViewBag.Name = name;
            ViewBag.AsyncName = asyncName;
            return View();
        }

        public async Task<IActionResult> Remove()
        {
            _distributedCache.Remove("deneme");
            await _distributedCache.RemoveAsync("deneme2");
            return View();
        }
    }
}
