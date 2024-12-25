using Microsoft.AspNetCore.Mvc;
using eczASP.Net.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace chtotonaASP.Controllers
{
    public class CatalogController : Controller
    {
        private readonly MusicStoreContext _context;

        public CatalogController(MusicStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Catalog(string search, string sort, string filter, decimal? minPrice, decimal? maxPrice, bool? inStock)
        {
            var query = _context.Products.AsQueryable();

            // Фильтрация по категории
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(p => p.Category.CategoryName == filter);
            }

            // Фильтрация по диапазону цен
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Фильтрация по наличию
            if (inStock.HasValue && inStock.Value)
            {
                query = query.Where(p => p.StockQuantity > 0);
            }

            // Поиск по названию или описанию
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.ProductName.Contains(search) || p.DescriptionProduct.Contains(search));
            }

            // Сортировка
            switch (sort)
            {
                case "price_asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case "name_asc":
                    query = query.OrderBy(p => p.ProductName);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(p => p.ProductName);
                    break;
                default:
                    query = query.OrderBy(p => p.IdProduct);
                    break;
            }

            var products = await query.ToListAsync();
            var viewModel = new ProductViewModel
            {
                Products = products,
                Search = search,
                Sort = sort,
                Filter = filter,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                InStock = inStock
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            return View(products);
        }
    }
}
