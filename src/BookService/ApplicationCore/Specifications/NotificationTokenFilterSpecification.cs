using Ardalis.Specification;
using Pillow.ApplicationCore.Entities.NotificationTokenAggregate;

namespace Pillow.ApplicationCore.Specifications
{
    public class NotificationTokenFilterSpecification : Specification<NotificationToken>
    {
        public NotificationTokenFilterSpecification(string token)
        {
            Query.Where(t => t.Token == token);
        }
    }
}