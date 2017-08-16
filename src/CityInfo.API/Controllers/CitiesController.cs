using System.Collections.Generic;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : BaseController
    {
        public CitiesController(ICityInfoRepository repository) : base(repository)
        {
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cityEntities = _repository.GetCities();
            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _repository.GetCity(id, includePointsOfInterest);

            if (city == null)
                return NotFound();

            if (includePointsOfInterest)
            {
                var cityResultWithPoints = Mapper.Map<CityDto>(city);
                return Ok(cityResultWithPoints);
            }

            var cityResult = Mapper.Map<CityWithoutPointsOfInterestDto>(city);
            return Ok(cityResult);
        }
    }
}
