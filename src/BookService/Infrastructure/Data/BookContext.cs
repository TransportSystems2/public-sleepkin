using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Pillow.ApplicationCore.Entities.BookAggregate;
using Pillow.ApplicationCore.Entities.NotificationTokenAggregate;
using Pillow.ApplicationCore.Entities.TagAggregate;
using Pillow.ApplicationCore.Entities.UserAccountAggregate;

namespace Pillow.Infrastructure.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        
        public DbSet<Track> Tracks { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<BookTag> BookTag { get; set; }

        public DbSet<NotificationToken> NotificationTokens { get; set; }

        public DbSet<FavoriteBook> FavoriteBooks { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}