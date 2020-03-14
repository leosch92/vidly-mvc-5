using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class NumberRentedPerMovie
    {
        public Movie Movie { get; set; }
        public int NumberRented { get; set; }
    }
}