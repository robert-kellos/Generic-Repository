using SampleArch.Model;
using SampleArch.Repository;
using SampleArch.Repository.Common;
using SampleArch.Service.Common;

namespace SampleArch.Service
{
    public class CountryService : EntityService<Country>, ICountryService
    {
        private IUnitOfWork _unitOfWork;
        private readonly ICountryRepository _countryRepository;
        
        public CountryService(IUnitOfWork unitOfWork, ICountryRepository countryRepository)
            : base(unitOfWork, countryRepository)
        {
            _unitOfWork = unitOfWork;
            _countryRepository = countryRepository;
        }


        public Country GetById(int id) {
            return _countryRepository.GetById(id);
        }
    }
}
