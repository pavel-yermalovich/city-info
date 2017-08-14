using System.Collections;
using System.Collections.Generic;
using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        City GetCity(int id, bool includePointsOfInterest);
        bool CityExists(int id);
        IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId);
        PointOfInterest GetPointOfInterestForCity(int cityId, int id);
        void AddPointOfInterest(int cityId, PointOfInterest pointOfInterest);
        bool Save();
    }
}
