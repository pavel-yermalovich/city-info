using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();

        public int PointOfInterestCount => PointsOfInterest.Count;

        public PointOfInterestDto GetPointOfInterestById(int id) => PointsOfInterest.FirstOrDefault(p => p.Id == id);
    }
}
