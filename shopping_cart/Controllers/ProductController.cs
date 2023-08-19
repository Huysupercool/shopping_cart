using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using shopping_cart.Data;
using shopping_cart.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace shopping_cart.Controllers
{
	public class ProductController : Controller
	{
		private readonly ILogger<ProductController> _logger;
		private readonly AppDbContext _dbContext;
		public ProductController(ILogger<ProductController> logger, AppDbContext context)
		{
			_logger = logger;
			this._dbContext = context;
		}
		//public IActionResult Products()
		//{
		//	List<Product> proList = _dbContext.Products.ToList();
		//	return View(proList);
		//}
		public IActionResult ProductPage()
		{
			ViewBag.ListCat = _dbContext.Categories.ToList();
			List<Product> proList = _dbContext.Products.ToList();
			return View(proList);
		}
		public IActionResult ProductCategory(int Id)
		{
			ViewBag.ListCat = _dbContext.Categories.ToList();
			List<Product> proList = _dbContext.Products.Where(x => x.Category.Id == Id).ToList();
			return View(proList);
		}

		public IActionResult HomepageProduct()
		{
			List<Product> proList = _dbContext.Products.ToList();
			return View(proList);
		}
		public async Task<IActionResult> Detail(int Id)
		{
			ViewBag.ListCat = _dbContext.Categories.ToList();
			if (Id == null)
			{
				return NotFound();
			}
			var product = await _dbContext.Products.SingleOrDefaultAsync(x => x.Id == Id);
			return View(product);
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
