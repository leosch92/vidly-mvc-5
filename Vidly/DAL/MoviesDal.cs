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
        public List<Movie> GetMoviesByIds(IEnumerable<int> ids, bool onlyAvailable)
        {
            using (var context = new ApplicationDbContext())
            {
                var query = GetMoviesByAvailability(context, onlyAvailable);
                return query
                    .Where(m => ids.Contains(m.Id))
                    .ToList();
            }
        }

        public List<Movie> GetMoviesWithGenres(string query, bool onlyAvailable)
        {
            using (var context = new ApplicationDbContext())
            {
                var moviesQuery = GetMoviesByAvailability(context, onlyAvailable);
                moviesQuery = FilterMoviesByQuery(query, moviesQuery);
                return moviesQuery
                    .Include(m => m.Genre)
                    .ToList();
            }
        }

        private IQueryable<Movie> FilterMoviesByQuery(string query, IQueryable<Movie> moviesQuery)
        {
            if (!String.IsNullOrWhiteSpace(query))
                moviesQuery = moviesQuery.Where(m => m.Name.Contains(query));
            return moviesQuery;
        }

        public IQueryable<Movie> GetMoviesByAvailability(ApplicationDbContext context, bool onlyAvailable)
        {
            if (!onlyAvailable)
            {
                return context.Movies;
            }
            var numberAvailablePerMovie = GetNumberAvailablePerMovies(context);
            return numberAvailablePerMovie
                .Where(x => x.NumberAvailable > 0)
                .Select(x => x.Movie);
        }

        private IQueryable<NumberRentedPerMovie> GetNumberRentedPerMovie(ApplicationDbContext context)
        {
            return context.Movies
                .GroupJoin(
                    context.Rentals,
                    m => m.Id,
                    r => r.Movie.Id,
                    (m, r) => new NumberRentedPerMovie { Movie = m, NumberRented = r.DefaultIfEmpty().Count() }
                );
        }

        private IQueryable<NumberAvailablePerMovie> GetNumberAvailablePerMovies(ApplicationDbContext context)
        {
            return GetNumberRentedPerMovie(context)
                .Select(x => new NumberAvailablePerMovie
                {
                    Movie = x.Movie,
                    NumberAvailable = x.Movie.NumberInStock - x.NumberRented
                });
        }
    }
}