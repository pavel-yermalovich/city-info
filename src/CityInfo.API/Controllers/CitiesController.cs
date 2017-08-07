using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : BaseController
    {
        [HttpGet]
        public JsonResult GetCities()
        {
            var result = new JsonResult(CitiesDataStore.Cities) {StatusCode = 200};
            return result;
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var city = CitiesDataStore.GetCity(id);

            if (city == null)
                return NotFound();

            return Ok(city);
        }
    }
}
