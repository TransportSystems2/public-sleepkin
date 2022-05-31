using Pillow.ApplicationCore.Entities.BookAggregate;
using Pillow.ApplicationCore.Interfaces;

namespace Pillow.Infrastructure.Data
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(BookContext dbContext) : base(dbContext)
        {
        }
    }
}