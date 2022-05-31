using System.Threading.Tasks;

namespace PromoCode.API.Infrastructure
{
    public class PromoCodeContextSeed
    {
        public static Task SeedAsync(PromoCodeContext context)
        {
            return Task.CompletedTask;
        }
    }
}