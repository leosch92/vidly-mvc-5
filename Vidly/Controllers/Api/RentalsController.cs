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
        private ApplicationDbContext _context;

        public RentalsController()
        {
            _context = new ApplicationDbContext();
            _rentalsDal = new RentalsDal(_context);
        }

        [HttpGet]
        public List<Rental> GetRentals(bool includeReturned = true)
        {
            return _rentalsDal.GetWithIncludes(includeReturned);
        }

        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            var success = _rentalsDal.CreateRental(newRental);

            if (!success)
            {
                return BadRequest("One or more movies are unavailable. None of the rentals were registered.");
            }

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
