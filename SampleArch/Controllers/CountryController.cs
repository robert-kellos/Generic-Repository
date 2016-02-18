using System.Web.Mvc;
using SampleArch.Model;
using SampleArch.Service;

namespace SampleArch.Controllers
{
    public class CountryController : Controller
    {
        //initialize service object
        ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        //
        // GET: /Country/
        public ActionResult Index()
        {
            return View(_countryService.GetAll());
        }

        //
        // GET: /Country/Details/5
        public ActionResult Details(int id)
        {
            

            return View();
        }

        //
        // GET: /Country/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Country/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Country country)
        {

            // TODO: Add insert logic here
            if (ModelState.IsValid)
            {
                _countryService.Add(country);
                return RedirectToAction("Index");
            }
            return View(country);

        }

        //
        // GET: /Country/Update/5
        public ActionResult Edit(int id)
        {            
            Country country = _countryService.GetById(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        //
        // POST: /Country/Update/5
        [HttpPost]
        public ActionResult Edit(Country country)
        {

            if (ModelState.IsValid)
            {
                _countryService.Update(country);
                return RedirectToAction("Index");
            }
            return View(country);

        }

        //
        // GET: /Country/Delete/5
        public ActionResult Delete(int id)
        {
            Country country = _countryService.GetById(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        //
        // POST: /Country/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection data)
        {
            Country country = _countryService.GetById(id);
            _countryService.Delete(country);
            return RedirectToAction("Index");
        }
    }
}
