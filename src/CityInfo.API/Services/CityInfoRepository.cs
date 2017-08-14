using System;
using System.Collections.Generic;
using System.Linq;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int id, bool includePointsOfInterest)
        {
            return _context.Cities.Include(c => c.PointsOfInterest).FirstOrDefault(c => c.Id == id);
        }

        public bool CityExists(int id)
        {
            return _context.Cities.Any(c => c.Id == id);
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId)
        {
            return _context.PointOfInterests.Where(p => p.CityId == cityId).ToList();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int id)
        {
            return _context.PointOfInterests.FirstOrDefault(p => p.CityId == cityId && p.Id == id);
        }

        public void AddPointOfInterest(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, true);
            city.PointsOfInterest.Add(pointOfInterest);
            Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
