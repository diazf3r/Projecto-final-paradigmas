using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Projecto_paradigmas.Controllers
{
    public class ReservaCoworking : Controller
    {
        // GET: ReservaCoworking
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReservaCoworking/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReservaCoworking/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReservaCoworking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReservaCoworking/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReservaCoworking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReservaCoworking/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReservaCoworking/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
