using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentHouseSystem.Models;

namespace RentHouseSystem.Data
{
    public class RentHouseSystemContext : IdentityDbContext
    {
        public RentHouseSystemContext (DbContextOptions<RentHouseSystemContext> options)
            : base(options)
        {
        }

        public DbSet<RentHouseSystem.Models.House> House { get; set; } = default!;
        public DbSet<RentHouseSystem.Models.User> User { get; set; } = default!;
        

        internal Task<object> FindAsync(string id)
        {
            throw new NotImplementedException();
        }


        public DbSet<RentHouseSystem.Models.Comment> Comment { get; set; } = default!;
        public DbSet<RentHouseSystem.Models.Image> Image { get; set; } = default!;


        public DbSet<RentHouseSystem.Models.Facilities> Facilities { get; set; } = default!;


       

        
    }
}
