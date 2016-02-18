using System.Collections.Generic;
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
    public class PersonApiController : ApiController
    {
        private readonly CancellationToken _cancellationToken;

        //initialize service object
        private readonly IPersonService _personService;


        //DEFAULT cstr not required when using IoC, below auto-instantiated with AutoFac

        public PersonApiController(IPersonService personService)
        {
            _personService = personService;
            _cancellationToken = new CancellationToken();
        }

        // GET: api/Person
        [ResponseType(typeof(IEnumerable<Person>))]
        public IEnumerable<Person> GetPersons()
        {
            return _personService.GetAll();
        }

        // GET: api/Person/5
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> GetPerson(int id)
        {
            var person = await _personService.GetByIdAsync(id, _cancellationToken);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/Person/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPerson(int id, Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Id is a required field to pass currently
            if (id != person.Id)
            {
                return BadRequest();
            }

            //db.Entry(Person).State = EntityState.Modified;
            //--> set with AutoChangeTracking

            try
            {
                if (PersonExists(id))
                {
                    await _personService.UpdateAsync(person, _cancellationToken);
                    //db.SaveChangesAsync(); 
                    //--> UnitOfWork gets called on Update, Save called there
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Person
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> PostPerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //db.Countries.Add(Person);
            person = await _personService.AddAsync(person, _cancellationToken);
            //await db.SaveChangesAsync();
            //--> UnitOfWork gets called on Update, Save called there

            return CreatedAtRoute("DefaultApi", new { id = person.Id }, person);
        }

        // DELETE: api/Person/5
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> DeletePerson(int id)
        {
            var person = await _personService.GetByIdAsync(id, _cancellationToken);
            if (person == null)
            {
                return NotFound();
            }

            await _personService.DeleteAsync(person, _cancellationToken);
            //db.Countries.Remove(Person);
            //await db.SaveChangesAsync();
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
            return _personService.GetAll().Count(e => e.Id == id) > 0;
        }
    }
}