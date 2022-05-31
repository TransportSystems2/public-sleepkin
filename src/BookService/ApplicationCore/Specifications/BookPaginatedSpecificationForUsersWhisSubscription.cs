using System.Linq;

namespace Pillow.ApplicationCore.Specifications
{
    public class BookPaginatedSpecificationForUsersWithSubscription : BaseBookPaginatedSpecification
    {
        public BookPaginatedSpecificationForUsersWithSubscription(string[] tags, bool includeNotPublished)
        {
            Query.Where(b => (tags == null || b.BookTag.Any(bt => tags.Contains(bt.TagCode)))
                && (b.IsPublished || includeNotPublished))
                .OrderByDescending(b => b.BookTag.Count(bt => bt.TagCode == "new"))
                .ThenBy(b => b.BookTag.Count(bt => bt.TagCode == "soon"))
                .ThenByDescending(b => b.Likes)
                .ThenByDescending(b => b.Preorders);
        }

        public BookPaginatedSpecificationForUsersWithSubscription(
            int skip,
            int take,
            string[] tags,
            bool includeNotPublished)
            : this(tags, includeNotPublished)
        {
            Query
                .Skip(skip)
                .Take(take);
        }
    }
}