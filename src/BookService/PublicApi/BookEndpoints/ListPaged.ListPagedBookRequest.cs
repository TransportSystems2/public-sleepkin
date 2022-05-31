namespace Pillow.PublicApi.BookEndpoints
{
    public class ListPagedBookRequest : BaseRequest 
    {
        public int Limit { get; set; } = 20;
        
        public int Offset { get; set; } = 0;

        public string[] Tags { get; set; }

        public bool IncludeNotPublished { get; set; } = false;
    }
}
