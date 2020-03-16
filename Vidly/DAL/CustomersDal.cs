using System.Collections.Generic;
using System.Linq;
using Vidly.Models;

namespace Vidly.DAL
{
    public class CustomersDal
    {
        private ApplicationDbContext _context;

        public CustomersDal(ApplicationDbContext context)
        {
            _context = context;
        }

        public Customer GetCustomer(int id)
        {
            return _context.Customers.Single(c => c.Id == id);
        }

    }
}