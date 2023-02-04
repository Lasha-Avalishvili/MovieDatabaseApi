using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<Movie>> Get(string anything)
        {
            var movieByTitle = await _context.Movies.FirstOrDefaultAsync(m => m.Title.ToLower() == anything.ToLower());
            if (movieByTitle != null)
            {
                return movieByTitle;
            }

            var movieByDescr = await _context.Movies.FirstOrDefaultAsync(m => m.Description.ToLower().Contains(anything.ToLower()));
            if (movieByDescr != null)
            {
                return movieByDescr;
            }
            var movieByDirector = await _context.Movies.FirstOrDefaultAsync(m => m.Director.ToLower() == anything.ToLower());
            if (movieByDirector != null)
            {
                return movieByDirector;
            }

            int year;
            if (int.TryParse(anything, out year))
            {
                var movieByYear = await _context.Movies.FirstOrDefaultAsync(m => m.ReleaseDate.Year == year);
                if (movieByYear != null)
                {
                    return movieByYear;
                }
            }
           

            return NotFound();
            
        }

    }
}
