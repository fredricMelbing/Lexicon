
using LogisticsSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class ProductsController : Controller
{
    private readonly LogisticsSystemContext _context;

    public ProductsController(LogisticsSystemContext context)
    {
        _context = context;
    }

	// GET: PRODUCTS
	[HttpGet]
	public async Task<IActionResult> Index()    
    {
        var products = await _context.Product.ToListAsync();		
		return View(products);
    }

	// GET: PRODUCTS/Details/5
	[HttpGet]
	public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Product
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

	// GET: Products/Inventory
	[HttpGet]
	public async Task<IActionResult> Inventory()
	{		
		var products = await _context.Product.ToListAsync();
        		
		IEnumerable<ProductViewModel> viewModelList = products.Select(p => new ProductViewModel
		{
            Id = p.Id,
			Name = p.Name,
			Price = p.Price,
			Count = p.Count,
			InventoryValue = p.Price * p.Count
		}).ToList();

		return View(viewModelList);
	}

    // GET: PRODUCTS/Search/Category
    [HttpGet]
	public async Task<IActionResult> Search(string category, string searchProduct)
    {
        List<Product> products = new List<Product>();
        var categories = await _context.Product.Select(p => p.Category).Distinct().ToListAsync();

        //GET products from DB if user want to search by category
        if (!string.IsNullOrWhiteSpace(category))
            products = await _context.Product.Where(p => p.Category.Contains(category)).ToListAsync();

        //Get products from DB if list of products are empty
        if (!string.IsNullOrEmpty(searchProduct) && !products.Any())
            products = await _context.Product.Where(p => p.Name.Contains(searchProduct)).ToListAsync();
        else if (!string.IsNullOrEmpty(searchProduct))
            products = products.Where(p => p.Name.Contains(searchProduct, StringComparison.OrdinalIgnoreCase)).ToList();

		ViewBag.SearchProduct = searchProduct;
		ViewBag.SearchCategory = category;
		ViewBag.Categories = new SelectList(categories, category);

		return View("Search", products);
	}

    // GET: PRODUCTS/Create
    [HttpGet]
	public IActionResult Create()
    {
        return View();
    }

    // POST: PRODUCTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Price,Orderdate,Category,Shelf,Count,Description")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

	// GET: PRODUCTS/Edit/5
	[HttpGet]
	public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Product.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // POST: PRODUCTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Price,Orderdate,Category,Shelf,Count,Description")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

	// GET: PRODUCTS/Delete/5
	[HttpGet]
	public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Product
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: PRODUCTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var product = await _context.Product.FindAsync(id);
        if (product != null)
        {
            _context.Product.Remove(product);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int? id)
    {
        return _context.Product.Any(e => e.Id == id);
    }
}
