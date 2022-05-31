using System.Collections.Generic;
using Pillow.ApplicationCore.Entities.BookAggregate;
using Pillow.ApplicationCore.Interfaces;

namespace Pillow.ApplicationCore.Entities.TagAggregate
{
    public class Tag : BaseCodeEntity, IAggregateRoot
    {
        public string Title { get; private set; }

        private readonly List<BookTag> _bookTag = new List<BookTag>();

        public virtual IReadOnlyCollection<BookTag> BookTag => _bookTag.AsReadOnly();

        public Tag(string code,
            string title)
        {
            Code = code;
            Title = title;
        }
    }
}