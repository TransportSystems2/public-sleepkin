using Pillow.ApplicationCore.Entities.BookAggregate;

namespace Pillow.ApplicationCore.Entities.UserAccountAggregate
{
    public class FavoriteBook : BaseEntity
    {
        public int Id { get; set; }

        public string BookCode { get; set; }

        public virtual Book Book { get; set; }

        public string UserAccountUserName { get; set; }

        public virtual UserAccount UserAccount { get; set; }

        public FavoriteBook()
        {
        }
     
        public FavoriteBook(Book book, UserAccount userAccount)
        {
            Book = book;
            UserAccount = userAccount;
        }
    }
}