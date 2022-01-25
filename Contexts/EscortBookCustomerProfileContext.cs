using EscortBookCustomerProfile.Models;
using Microsoft.EntityFrameworkCore;

namespace EscortBookCustomerProfile.Contexts
{
    public class EscortBookCustomerProfileContext : DbContext
    {
        public EscortBookCustomerProfileContext(DbContextOptions<EscortBookCustomerProfileContext> options) : base(options) {}

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Identification> Identifications { get; set; }

        public DbSet<IdentificationPart> IdentificationParts { get; set; }

        public DbSet<Avatar> Avatars { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<ProfileStatus> ProfileStatus { get; set; }

        public DbSet<ProfileStatusCategory> ProfileStatusCategories { get; set; }
    }
}