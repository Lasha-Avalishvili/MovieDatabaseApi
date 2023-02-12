using Microsoft.IdentityModel.Tokens;
using MovieDatabase_API.Models.Requests;

namespace MovieDatabase_API.Validations
{
    public class AddMovieRequestValidator
    {
        public static void Validate(AddMovieRequest request)
        {
            if (request.Name.IsNullOrEmpty() || request.Name.Length > 200) throw new Exception("Name validation");
            if (request.Description.IsNullOrEmpty() || request.Description.Length > 2000) throw new Exception("Description validation");
            if (request.ReleaseDate.Year < 1895) throw new Exception("Year validation");
            if (request.Director.IsNullOrEmpty() || request.Director.Length > 50) throw new Exception("Director validation");

        }

    }
}
