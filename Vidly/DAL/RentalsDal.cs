using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.DAL
{
    public class RentalsDal
    {
        private MoviesDal _moviesDal;
        private ApplicationDbContext _context;

        public RentalsDal(ApplicationDbContext context)
        {
            _context = context;
            _moviesDal = new MoviesDal(_context);
        }

        public Rental GetById(int id)
        {
            return _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Movie)
                .First(r => r.Id == id);
        }

        public List<Rental> GetWithIncludes(bool includeReturned)
        {
           var query = _context.Rentals
                .Include(r => r.Movie)
                .Include(r => r.Customer);

           if (!includeReturned)
           {
               query = query.Where(r => r.DateReturned == null);
           }

           return query.ToList();
        }

        public bool CreateRental(NewRentalDto dto)
        {
            var customer = _context.Customers.Single(c => c.Id == dto.CustomerId);

            var movies = _moviesDal.GetMoviesByIds(dto.MoviesIds, true).ToList();

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

                _context.Rentals.Add(rental);
            }

            _context.SaveChanges();

            return true;
            
        }

        public void Return(int id)
        {
            var rental = GetById(id);
            rental.DateReturned = DateTime.Now;
            _context.SaveChanges();
        }
    }
}