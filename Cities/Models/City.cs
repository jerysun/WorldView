using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cities
{
    public class City
    {
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        public string State { get; set; }

        [Required, MaxLength(256)]
        public string Country { get; set; }

        [Required]
        [Range(1, 5)]
        public int ToruistRating { get; set; }

        [Required]
        public DateTime DateEstablished { get; set; }

        [Required]
        [Range(1, 100000000)]
        public int Population { get; set; }

        [Required, MaxLength(2), MinLength(2)]
        public string Alpha2Code { get; set; }

        [Required, MaxLength(3), MinLength(3)]
        public string Alpha3Code { get; set; }

        [MaxLength(3), MinLength(3)]
        public string CurrenciesCode { get; set; }

        [NotMapped]
        public string Weather { get; set; }
    }
}
