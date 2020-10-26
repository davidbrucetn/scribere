using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface ICountryRepository
    {
        void AddCountry(Country country);
        void DeleteCountry(int countryId);
        List<Country> GetAll();
        Country GetCountryById(int countryId);
        void UpdateCountry(Country country);
    }
}