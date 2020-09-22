﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COMP2084BeerStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace COMP2084BeerStore.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            // use our mock Category model to create and display 10 categories
            // first, create an object to hold a list of categories
            var categories = new List<Category>();

            for (var i = 1; i <= 10; i++)
            {
                categories.Add(new Category { Id = i, Name = "Category " + i.ToString() });
            }

            // modify the return View statement so that it now accepts a list of categories to pass to the view for display
            return View(categories);
        }

        // /Store/Browse
        public IActionResult Browse(string category)
        {
            ViewBag.category = category;
            return View();
        }

        // /Store/AddCategory
        public IActionResult AddCategory()
        {
            // load a form to capture a new category object from the user
            return View();
        }
    }
}
