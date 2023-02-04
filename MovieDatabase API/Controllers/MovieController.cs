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


    }
}
