using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.DAL
{
    public class MoviesDal
    {
        private ApplicationDbContext _context;

        public MoviesDal(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetMoviesByIds(List<int> ids, bool onlyAvailable)
        {
            var query = GetMoviesByAvailability(onlyAvailable);
            return query
                .Where(m => ids.Contains(m.Id))
                .ToList();
        }

        public List<Movie> GetMoviesWithGenres(string query, bool onlyAvailable)
        {
            var moviesQuery = GetMoviesByAvailability(onlyAvailable);
            moviesQuery = FilterMoviesByQuery(query, moviesQuery);
            return moviesQuery
                .Include(m => m.Genre)
                .ToList();
        }

        private IQueryable<Movie> FilterMoviesByQuery(string query, IQueryable<Movie> moviesQuery)
        {
            if (!String.IsNullOrWhiteSpace(query))
                moviesQuery = moviesQuery.Where(m => m.Name.Contains(query));
            return moviesQuery;
        }

        public IQueryable<Movie> GetMoviesByAvailability(bool onlyAvailable)
        {
            if (!onlyAvailable)
            {
                return _context.Movies;
            }

            var numberAvailablePerMovie = GetNumberAvailablePerMovies();
            return numberAvailablePerMovie
                .Where(x => x.NumberAvailable > 0)
                .Select(x => x.Movie);
        }

        private IQueryable<NumberRentedPerMovie> GetNumberRentedPerMovie()
        {
            return _context.Movies
                .GroupJoin(
                    _context.Rentals,
                    m => m.Id,
                    r => r.Movie.Id,
                    (m, rentals) => new NumberRentedPerMovie
                    {
                        Movie = m,
                        NumberRented = rentals
                            .DefaultIfEmpty()
                            .Count(r => r.Movie.Id == m.Id && r.DateReturned == null)
                    }
                );
        }

        private IQueryable<NumberAvailablePerMovie> GetNumberAvailablePerMovies()
        {
            return GetNumberRentedPerMovie()
                .Select(x => new NumberAvailablePerMovie
                {
                    Movie = x.Movie,
                    NumberAvailable = x.Movie.NumberInStock - x.NumberRented
                });
        }
    }
}