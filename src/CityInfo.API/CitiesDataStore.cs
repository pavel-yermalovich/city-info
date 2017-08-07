﻿using System.Collections.Generic;
using System.Linq;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current => new CitiesDataStore();

        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>
            {
                new CityDto
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with the big park",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "The most visited urban park in the United States"
                        },
                        new PointOfInterestDto
                        {
                            Id = 2,
                            Name = "Empire State Building",
                            Description = "A 102-story scyscraper located in Manhattan Midtown"
                        },
                    }
                },
                new CityDto
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished"
                },
                new CityDto
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with the big tower"
                }
            };
        }

        public CityDto GetCity(int id) => Cities.FirstOrDefault(c => c.Id == id);
    }
}
