using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    public class BaseController : Controller
    {
        protected CitiesDataStore CitiesDataStore => CitiesDataStore.Current;
    }
}
