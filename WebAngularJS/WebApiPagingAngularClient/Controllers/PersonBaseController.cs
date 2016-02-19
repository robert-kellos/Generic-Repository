using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Web.Http;
using SampleArch.Model;
using SampleArch.Service;
using WebApiPagingAngularClient.Controllers.Base;

namespace WebApiPagingAngularClient.Controllers
{
    [RoutePrefix("api/PersonBase")]
    public class PersonBaseController : BaseApiController<Person>
    {
        private readonly CancellationToken _cancellationToken;

        protected PersonBaseController(IPersonService service) : base(service)
        {
            _service = service;

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }

        [HttpGet]
        public override IEnumerable<Person> GetAll()
        {
            return base.GetAll();
        }

        [HttpGet]
        public override IEnumerable<Person> FindBy(Expression<Func<Person, bool>> predicate)
        {
            return base.FindBy(predicate);
        }
    }
}
