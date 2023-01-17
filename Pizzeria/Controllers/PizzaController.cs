using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Server;
using Pizzeria.Database;
using Pizzeria.Models;

namespace Pizzeria.Controllers
{
    public class PizzaController : Controller
    {
        public IActionResult Index()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> PizzaList = db.Pizzas.ToList<Pizza>();
                return View("Index", PizzaList);
            }
        }

        public IActionResult Details(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                // LINQ: syntax methos
                Pizza FoundPizza = db.Pizzas
                    .Where(DbPizza => DbPizza.Id == id)
                    .Include(pizza => pizza.Category)
                    .FirstOrDefault();

                if (FoundPizza != null)
                {
                    return View(FoundPizza);
                }

                return NotFound("La pizza che stai cercando non esiste!");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Category> categoriesFromDb = db.Categories.ToList<Category>();

                PizzaCategoriesView modelForView = new PizzaCategoriesView();
                modelForView.Pizza = new Pizza();

                modelForView.Categories = categoriesFromDb;

                return View("Create", modelForView);
            }


        }
        [HttpGet]
        public ActionResult Edit(int id) 
        { 
            using (PizzaContext db = new PizzaContext())
            {
                Pizza PizzaToEdit = db.Pizzas
                    .Where(DbPizza => DbPizza.Id == id)
                    .FirstOrDefault();

                if (PizzaToEdit == null)
                {
                    
                    return NotFound("La pizza che cerchi di modificare non esiste!");
                }

                List<Category> categories= db.Categories.ToList<Category>();

                PizzaCategoriesView modelForView = new PizzaCategoriesView();
                modelForView.Pizza = PizzaToEdit;
                modelForView.Categories = categories;
                return View("Edit", modelForView);
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza PizzaToDelete = db.Pizzas
                    .Where(DbPizza => DbPizza.Id == id)
                    .FirstOrDefault();

                if (PizzaToDelete == null)
                {
                    return NotFound("La pizza che cerchi di eliminare non esiste!");
                }

                List<Category> categories = db.Categories.ToList<Category>();

                PizzaCategoriesView modelForView = new PizzaCategoriesView();
                modelForView.Pizza = PizzaToDelete;
                modelForView.Categories = categories;
                return View("Delete", modelForView);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaCategoriesView formData)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext db = new PizzaContext())
                {
                    List<Category> categories = db.Categories.ToList<Category>();

                    formData.Categories = categories;
                }
                return View("Create", formData);
            }

            using(PizzaContext db = new PizzaContext())
            {
                db.Pizzas.Add(formData.Pizza);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PizzaCategoriesView formData)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext db = new PizzaContext())
                {
                    List<Category> categories = db.Categories.ToList<Category>();

                    formData.Categories = categories;
                }

                return View("Edit", formData);
            }

            using (PizzaContext db = new PizzaContext())
            {
                Pizza PizzaToEdit = db.Pizzas
                    .Where(DbPizza => DbPizza.Id == id)
                    .FirstOrDefault();
                if (PizzaToEdit != null)
                {
                    PizzaToEdit.Name = formData.Pizza.Name;
                    PizzaToEdit.Description = formData.Pizza.Description;
                    PizzaToEdit.Image = formData.Pizza.Image;
                    PizzaToEdit.CategoryId = formData.Pizza.CategoryId;
                    
                    db.SaveChanges();
                }
                else
                {
                    return NotFound("La pizza che volevi modificare non esiste!");
                }

                return RedirectToAction("Index");
                
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, PizzaCategoriesView formdata) 
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza PizzaToDelete = db.Pizzas
                    .Where(DbPizza => DbPizza.Id == id)
                    .FirstOrDefault();
                if (PizzaToDelete != null)
                {
                    db.Pizzas.Remove(PizzaToDelete);
                    db.SaveChanges() ;
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
        }
            
    }
}
