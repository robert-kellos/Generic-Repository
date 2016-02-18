using SampleArch.Model;
using SampleArch.Service.Common;

namespace SampleArch.Service
{
    public interface ICountryService : IEntityService<Country>
    {
        Country GetById(int id);
    }
}
