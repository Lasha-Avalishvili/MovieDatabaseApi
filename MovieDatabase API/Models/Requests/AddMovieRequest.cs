using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieDatabase_API.Models.Requests
{
    public class AddMovieRequest 
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Director { get; set; }
        public DateTime ReleaseDate { get; set; }

    }
}
