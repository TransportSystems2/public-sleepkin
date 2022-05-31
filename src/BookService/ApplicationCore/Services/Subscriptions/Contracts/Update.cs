namespace Pillow.ApplicationCore.Services.Subscriptions.Contracts
{
    public static class Update
    {
        public sealed class Request
        {
            public string ReceiptData { get; set; }
            public string UserName { get; set; }
            public bool ExcludeOldTransaction { get; set; }
        }

        public sealed class Response
        {
            public int Status { get; set; }
        }
    }
}