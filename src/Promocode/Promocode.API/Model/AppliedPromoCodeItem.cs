namespace PromoCode.API.Model
{
    public class AppliedPromoCodeItem
    {
        public string PromoCode { get; set; }
        
        public virtual PromoCodeItem PromoCodeItem { get; set; }
        
        public string UserName { get; set; }
    }
}