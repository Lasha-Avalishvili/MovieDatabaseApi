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
        // public Statuses Status { get; set; }
        public string Status { get; set; }  
       

        public DateTime CreationDate { get; set; }

        public void ValidateTitle()
        {

            if (string.IsNullOrWhiteSpace(Title))
            {
                throw new Exception("Title cannot be empty or whitespace");
            }
            if (Title.Length > 200)
            {
                throw new Exception("Title is too long");
            }
        }

        public void ValidateDescription()
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                throw new Exception("Description cannot be empty or whitespace");
            }
            if (Description.Length > 2000)
            {
                throw new Exception("Description is too long");
            }
        }

        public void ValidateReleaseDate()
        {
            if (ReleaseDate == default(DateTime))
            {
                throw new Exception("ReleaseDate cannot be empty");
            }
        }

        //public void ValidateStatus()
        //{
        //    if (Status == default(Statuses))
        //    {
        //        throw new Exception("enumField cannot be empty");
        //    }

        //}

        public void ValidateCreationDate()
        {
            if (CreationDate == default(DateTime))
            {
                throw new Exception("CreationDate cannot be empty");
            }

        }


    }

    public enum Statuses
    {
         inactive, active
    }
}
