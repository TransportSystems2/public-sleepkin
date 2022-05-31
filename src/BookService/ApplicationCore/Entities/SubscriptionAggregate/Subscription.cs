using System;
using System.Linq.Expressions;
using Pillow.ApplicationCore.Entities.UserAccountAggregate;
using Pillow.ApplicationCore.Interfaces;

namespace Pillow.ApplicationCore.Entities.SubscriptionAggregate
{
    public class Subscription : BaseEntity, IAggregateRoot
    {
        public int Id { get; set; }

        public string ReceiptData { get; set; }

        public string UserAccountUserName { get; set; }

        public virtual UserAccount UserAccount { get; set; }

        public DateTime OriginalPurchaseDate { get; set; }
        
        public DateTime PurchaseDate { get; set; }
        
        public DateTime ExpiredDate { get; set; }
        
        public DateTime CancellationDate { get; set; }
        
        public string ProductId { get; set; }

        public Subscription()
        {
        }
        
        public Subscription(string userName, string receiptText, LatestReceiptInfo latestReceiptInfo)
        {
            UserAccountUserName = userName;
            UpdateReceiptInfo(receiptText, latestReceiptInfo);
        }

        public void UpdateReceiptInfo(string receiptText, LatestReceiptInfo latestReceiptInfo)
        {
            ReceiptData = receiptText;
            OriginalPurchaseDate = latestReceiptInfo.OriginalPurchaseDate;
            PurchaseDate = latestReceiptInfo.PurchaseDate;
            ExpiredDate = latestReceiptInfo.ExpiresDate;
            CancellationDate = latestReceiptInfo.CancellationDate;
            ProductId = latestReceiptInfo.ProductId;
        }

        public bool IsActive()
        {
            return ExpiredDate > DateTime.UtcNow
                   && (CancellationDate == DateTime.MinValue);
        }
    }
}