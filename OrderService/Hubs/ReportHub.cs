using Microsoft.AspNetCore.SignalR;

namespace OrderService.Hubs
{
    public class ReportHub : Hub
    {
        public async Task JoinGroup(int userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        }
    }
}