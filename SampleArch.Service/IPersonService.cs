using SampleArch.Model;
using SampleArch.Service.Common;

namespace SampleArch.Service
{
    public interface IPersonService : IEntityService<Person>
    {
        Person GetById(long id);
    }
}
