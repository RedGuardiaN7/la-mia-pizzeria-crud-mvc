using Microsoft.AspNetCore.Mvc;
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
                Pizza FoundPizza = db.Pizzas
                    .Where(DbPizza => DbPizza.Id == id)
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
            return View("Create");
        }
        [HttpGet]
        public ActionResult Edit(int id) 
        { 
            using (PizzaContext db = new PizzaContext())
            {
                Pizza PizzaToEdit = db.Pizzas
                    .Where(DbPizza => DbPizza.Id == id)
                    .FirstOrDefault();

                if (PizzaToEdit != null)
                {
                    return View(PizzaToEdit);
                }

                return NotFound("La pizza che cerchi di modificare non esiste!");
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

                if (PizzaToDelete != null)
                {
                    return View(PizzaToDelete);
                }

                return NotFound("La pizza che cerchi di eliminare non esiste!");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza formData)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", formData);
            }

            using(PizzaContext db = new PizzaContext())
            {
                db.Pizzas.Add(formData);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Pizza formData)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", formData);
            }
            using (PizzaContext db = new PizzaContext())
            {
                Pizza PizzaToEdit = db.Pizzas
                    .Where(DbPizza => DbPizza.Id == id)
                    .FirstOrDefault();
                if (PizzaToEdit != null)
                {
                    PizzaToEdit.Name = formData.Name;
                    PizzaToEdit.Description = formData.Description;
                    PizzaToEdit.Image = formData.Image;
                    db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }

                return RedirectToAction("Index");
                
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Pizza formdata) 
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
