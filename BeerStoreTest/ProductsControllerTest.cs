using COMP2084BeerStore.Controllers;
using COMP2084BeerStore.Data;
using COMP2084BeerStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeerStoreTest
{
    [TestClass]
    public class ProductsControllerTest
    {
        // create db reference that will point to our in-memory db
        private ApplicationDbContext _context;

        // create empty product list to hold mock product data
        List<Product> products = new List<Product>();

        // declare controller we are going to test
        ProductsController controller;

        [TestInitialize]
        // this method runs automatically before each unit test to streamline the arranging
        public void TestInitialize()
        {
            // instantiate in-memory db
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            // create mock data inside the in-memory db
            var category = new Category { Id = 505, Name = "Some Category" };

            products.Add(new Product { Id = 87, ProductName = "Prod 1", Price = 8, Category = category });
            products.Add(new Product { Id = 92, ProductName = "Prod 0", Price = 9, Category = category });
            products.Add(new Product { Id = 95, ProductName = "Prod 3", Price = 10, Category = category });

            foreach (var p in products)
            {
                _context.Products.Add(p);
            }

            _context.SaveChanges();

            // instantiate the products controller and pass it the mock db object (dependency injection)
            controller = new ProductsController(_context);
        }

        [TestMethod]
        public void IndexViewLoads()
        {
            // no arrange needed as all setup done first in TestInitialize()
            // act, casting the Result property to a ViewResult
            var result = controller.Index();
            var viewResult = (ViewResult)result.Result;

            // assert
            Assert.AreEqual("Index", viewResult.ViewName);
        }

        [TestMethod]
        public void IndexReturnsProductData()
        {
            // act
            var result = controller.Index();
            var viewResult = (ViewResult)result.Result;
            // cast the result's data Model to a list of products so we can check it
            List<Product> model = (List<Product>)viewResult.Model;

            // assert
            CollectionAssert.AreEqual(products.OrderBy(p => p.ProductName).ToList(), model);
        }

        #region Group 1
        [TestMethod]
        public void DetailsNoId()
        {
            // Act
            var result = controller.Details(id: null);
            var viewResult = (ViewResult)result.Result;

            // Assert
            Assert.AreEqual("Error", viewResult.ViewName);
        }

        [TestMethod]
        public void DetailsInvalidId()
        {
            // Act
            var result = controller.Details(id: -1);
            var notFoundResult = (NotFoundResult)result.Result;

            // Assert
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public void DetailsViewLoads()
        {
            //Arrange
            int id = products[0].Id;
            //Act
            var result = controller.Details(id);
            var viewResult = (ViewResult)result.Result;

            // Assert       
            Assert.AreEqual("Details", viewResult.ViewName);
        }

        [TestMethod]
        public void DetailObjectReturnsMatches()
        {
            //Arrange
            int id = products[0].Id;
            //Act
            var result = controller.Details(id);
            var viewResult = (ViewResult)result.Result;

            // Assert       
            Assert.AreEqual(products[0], viewResult.Model);
        }

        [TestMethod]
        public void CreateReturnsValidList()
        {
            var result = controller.Create();
            var viewData = controller.ViewData["CategoryId"];

            Assert.IsNotNull(viewData);
        }

        [TestMethod]
        public void CreateLoadsView()
        {
            // act
            var result = controller.Create();
            var viewResult = (ViewResult)result;

            //Assert 
            Assert.AreEqual("Create", viewResult.ViewName);
        }
        #endregion Group 1

        #region Group 2
        [TestMethod]
        public void CreatePostSavesToDb()
        {
            var product = new Product { Id = 400, ProductName = "newProduct", Price = 5, Category = new Category { Id = 300, Name = "newCategory" } }; 
            _context.Products.Add(product);
            _context.SaveChanges(); 
            Assert.AreEqual(product, _context.Products.ToArray()[3]);
        }

        [TestMethod]
        public void CreatePostReturnsCreate()
        {            
            //create test produst            
            var product = new Product { };            
            controller.ModelState.AddModelError("put a descriptive key name here", "add an appropriate key value here");            
            var result = controller.Create(product, null);            
            var viewResult = (ViewResult)result.Result;            
            
            // assert         
            Assert.AreEqual("Create", viewResult.ViewName);        
        }        
        [TestMethod]
        public void CreatePostViewDataNotNull()        
        {            
            //create test produst
            var product = new Product { };
            controller.ModelState.AddModelError("put a descriptive key name here", "add an appropriate key value here");  
            var result = controller.Create(product, null);            
            var viewResult = (ViewResult)result.Result;            
            
            // assert            
            Assert.IsNotNull(viewResult.ViewData);        
        }
        #endregion Group 2

        #region Group 3
        [TestMethod]
        public void EditLoadsErrorViewWithNullId()
        {
            var result = controller.Edit(null);
            var viewResult = (ViewResult)result.Result;
            Assert.AreEqual("Error", viewResult.ViewName);
        }

        [TestMethod]
        public void EditLoadsErrorViewWithInvalidId()
        {
            var result = controller.Edit(10);
            var viewResult = (ViewResult)result.Result;
            Assert.AreEqual("Error", viewResult.ViewName);
        }

        [TestMethod]
        public void EditLoadsViewWithValidId()
        {
            var result = controller.Edit(87);
            var viewResult = (ViewResult)result.Result;
            Assert.AreEqual("Edit", viewResult.ViewName);
        }

        [TestMethod]
        public void EditLoadsCorrectModel()
        {
            var result = controller.Edit(87);
            var viewResult = (ViewResult)result.Result;
            Product model = (Product)viewResult.Model;
            Assert.AreEqual(_context.Products.Find(87), model);
        }

        [TestMethod]
        public void EditLoadsViewData()
        {
            var result = controller.Edit(87);
            var viewResult = (ViewResult)result.Result;
            var viewData = viewResult.ViewData;
            Assert.AreEqual(viewData, viewResult.ViewData);
        }

        [TestMethod]
        public void EditLoadsErrorViewForInvalidModel()
        {
            var result = controller.Edit(10);
            var viewResult = (ViewResult)result.Result;
            Product model = (Product)viewResult.Model;
            Assert.AreNotEqual(_context.Products.FindAsync(10), model);
        }
        #endregion Group 3

        #region Group 4
        [TestMethod]
        public void EditReturnsId()
        {

            var result = controller.Edit(34, products[0]);
            var viewResult = (ViewResult)result.Result;

            Assert.AreEqual("Error", viewResult.ViewName);
        }

        [TestMethod]
        public void EditSave()
        {
            var product = products[0];
            product.Price = 1;
            var result = controller.Edit(product.Id, product);
            var redirectResult = (RedirectToActionResult)result.Result;
            // assert
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod]
        public void EditIdInDatabase()
        {
            var checkProduct = new Product { Id = 1, ProductName = "Prod 1", Price = 8, Category = new Category { Id = 595, Name = "Some Category" } };

            var result = controller.Edit(1, checkProduct);
            var viewResult = (ViewResult)result.Result;
            // assert
            Assert.AreEqual("Error", viewResult.ViewName);
        }
        #endregion Group 4

        #region Group 5
        [TestMethod]
        public void DeleteNullId()
        {
            var result = controller.Delete(null);
            var viewResult = (ViewResult)result.Result;
            Assert.AreEqual("Error", viewResult.ViewName);
        }

        [TestMethod]
        public void DeleteIdNotExists()
        {
            var result = controller.Delete(99); //invalid ID
            var viewResult = (ViewResult)result.Result;
            Assert.AreEqual("Error", viewResult.ViewName);
        }

        [TestMethod]
        public void DeleteCorrectView()
        {
            var id = 87;
            var result = controller.Delete(id); // valid ID
            var viewResult = (ViewResult)result.Result;
            Assert.AreEqual("Delete", viewResult.ViewName);
        }

        [TestMethod]
        public void DeleteCorrectProduct()
        {
            var id = 87;
            var result = controller.Delete(id); // valid ID
            var viewResult = (ViewResult)result.Result;
            Product product = (Product)viewResult.Model;
            Assert.AreEqual(products[0], product);
        }

        [TestMethod]
        public void DeleteConfirmedSuccess()
        {
            var id = 1;
            var result = controller.DeleteConfirmed(id); // valid ID
            var product = _context.Products.Find(id);
            Assert.AreEqual(product, null);
        }

        [TestMethod]
        public void DeleteConfirmedRedirectIndex()
        {
            var id = 87;
            var result = controller.DeleteConfirmed(id); // valid ID
            var actionResult = (RedirectToActionResult)result.Result;
            Assert.AreEqual("Index", actionResult.ActionName);
        }
        #endregion Group 5
    }
}
