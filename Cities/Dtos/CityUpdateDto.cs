﻿using System;

namespace Cities.Dtos
{
    public class CityUpdateDto
    {
        public string Name { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public int ToruistRating { get; set; }

        public DateTime DateEstablished { get; set; }

        public int Population { get; set; }

        public string Alpha2Code { get; set; }

        public string Alpha3Code { get; set; }

        public string CurrenciesCode { get; set; }
    }
}
