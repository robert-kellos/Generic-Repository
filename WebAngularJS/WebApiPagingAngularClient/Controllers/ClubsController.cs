using System;
using System.Linq;
using System.Threading;
using System.Web.Http;
using WebApiPagingAngularClient.Models;
using WebApiPagingAngularClient.Utility;

namespace WebApiPagingAngularClient.Controllers
{
    [RoutePrefix("api/clubs")]
    public class ClubsController : ApiController
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;

        private readonly ClubRepository _clubRepository;

        public ClubsController(): this(new ClubRepository())
        {

        }

        public ClubsController(ClubRepository repository)
        {
            _clubRepository = repository;

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }

        // GET: api/Clubs
        [Route("")]
        public IHttpActionResult Get()
        {
            var clubs = _clubRepository.Clubs.ToList();

            return Ok(clubs);
        }

        // GET: api/Clubs/5
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var club = _clubRepository.Clubs.FirstOrDefault(c => c.Id == id);

            return Ok(club);
        }

        // GET: api/Clubs/clubName
        [Route("{name:alpha}")]
        public IHttpActionResult Get(string name)
        {
            var club = _clubRepository.Clubs.FirstOrDefault(c => c.Name == name);

            return Ok(club);
        }

        // GET: api/Clubs/pageSize/pageNumber/orderBy(optional)
        [Route("{pageSize:int}/{pageNumber:int}/{orderBy:alpha?}")]
        public IHttpActionResult Get(int pageSize, int pageNumber, string orderBy = "")
        {
            var totalCount = _clubRepository.Clubs.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);
            var clubQuery = _clubRepository.Clubs;

            if (QueryHelper.PropertyExists<Club>(orderBy))
            {
                var orderByExpression = QueryHelper.GetPropertyExpression<Club>(orderBy);
                clubQuery = clubQuery.OrderBy(orderByExpression);
            } else
            {
                clubQuery = clubQuery.OrderBy(c => c.Id);
            }

            var clubs = clubQuery.Skip((pageNumber - 1) * pageSize)                            
                                    .Take(pageSize)                
                                    .ToList();

            var result = new
            {
                TotalCount = totalCount,
                totalPages = totalPages,
                Clubs = clubs
            };

            return Ok(result);
        }
      
        // POST: api/Clubs
        public void Post([FromBody]string value)
        {
            _cancellationTokenSource.Cancel();
        }

        // PUT: api/Clubs/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Clubs/5
        public void Delete(int id)
        {
        }
    }
}
