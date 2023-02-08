using System.Reflection.Metadata.Ecma335;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieDatabase_API.Db;
using MovieDatabase_API.Models;
using MovieDatabase_API.Models.Requests;
using MovieDatabase_API.Repositories;

namespace MovieDatabase_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly AppDbContext _context;
        public MovieController(IMovieRepository repo, AppDbContext context)
        {
            _movieRepository = repo;
            _context = context;
        }

        [HttpPost("/movie/add")]
        public async Task<IActionResult> Add([FromBody] AddMovieRequest request)
        {
            if (request.Name.IsNullOrEmpty() || request.Name.Length > 200) throw new Exception("Name validation");
            if (request.Description.IsNullOrEmpty() || request.Description.Length > 2000) throw new Exception("Description validation");
            if (request.ReleaseDate.Year < 1895) throw new Exception("Year validation");
            if (request.Director.IsNullOrEmpty() || request.Director.Length > 50) throw new Exception("Director validation");
            
            await _movieRepository.AddMovieAsync(request);
            await _movieRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("/movie/get")]
        public async Task<ActionResult<Movie>> Get(int movieId)
        {
            var movie = await _context.Movies.Where(m => m.Id == movieId).FirstOrDefaultAsync(); 
            if(movie == null)
            {
                return NotFound();
            }
            return movie;
        }

        [HttpGet("/movie/search")]
        public async Task<ActionResult<List<Movie>>> Get(string anything)
        {
            var foundMovies = new List<Movie>();


            var movieByTitle = await _context.Movies.Where(m => m.Title.ToLower()== anything.ToLower()).ToListAsync();
            foundMovies.AddRange(movieByTitle);

            var movieByDescr = await _context.Movies.Where(m => m.Description.ToLower().Contains(anything.ToLower())).ToListAsync();
            foundMovies.AddRange(movieByDescr);

            var movieByDirector = await _context.Movies.Where(m => m.Director.ToLower() == anything.ToLower()).ToListAsync();
            foundMovies.AddRange(movieByDirector);

            int year;
            if (int.TryParse(anything, out year))
            {
                var movieByYear = await _context.Movies.Where(m => m.ReleaseDate.Year == year).ToListAsync();
                 foundMovies.AddRange(movieByYear);
            }           

             if(foundMovies.Count >0)
            {
                return foundMovies;
            } else
            {
                return BadRequest("nothing found");
            }
        }


        [HttpPut("/update")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] AddMovieRequest updatedMovie)
        {
            if (updatedMovie.Name.IsNullOrEmpty() || updatedMovie.Name.Length > 200) throw new Exception("Name validation");
            if (updatedMovie.Description.IsNullOrEmpty() || updatedMovie.Description.Length > 2000) throw new Exception("Description validation");
            if (updatedMovie.ReleaseDate.Year < 1895) throw new Exception("Year validation");
            if (updatedMovie.Director.IsNullOrEmpty() || updatedMovie.Director.Length > 50) throw new Exception("Director validation");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = updatedMovie.Name;
            movie.Description = updatedMovie.Description;
            movie.ReleaseDate = updatedMovie.ReleaseDate;
            movie.Director = updatedMovie.Director;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("/delete")]
        public async Task<IActionResult> SoftDeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            movie.Status = "Deleted";   // Statuses.inactive;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
