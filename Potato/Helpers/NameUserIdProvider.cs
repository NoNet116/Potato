using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Potato.Helpers
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
            => connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
