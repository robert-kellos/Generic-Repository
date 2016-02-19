using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using FluentValidation.Attributes;
using SampleArch.Logging;
using SampleArch.Model;
using SampleArch.Service;
using SampleArch.Validation.Validators;

namespace WebApiPagingAngularClient.Controllers
{
    [Validator(typeof(PersonValidator))]
    public class PersonController : ApiController
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;

        //initialize service object
        private readonly IPersonService _personService;

        //DEFAULT cstr not required when using IoC, below auto-instantiated with AutoFac

        public PersonController(IPersonService personService)
        {
            _personService = personService;

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }

        // GET: api/Person
        [ResponseType(typeof(IEnumerable<Person>))]
        public async Task<IEnumerable<Person>> GetPersons()
        {
            return await _personService.GetAllAsync(_cancellationToken);
        }

        // GET: api/Person/5
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> GetPerson(int id)
        {
            var person = await _personService.GetByIdAsync(id, _cancellationToken);
            
            if (person != null) return Ok(person);
            _cancellationTokenSource.Cancel();

            return NotFound();
        }

        // PUT: api/Person/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPerson(int id, Person person)
        {
            if (!ModelState.IsValid)
            {
                _cancellationTokenSource.Cancel();
                return BadRequest(ModelState);
            }

            //Id is a required field to pass currently
            if (id != person.Id)
            {
                _cancellationTokenSource.Cancel();
                return NotFound();
            }

            try
            {
                if (PersonExists(id))
                {
                   await _personService.UpdateAsync(person, _cancellationToken);
                    //--> UnitOfWork gets called on Update, Save called there
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (PersonExists(id)) throw;
                _cancellationTokenSource.Cancel();

                return NotFound();
            }

            return Ok(person);
        }

        // POST: api/Person
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> PostPerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                _cancellationTokenSource.Cancel();
                return BadRequest(ModelState);
            }

            var validatedResults = PersonValidator.Instance.Validate(person);
            if (validatedResults.IsValid)
            {
                person = await _personService.AddAsync(person, _cancellationToken);
                //--> UnitOfWork gets called on Update, Save called there
            }
            else
            {
                var errors = string.Join(", ", validatedResults.Errors);

                Audit.Log.Debug(errors);

                return Ok(errors);
            }

            return CreatedAtRoute("DefaultApi", new { id = person.Id }, person);
        }

        // DELETE: api/Person/5
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> DeletePerson(int id)
        {
            var person = await _personService.GetByIdAsync(id, _cancellationToken);
            if (person == null)
            {
                _cancellationTokenSource.Cancel();
                return NotFound();
            }

            person = await _personService.DeleteAsync(person, _cancellationToken);
            //--> UnitOfWork gets called on Update, Save called there

            return Ok(person);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //any resources created
            }
            base.Dispose(disposing);
        }

        private bool PersonExists(int id)
        {
            return _personService.FindBy(e => e.Id == id).AsParallel().Any();
        }
    }
}