using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase_API.Db;
using MovieDatabase_API.Models;
using MovieDatabase_API.Models.Requests;

namespace MovieDatabase_API.Repositories
{  
    public interface IMovieRepository
    {
        Task<Movie> AddMovieAsync(AddMovieRequest request);
        Task SaveChangesAsync();
    }
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _db;
        public MovieRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Movie> AddMovieAsync(AddMovieRequest request)
        {
            var movie = new Movie()
            {
                Title = request.Name,
                ReleaseDate = request.ReleaseDate,
                Description = request.Description,
                Director = request.Director                 
            };

            movie.Status = "Active"; // Statuses.active;        // status is always active here
            movie.CreationDate = DateTime.Today;

            await _db.Movies.AddAsync(movie);   // do I need await here?

            return movie;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }


    }
}
