using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AprendiendoWebApi.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Provincia> Provincias { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }
    }
}
