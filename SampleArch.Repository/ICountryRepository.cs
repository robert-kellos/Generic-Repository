using System;
using SampleArch.Model;
using SampleArch.Repository.Common;

namespace SampleArch.Repository
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Country GetById(int id);
    }
}
