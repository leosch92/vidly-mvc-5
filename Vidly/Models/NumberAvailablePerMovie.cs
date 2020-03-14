using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class NumberAvailablePerMovie
    {
        public Movie Movie { get; set; }
        public int NumberAvailable { get; set; }
    }
}