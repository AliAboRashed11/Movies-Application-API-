﻿using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class Movie
    {

        public int Id { get; set; }
        [MaxLength(length: 250)]
        public string Title { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }
        [MaxLength(length:2500)]
        public string Storelien { get; set; }

        public byte[] Poster { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}
