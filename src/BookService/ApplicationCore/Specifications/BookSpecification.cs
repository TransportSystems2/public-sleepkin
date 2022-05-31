using Ardalis.Specification;
using Pillow.ApplicationCore.Entities.BookAggregate;

namespace Pillow.ApplicationCore.Specifications
{
    public class BookSpecification: Specification<Book>
    {
        public BookSpecification(string bookCode)
        {
            Query.Where(b => b.Code == bookCode);
        }
    }
}