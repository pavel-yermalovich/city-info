using System.Linq;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : BaseController
    {
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterests(int cityId)
        {
            var city = CitiesDataStore.GetCity(cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.GetCity(cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The name and description should be different.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.GetCity(cityId);

            if (city == null)
            {
                return NotFound();
            }

            var maxPointOfInterstId = CitiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            var pointOfInterestToAdd = new PointOfInterestDto()
            {
                Id = maxPointOfInterstId++,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(pointOfInterestToAdd);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, pointOfInterestToAdd.Id });
        }
    }
}
