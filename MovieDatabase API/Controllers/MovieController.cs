using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public MovieController(IMovieRepository repo)
        {
            _movieRepository = repo;
        }

        [HttpPost("/movie/add")]
        public async Task<IActionResult> Add([FromBody] AddMovieRequest request)
        {

            await _movieRepository.AddMovieAsync(request);
            await _movieRepository.SaveChangesAsync();
            return Ok();
        }
    }
}
