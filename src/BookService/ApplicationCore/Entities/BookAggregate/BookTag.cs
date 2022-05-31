using Pillow.ApplicationCore.Entities.TagAggregate;

namespace Pillow.ApplicationCore.Entities.BookAggregate
{
    public class BookTag
    {
        public string BookCode { get; set; }
        public virtual Book Book { get; set; }

        public string TagCode { get; set; }
        public virtual Tag Tag { get; set; }
        
        public BookTag(string bookCode, string tagCode)
        {
            BookCode = bookCode;
            TagCode = tagCode;
        }
    }
}