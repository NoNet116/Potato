using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Potato.Helpers
{
    //не нужное
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
            => connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
