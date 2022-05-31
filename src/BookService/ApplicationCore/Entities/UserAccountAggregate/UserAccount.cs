using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Pillow.ApplicationCore.Entities.BookAggregate;
using Pillow.ApplicationCore.Exceptions;
using Pillow.ApplicationCore.Interfaces;

namespace Pillow.ApplicationCore.Entities.UserAccountAggregate
{
    public class UserAccount : BaseEntity, IAggregateRoot
    {
        public string UserName { get; set; }

        private readonly List<FavoriteBook> _favorites = new List<FavoriteBook>();

        public virtual IReadOnlyCollection<FavoriteBook> Favorites => _favorites.AsReadOnly();

        private readonly List<PreorderBook> _preorders = new List<PreorderBook>();

        public virtual IReadOnlyCollection<PreorderBook> Preorders => _preorders.AsReadOnly();

        public UserAccount(string userName)
        {
            UserName = userName;
        }

        public void AddFavoriteBook(Book book)
        {
            Guard.Against.Null(book, nameof(book));

            if (Favorites.Any(favorite => favorite.Book == book))
            {
                throw new DublicateEntityException($"UserName: {UserName} already has in favorites this book", book.Code);
            }

            _favorites.Add(new FavoriteBook(book, this));
            book.Like();
        }

        public void RemoveFavoriteBook(Book book)
        {
            Guard.Against.Null(book, nameof(book));

            FavoriteBook favorite = Favorites.FirstOrDefault(favorite => favorite.Book == book);
            Guard.Against.Null(favorite, nameof(favorite));

            _favorites.Remove(favorite);
            book.Unlike();
        }

        public void AddPreorderBook(Book book)
        {
            Guard.Against.Null(book, nameof(book));

            if (Preorders.Any(preorder => preorder.Book == book))
            {
                throw new DublicateEntityException($"UserName: {UserName} already has in preorders this book", book.Code);
            }

            _preorders.Add(new PreorderBook(book, this));
            book.AddPreorder();
        }

        public void RemovePreorderBook(Book book)
        {
            Guard.Against.Null(book, nameof(book));

            PreorderBook preorder = Preorders.FirstOrDefault(preorder => preorder.Book == book);
            Guard.Against.Null(preorder, nameof(preorder));

            _preorders.Remove(preorder);
            book.RemovePreorder();
        }
    }
}