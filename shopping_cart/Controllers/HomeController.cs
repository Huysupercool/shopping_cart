using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopping_cart.Data;
using shopping_cart.Models;
using System.Diagnostics;

namespace shopping_cart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext  _dbContext;
		public HomeController(ILogger<HomeController> logger, AppDbContext context) 
        { 
            _logger = logger;
            _dbContext = context;
        }

        public IActionResult Index()
        {
			//List<Category> cateList = _dbContext.Categories.ToList();
			ViewBag.ListCat = _dbContext.Categories.ToList();
			List<Product> proList = _dbContext.Products.ToList();
            return View(proList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

		public IActionResult Blog()
		{
			return View();
		}

		public IActionResult BlogSingle()
		{
			return View();
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}