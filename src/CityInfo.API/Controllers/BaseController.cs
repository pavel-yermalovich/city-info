using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    public class BaseController : Controller
    {
        public BaseController(ICityInfoRepository repository)
        {
            _repository = repository;
        }

        protected readonly ICityInfoRepository _repository;
    }
}
