using System.Net;
using System.Threading;
using System.Web.Mvc;
using SampleArch.Model;
using SampleArch.Service;

namespace SampleArch.Controllers
{
    public class PersonController : Controller
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;

        readonly IPersonService _personService;
        readonly ICountryService _countryService;
        public PersonController(IPersonService personService, ICountryService countryService)
        {
            _personService = personService;
            _countryService = countryService;
            
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }

        // GET: /Person/
        public ActionResult Index()
        {
            return View(_personService.GetAll());
        }

        // GET: /Person/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                _cancellationTokenSource.Cancel();
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var person = _personService.GetById(id.Value);
            if (person != null) return View(person);
            _cancellationTokenSource.Cancel();

            return HttpNotFound();
        }

        // GET: /Person/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(_countryService.GetAll(), "Id", "Name");
            return View();
        }

        // POST: /Person/Create
        // To protect from over-posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Phone,Address,State,CountryId")] Person person)
        {
            if (ModelState.IsValid)
            {
                _personService.Add(person);
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(_countryService.GetAll(), "Id", "Name", person.CountryId);
            return View(person);
        }

        // GET: /Person/Update/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                _cancellationTokenSource.Cancel();
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var person = _personService.GetById(id.Value);
            if (person == null)
            {
                _cancellationTokenSource.Cancel();
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(_countryService.GetAll(), "Id", "Name", person.CountryId);

            return View(person);
        }

        // POST: /Person/Update/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Phone,Address,State,CountryId")] Person person)
        {
            if (ModelState.IsValid)
            {
                _personService.Update(person);
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(_countryService.GetAll(), "Id", "Name", person.CountryId);

            return View(person);
        }

        // GET: /Person/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                _cancellationTokenSource.Cancel();
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var person = _personService.GetById(id.Value);
            if (person != null) return View(person);
            _cancellationTokenSource.Cancel();

            return HttpNotFound();
        }

        // POST: /Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var person = _personService.GetById(id);
            _personService.Delete(person);

            return RedirectToAction("Index");
        }
    }
}
