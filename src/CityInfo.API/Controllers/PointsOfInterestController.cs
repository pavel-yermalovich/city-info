using System;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : BaseController
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService,
            ICityInfoRepository repository) : base(repository)
        {
            _logger = logger;
            _mailService = mailService;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterests(int cityId)
        {
            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointsOfInterest = _repository.GetPointsOfInterest(cityId);

            return Ok(pointsOfInterest);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            try
            {
                if (!_repository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} was not found");
                    return NotFound();
                }

                var pointOfInterest = _repository.GetPointOfInterestForCity(cityId, id);

                if (pointOfInterest == null)
                {
                    return NotFound();
                }

                var pointOfInterestDto = Mapper.Map<PointOfInterestDto>(pointOfInterest);

                return Ok(pointOfInterestDto);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id = {cityId}.", exception);
                return StatusCode(500, "A problem happened while handling your request.");
            }
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

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestToAdd = Mapper.Map<PointOfInterest>(pointOfInterest);
            _repository.AddPointOfInterest(cityId, pointOfInterestToAdd);
            var saveResult = _repository.Save();

            if (!saveResult)
            {
                return StatusCode(500, "A problem happened during adding a point of interest");
            }

            var createdPoint = Mapper.Map<PointOfInterestDto>(pointOfInterestToAdd);

            return CreatedAtRoute("GetPointOfInterest", new {cityId, createdPoint.Id }, createdPoint);
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

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterstEntity = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterstEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(pointOfInterest, pointOfInterstEntity);

            if (!_repository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request");
            }

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterstEntity = _repository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterstEntity == null)
            {
                return NotFound();
            }

            _repository.DeletePointOfInterest(pointOfInterstEntity);
            if (!_repository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request");
            }

            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterstEntity.Name} with id {pointOfInterstEntity.Id} was deleted.");

            return NoContent();
        }
    }
}
