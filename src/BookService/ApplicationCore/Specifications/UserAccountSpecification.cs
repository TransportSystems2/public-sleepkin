using Ardalis.Specification;
using Pillow.ApplicationCore.Entities.UserAccountAggregate;

namespace Pillow.ApplicationCore.Specifications
{
    public class UserAccountSpecification : Specification<UserAccount>
    {
        public UserAccountSpecification(string userName)
        {
            Query.Where(ua => ua.UserName == userName);
        }
    }
}