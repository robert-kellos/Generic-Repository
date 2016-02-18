using SampleArch.Model;
using SampleArch.Repository;
using SampleArch.Repository.Common;
using SampleArch.Service.Common;

namespace SampleArch.Service
{
    public class PersonService : EntityService<Person>, IPersonService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IPersonRepository _personRepository;
        
        public PersonService(IUnitOfWork unitOfWork, IPersonRepository personRepository)
            : base(unitOfWork, personRepository)
        {
            _unitOfWork = unitOfWork;
            _personRepository = personRepository;
        }


        public Person GetById(long id) {
            return _personRepository.GetById(id);
        }
    }
}
