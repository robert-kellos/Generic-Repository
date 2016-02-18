using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleArch.Model;
using SampleArch.Repository.Common;

namespace SampleArch.Repository
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        Person GetById(long id);
    }
}
