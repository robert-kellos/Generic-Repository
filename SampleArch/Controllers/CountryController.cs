using System.Threading;
using System.Web.Mvc;
using SampleArch.Model;
using SampleArch.Service;

namespace SampleArch.Controllers
{
    public class CountryController : Controller
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;

        //initialize service object
        readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
            
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
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
            if (!ModelState.IsValid)
            {
                _cancellationTokenSource.Cancel();
                return View(country);
            }
            _countryService.Add(country);

            return RedirectToAction("Index");
        }

        //
        // GET: /Country/Update/5
        public ActionResult Edit(int id)
        {            
            var country = _countryService.GetById(id);
            if (country == null)
            {
                _cancellationTokenSource.Cancel();
                return HttpNotFound();
            }
            return View(country);
        }

        //
        // POST: /Country/Update/5
        [HttpPost]
        public ActionResult Edit(Country country)
        {
            if (!ModelState.IsValid)
            {
                _cancellationTokenSource.Cancel();
                return View(country);
            }
            _countryService.Update(country);

            return RedirectToAction("Index");
        }

        //
        // GET: /Country/Delete/5
        public ActionResult Delete(int id)
        {
            var country = _countryService.GetById(id);
            if (country == null)
            {
                _cancellationTokenSource.Cancel();
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
            var country = _countryService.GetById(id);
            _countryService.Delete(country);

            return RedirectToAction("Index");
        }
    }
}
