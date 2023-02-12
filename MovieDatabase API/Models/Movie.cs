using System;

namespace MovieDatabase_API.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Director { get; set; }
        public string? Status { get; set; }  
        public DateTime CreationDate { get; set; }
    }
    
}
