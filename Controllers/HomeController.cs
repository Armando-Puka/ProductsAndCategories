using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductsAndCategories.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductsAndCategories.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
     private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)    
    {        
        _logger = logger;
        _context = context;    
    } 

    public IActionResult Index()
    {
        ViewBag.Products = _context.Products.Include(p => p.ProductAssociation).ToList();
        // ViewBag.Products = _context.Products.ToList();

        return View();
    }

    [HttpPost]
    public IActionResult CreateProduct(Product productFromForm)
    {
        if(ModelState.IsValid){
            _context.Add(productFromForm);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // List<Product> products = _context.Products.Include(c => c.ProductAssociation).ToList();
        ViewBag.Products = _context.Products.Include(p => p.ProductAssociation).ToList();
        // ViewBag.Products = _context.Products.ToList();

        return View("Index");
    }

    [HttpGet("Categories")]
    public IActionResult Category()
    {
        ViewBag.Category = _context.Categories.Include(c => c.CategoryAssociation).ToList();
        // List<Category> Categories = _context.Categories.Include(c => c.CategoryAssociation).ToList();
        // ViewBag.Category = _context.Categories.ToList();

        return View();
    }

    [HttpPost]
    public IActionResult CreateCategory(Category categoryFromForm)
    {
        if(ModelState.IsValid)
        {
            _context.Add(categoryFromForm);
            _context.SaveChanges();
            return RedirectToAction("Category");
        }

        // List<Category> Categories = _context.Categories.Include(c => c.CategoryAssociation).ToList();
        ViewBag.Category = _context.Categories.Include(c => c.CategoryAssociation).ToList();
        // ViewBag.Category = _context.Categories.ToList();

        return View("Category");
    }

    [HttpGet("product/{id}")]
    public IActionResult Product(int id)
    {

        Product product = _context.Products.Include(p => p.ProductAssociation).ThenInclude(a => a.Category).FirstOrDefault(e => e.ProductId == id);

        // ViewBag.Products = product;

        ViewBag.ExistingCategories = product?.ProductAssociation.Select(a => a.Category).ToList() ?? new List<Category>();

        // Product Products = _context.Products.FirstOrDefault(e => e.ProductId == id);
        // ViewBag.Products = Products;

        List<Category> Categories = _context.Categories.ToList();
        ViewBag.CategoriesToInclude = Categories;

        return View(product);
    }

    [HttpGet("categories/{id}")]
    public IActionResult Categories(int id)
    {

        Category category = _context.Categories.Include(p => p.CategoryAssociation).ThenInclude(a => a.Product).FirstOrDefault(e => e.CategoryId == id);

        // ViewBag.Products = product;

        ViewBag.ExistingProducts = category?.CategoryAssociation.Select(a => a.Product).ToList() ?? new List<Product>();

        // Product Products = _context.Products.FirstOrDefault(e => e.ProductId == id);
        // ViewBag.Products = Products;

        List<Product> products = _context.Products.ToList();
        ViewBag.ProductsToInclude = products;

        return View(category);
    }

    [HttpPost]
    public IActionResult AddCategory(int productId, int categoryId) {
        if (ModelState.IsValid) {
            if (!_context.Associations.Any(a => a.ProductId == productId && a.CategoryId == categoryId)) {
                Association newAssociation = new Association {
                    ProductId = productId,
                    CategoryId = categoryId
                };

                _context.Associations.Add(newAssociation);
                _context.SaveChanges();
                
                }
            }
            return RedirectToAction("Product", new { id = productId });
    }

    [HttpPost]
    public IActionResult AddProduct(int productId, int categoryId) {
        if (ModelState.IsValid) {
            Console.WriteLine($"Attempting to add product {productId} to category {categoryId}");

            if (!_context.Associations.Any(a => a.CategoryId == categoryId && a.ProductId == productId)) {
                Association newAssociation = new Association {
                    ProductId = productId,
                    CategoryId = categoryId
                };

                _context.Associations.Add(newAssociation);
                _context.SaveChanges();
                
                }
            }
            Console.WriteLine("Product added to category successfully");

            return RedirectToAction("Categories", new { id = categoryId });
    }

    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
