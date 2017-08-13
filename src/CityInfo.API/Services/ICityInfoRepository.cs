using System.Collections;
using System.Collections.Generic;
using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable GetCities();
        City GetCity(int id, bool includePointsOfInterest);
        IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId);
        PointOfInterest GetPointOfInterestForCity(int cityId, int id);
    }
}
