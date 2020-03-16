using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using Vidly.DAL;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class RentalsController : ApiController
    {
        private RentalsDal _rentalsDal;
        private CustomersDal _customersDal;
        private MoviesDal _moviesDal;
        private ApplicationDbContext _context;

        public RentalsController()
        {
            _context = new ApplicationDbContext();
            _rentalsDal = new RentalsDal(_context);
            _customersDal = new CustomersDal(_context);
            _moviesDal = new MoviesDal(_context);
        }

        [HttpGet]
        public List<Rental> GetRentals(bool includeReturned = true)
        {
            return _rentalsDal.GetWithIncludes(includeReturned);
        }

        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            var customer = _customersDal.GetCustomer(newRental.CustomerId);
            var movies = _moviesDal.GetMoviesByIds(newRental.MoviesIds, true).ToList();

            _rentalsDal.CreateRental(customer, movies);
            
            return Ok();
        }

        [HttpPut]
        [Route("api/rentals/return/{id}")]
        public IHttpActionResult Return(int id)
        {
            _rentalsDal.Return(id);
            return Ok();
        }
    }
}
