using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _39eertime_.Models;

namespace _39eertime_.Data
{
    public class _39eertime_Context : DbContext
    {
        public _39eertime_Context (DbContextOptions<_39eertime_Context> options)
            : base(options)
        {
        }

        public DbSet<_39eertime_.Models.Product> Product { get; set; } = default!;
    }
}
