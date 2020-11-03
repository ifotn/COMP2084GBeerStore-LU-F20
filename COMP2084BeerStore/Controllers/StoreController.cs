using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COMP2084BeerStore.Data;
using COMP2084BeerStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace COMP2084BeerStore.Controllers
{
    public class StoreController : Controller
    {
        // db connection
        private readonly ApplicationDbContext _context;

        // constructor that instantiates an instance of our db connection
        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //// use our mock Category model to create and display 10 categories
            //// first, create an object to hold a list of categories
            //var categories = new List<Category>();

            //for (var i = 1; i <= 10; i++)
            //{
            //    categories.Add(new Category { Id = i, Name = "Category " + i.ToString() });
            //}

            // use the Categories DbSet in ApplicationDbContext to query the db for the list of Categories
            var categories = _context.Categories.OrderBy(c => c.Name).ToList();

            // modify the return View statement so that it now accepts a list of categories to pass to the view for display
            return View(categories);
        }

        // /Store/Browse/6
        public IActionResult Browse(int id)
        {
            // query Products for the selected Category
            var products = _context.Products.Where(p => p.CategoryId == id).OrderBy(p => p.ProductName).ToList();

            // get Name of selected Category.  Find() only filters on key fields
            ViewBag.category = _context.Categories.Find(id).Name.ToString();
            return View(products);
        }

        // /Store/AddCategory
        public IActionResult AddCategory()
        {
            // load a form to capture a new category object from the user
            return View();
        }

        // /Store/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int ProductId, int Quantity)
        {
            // query the db for the product price
            var price = _context.Products.Find(ProductId).Price;

            // get current Date & Time using built in .net function
            var currentDateTime = DateTime.Now;

            // create and save a new Cart object
            var cart = new Cart
            {
                ProductId = ProductId,
                Quantity = Quantity,
                Price = price,
                DateCreated = currentDateTime,
                CustomerId = "Test" // we will make this dynamic next
            };

            _context.Carts.Add(cart);
            _context.SaveChanges();

            // redirect to the Cart view
            return RedirectToAction("Cart");
        }

        // /Store/Cart
        public IActionResult Cart()
        {
            return View();
        }
    }
}
