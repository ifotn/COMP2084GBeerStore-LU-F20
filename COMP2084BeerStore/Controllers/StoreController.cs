using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COMP2084BeerStore.Data;
using COMP2084BeerStore.Models;
using Microsoft.AspNetCore.Http;
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

            // CustomerId variable
            var CustomerId = GetCustomerId();

            // create and save a new Cart object
            var cart = new Cart
            {
                ProductId = ProductId,
                Quantity = Quantity,
                Price = price,
                DateCreated = currentDateTime,
                CustomerId = CustomerId
            };

            _context.Carts.Add(cart);
            _context.SaveChanges();

            // redirect to the Cart view
            return RedirectToAction("Cart");
        }

        private string GetCustomerId()
        {
            // check the session for an existing CustomerId
            if (HttpContext.Session.GetString("CustomerId") == null) {
                // if we don't already have an existing CustomerId in the session, check if customer is logged in
                var CustomerId = "";

                // if customer is logged in, use their email as the CustomerId
                if (User.Identity.IsAuthenticated)
                {
                    CustomerId = User.Identity.Name;
                }
                // if the customer is anonymous, use Guid to create a new identifier
                else
                {
                    CustomerId = Guid.NewGuid().ToString();
                } 

                // now store the CustomerId in a session variable
                HttpContext.Session.SetString("CustomerId", CustomerId);
            }

            // return the Session variable
            return HttpContext.Session.GetString("CustomerId");
        }

        // /Store/Cart
        public IActionResult Cart()
        {
            // fetch current cart for display
            var CustomerId = "";

            // in case user comes to cart page before adding anything, identify them first
            if (HttpContext.Session.GetString("CustomerId") == null)
            {
                CustomerId = GetCustomerId();
            }
            else
            {
                CustomerId = HttpContext.Session.GetString("CustomerId");
            }

            // query the db for this customer
            var cartItems = _context.Carts.Where(c => c.CustomerId == CustomerId).ToList();

            // pass the data to the view for display
            return View(cartItems);
        }
    }
}
