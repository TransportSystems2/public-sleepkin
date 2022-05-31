using Pillow.ApplicationCore.Entities.BookAggregate;

namespace Pillow.ApplicationCore.Interfaces
{
    public interface IBookRepository: IAsyncRepository<Book>
    {
    }
}