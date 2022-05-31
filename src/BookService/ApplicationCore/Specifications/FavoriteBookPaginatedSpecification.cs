using System.Linq;
using Ardalis.Specification;
using Pillow.ApplicationCore.Entities.BookAggregate;

namespace Pillow.ApplicationCore.Specifications
{
    public class FavoriteBookPaginatedSpecification : Specification<Book>
    {
        public FavoriteBookPaginatedSpecification(string[] bookCodes,
            string[] tags)
        {
            Query.Where(b =>
                bookCodes.Contains(b.Code)
                && (tags == null || b.BookTag.Any(bt => tags.Contains(bt.TagCode))))
                .OrderByDescending(b => b.BookTag.Count(bt => bt.TagCode == "new"))
                .ThenByDescending(b => b.Likes);
        }

        public FavoriteBookPaginatedSpecification(
            int skip,
            int take,
            string[] bookCodes,
            string[] tags)
            : this(bookCodes, tags)
        {
            Query
                .Skip(skip)
                .Take(take);
        }
    }
}