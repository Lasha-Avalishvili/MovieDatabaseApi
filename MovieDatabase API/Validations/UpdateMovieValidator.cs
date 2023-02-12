using Microsoft.IdentityModel.Tokens;
using MovieDatabase_API.Models.Requests;

namespace MovieDatabase_API.Validations
{
    public class UpdateMovieValidator
    {
        public static void Validate(AddMovieRequest updatedMovie)
        {
            if (updatedMovie.Name.IsNullOrEmpty() || updatedMovie.Name.Length > 200) throw new Exception("Name validation");
            if (updatedMovie.Description.IsNullOrEmpty() || updatedMovie.Description.Length > 2000) throw new Exception("Description validation");
            if (updatedMovie.ReleaseDate.Year < 1895) throw new Exception("Year validation");
            if (updatedMovie.Director.IsNullOrEmpty() || updatedMovie.Director.Length > 50) throw new Exception("Director validation");
        }
    }
}
