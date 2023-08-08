using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using shopping_cart.Data;

namespace shopping_cart.Components
{
	public class CategoryViewComponent : ViewComponent
	{
		private readonly AppDbContext _dbContext;
		public CategoryViewComponent(AppDbContext context)
		{
			this._dbContext = context;
		}
		
		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<Category> cateList = await _dbContext.Categories.ToListAsync();
			return View(cateList);
		}
	}
}
