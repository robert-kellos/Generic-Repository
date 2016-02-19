using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SampleArch.Model;
using SampleArch.Service;

namespace WebApiPagingAngularClient.Controllers
{
    public class CountryController : ApiController
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;

        //initialize service object
        private readonly ICountryService _countryService;

        //DEFAULT cstr not required when using IoC, below auto-instantiated with AutoFac

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }

        // GET: api/Country
        [ResponseType(typeof(IEnumerable<Country>))]
        public IEnumerable<Country> GetCountries()
        {
            return _countryService.GetAll();
        }

        // GET: api/Country/5
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> GetCountry(int id)
        {
            var country = await _countryService.GetByIdAsync(id, _cancellationToken);

            if (country != null) return Ok(country);
            _cancellationTokenSource.Cancel();

            return NotFound();
        }

        // PUT: api/Country/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCountry(int id, Country country)
        {
            if (!ModelState.IsValid)
            {
                _cancellationTokenSource.Cancel();
                return BadRequest(ModelState);
            }

            //Id is a required field to pass currently
            if (id != country.Id)
            {
                _cancellationTokenSource.Cancel();
                return NotFound();
            }

            try
            {
                if (CountryExists(id))
                {
                    await _countryService.UpdateAsync(country, _cancellationToken); 
                    //--> UnitOfWork gets called on Update, Save called there
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (CountryExists(id)) throw;
                _cancellationTokenSource.Cancel();
                return NotFound();
            }

            return Ok(country);
        }

        // POST: api/Country
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> PostCountry(Country country)
        {
            if (!ModelState.IsValid)
            {
                _cancellationTokenSource.Cancel();
                return BadRequest(ModelState);
            }

            await _countryService.AddAsync(country, _cancellationToken);
            //--> UnitOfWork gets called on Update, Save called there

            return CreatedAtRoute("DefaultApi", new { id = country.Id }, country);
        }

        // DELETE: api/Country/5
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> DeleteCountry(int id)
        {
            var country = await _countryService.GetByIdAsync(id, _cancellationToken);
            if (country == null)
            {
                _cancellationTokenSource.Cancel();
                return NotFound();
            }

            await _countryService.DeleteAsync(country, _cancellationToken);
            //--> UnitOfWork gets called on Update, Save called there

            return Ok(country);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //any resources created
            }
            base.Dispose(disposing);
        }

        private bool CountryExists(int id)
        {
            return _countryService.FindBy(e => e.Id == id).AsParallel().Any();
        }
    }
}