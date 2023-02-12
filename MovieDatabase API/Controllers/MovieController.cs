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
using MovieDatabase_API.Validations;

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
            try
            {
                AddMovieRequestValidator.Validate(request);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            
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
        public async Task<ActionResult<List<Movie>>> Get(string searchInput)
        {
            var foundMovies = new List<Movie>();


            var movieByTitle = await _context.Movies.Where(m => m.Title.ToLower()== searchInput.ToLower()).ToListAsync();
            foundMovies.AddRange(movieByTitle);

            var movieByDescr = await _context.Movies.Where(m => m.Description.ToLower().Contains(searchInput.ToLower())).ToListAsync();
            foundMovies.AddRange(movieByDescr);

            var movieByDirector = await _context.Movies.Where(m => m.Director.ToLower() == searchInput.ToLower()).ToListAsync();
            foundMovies.AddRange(movieByDirector);

            if (int.TryParse(searchInput, out int year))
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
            try
            {
                UpdateMovieValidator.Validate(updatedMovie);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

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
