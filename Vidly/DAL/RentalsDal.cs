using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.DAL
{
    public class RentalsDal
    {
        public MoviesDal _moviesDal { get; set; }

        public RentalsDal()
        {
            _moviesDal = new MoviesDal();
        }

        public List<Rental> GetWithIncludes()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Rentals
                    .Include(r => r.Movie)
                    .Include(r => r.Customer)
                    .ToList();
            }
        }

        public bool CreateRental(NewRentalDto dto)
        {
            using (var context = new ApplicationDbContext())
            {
                var customer = context.Customers.Single(c => c.Id == dto.CustomerId);

                var movies = _moviesDal.GetMoviesByIds(context, dto.MoviesIds, true).ToList();

                if (movies.Count() != dto.MoviesIds.Count())
                {
                    return false;
                }

                foreach (var movie in movies)
                {
                    var rental = new Rental
                    {
                        Customer = customer,
                        Movie = movie,
                        DateRented = DateTime.Now
                    };

                    context.Rentals.Add(rental);
                }

                context.SaveChanges();

                return true;
            }
            
        } 

    }
}