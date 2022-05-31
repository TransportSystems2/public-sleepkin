using Microsoft.AspNetCore.Http;

namespace Pillow.PublicApi.Utils
{
    public static class NetworkUtils
    {
        public static string RemoteIpAddress(this HttpContext context)
        {
            string ipAddress = context.Request.Headers["X-Real-IP"];
            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                return ipAddress;
            }

            ipAddress = context.Request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return ipAddress;
        }
    }
}