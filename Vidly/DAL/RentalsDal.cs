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

        public void CreateRental(Customer customer, List<Movie> movies)
        {
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
            
        }

        public void Return(int id)
        {
            var rental = GetById(id);
            rental.DateReturned = DateTime.Now;
            _context.SaveChanges();
        }

        public int CountPerCustomer(int customerId)
        {
            return _context.Rentals.Count(r => r.Customer.Id == customerId);
        }
    }
}