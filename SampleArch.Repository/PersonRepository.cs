using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleArch.Model;
using SampleArch.Repository.Common;

namespace SampleArch.Repository
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(DbContext context)
            : base(context)
        {
            //   
        }

        public override IEnumerable<Person> GetAll()
        {
            return _currentDbContext.Set<Person>().Include(x=>x.Country).AsEnumerable(); 
        }

        public Person GetById(long id)
        {
            return _currentDbSet.Include(x=>x.Country).FirstOrDefault(x => x.Id == id);            
        }
    }
}
