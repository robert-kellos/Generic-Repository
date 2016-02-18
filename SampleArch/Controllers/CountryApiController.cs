﻿using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SampleArch.Service;
using SampleArch.Model;

namespace SampleArch.Controllers
{
    public class CountryApiController : ApiController
    {
        //TODO: validate CancellationToken is efficiently implemented
        private readonly CancellationToken _cancellationToken;

        //initialize service object
        private readonly ICountryService _countryService;


        //DEFAULT cstr not required when using IoC, below auto-instantiated with AutoFac

        public CountryApiController(ICountryService countryService)
        {
            _countryService = countryService;
            _cancellationToken = new CancellationToken();
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
            Country country = await _countryService.GetByIdAsync(id, _cancellationToken);
            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        // PUT: api/Country/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCountry(int id, Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Id is a required field to pass currently
            if (id != country.Id)
            {
                return BadRequest();
            }
            //--> set with AutoChangeTracking

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
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Country
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> PostCountry(Country country)
        {
            if (!ModelState.IsValid)
            {
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
            Country country = await _countryService.GetByIdAsync(id, _cancellationToken).ConfigureAwait(true);
            if (country == null)
            {
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
            return _countryService.GetAll().Count(e => e.Id == id) > 0;
        }
    }
}