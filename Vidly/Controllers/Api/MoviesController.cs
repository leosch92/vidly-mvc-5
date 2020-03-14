using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class MoviesController : ApiController
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<MovieDto> GetMovies(string query = null, bool onlyAvailable = false)
        {
            var moviesQuery = _context.Movies
                .Include(m => m.Genre);

            if (onlyAvailable)
            {
                moviesQuery = GetOnlyAvailableMovies(moviesQuery);
            }

            if (!String.IsNullOrWhiteSpace(query))
                moviesQuery = moviesQuery.Where(m => m.Name.Contains(query));

            return moviesQuery
                .ToList()
                .Select(Mapper.Map<Movie, MovieDto>);
        }

        private IQueryable<Movie> GetOnlyAvailableMovies(IQueryable<Movie> query)
        {
            var numberAvailablePerMovie = GetNumberAvailablePerMovies(query);
            return numberAvailablePerMovie
                .Where(x => x.NumberAvailable > 0)
                .Select(x => x.Movie);
        }

        private IQueryable<NumberRentedPerMovie> GetNumberRentedPerMovie(IQueryable<Movie> query)
        {
            return query
                .GroupJoin(
                    _context.Rentals,
                    m => m.Id,
                    r => r.Movie.Id,
                    (m, r) => new NumberRentedPerMovie { Movie = m, NumberRented = r.DefaultIfEmpty().Count()}
                );
        }

        private IQueryable<NumberAvailablePerMovie> GetNumberAvailablePerMovies(IQueryable<Movie> query)
        {
            return GetNumberRentedPerMovie(query)
                .Select(x => new NumberAvailablePerMovie
                {
                    Movie = x.Movie,
                    NumberAvailable = x.Movie.NumberInStock - x.NumberRented
                });
        }

        public IHttpActionResult GetMovie(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movie == null)
                return NotFound();

            return Ok(Mapper.Map<Movie, MovieDto>(movie));
        }

        [HttpPost]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult CreateMovie(MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movie = Mapper.Map<MovieDto, Movie>(movieDto);
            _context.Movies.Add(movie);
            _context.SaveChanges();

            movieDto.Id = movie.Id;
            return Created(new Uri(Request.RequestUri + "/" + movie.Id), movieDto);
        }

        [HttpPut]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult UpdateMovie(int id, MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movieInDb = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movieInDb == null)
                return NotFound();

            Mapper.Map(movieDto, movieInDb);

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult DeleteMovie(int id)
        {
            var movieInDb = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movieInDb == null)
                return NotFound();

            _context.Movies.Remove(movieInDb);
            _context.SaveChanges();

            return Ok();
        }
    }
}
