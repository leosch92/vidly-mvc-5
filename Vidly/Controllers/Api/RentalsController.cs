using System;
using System.Linq;
using System.Web.Http;
using Vidly.DAL;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class RentalsController : ApiController
    {
        private ApplicationDbContext _context;
        private MoviesDal _moviesDal;
        private RentalsDal _rentalsDal;

        public RentalsController()
        {
            _context = new ApplicationDbContext();
            _moviesDal = new MoviesDal();
            _rentalsDal = new RentalsDal();
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
    }
}
