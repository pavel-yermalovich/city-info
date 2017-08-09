using System.Linq;
using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
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
        public IActionResult CreatePointOfInterest(int cityId, 
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
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

            var pointOfInterestToAdd = new PointOfInterestDto
            {
                Id = maxPointOfInterstId++,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(pointOfInterestToAdd);

            return CreatedAtRoute("GetPointOfInterest", new {cityId, pointOfInterestToAdd.Id });
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
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

            var pointOfInterstFromStore = city.GetPointOfInterestById(id);

            if (pointOfInterstFromStore == null)
            {
                return NotFound();
            }

            pointOfInterstFromStore.Name = pointOfInterest.Name;
            pointOfInterstFromStore.Description = pointOfInterest.Name;

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return NotFound();
            }

            var city = CitiesDataStore.GetCity(cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterstFromStore = city.GetPointOfInterestById(id);

            if (pointOfInterstFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto
            {
                Name = pointOfInterstFromStore.Name,
                Description = pointOfInterstFromStore.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pointOfInterstFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterstFromStore.Description = pointOfInterestToPatch.Name;

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.GetCity(cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterstFromStore = city.GetPointOfInterestById(id);

            if (pointOfInterstFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterstFromStore);

            return NoContent();
        }
    }
}
