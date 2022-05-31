using System;

namespace Pillow.ApplicationCore.Services.Subscriptions.Contracts
{
    public static class MultipleUpdate
    {
        public sealed class Request
        {
            public bool IsOnlyActive { get; set; }
            
            public bool ExcludeOldTransaction { get; set; }

            public TimeSpan GracePeriod { get; set; } = TimeSpan.FromDays(7);
        }

        public sealed class Response
        {
            public int NumberOfTotal { get; set; }

            public int NumberOfSuccess { get; set; }

            public int NumberOfErrors { get; set; }
        }
    }
}