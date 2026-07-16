using HR_AUTOMATION.Infrastructure.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;

namespace HR_AUTOMATION.Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            HttpContext context = Context.GetHttpContext();

            StringValues? orgId = context?.Request.Query[HubConstants.NotificationOrganizationQuery];
            StringValues? allOrgs = context?.Request.Query[HubConstants.NotificationAllOrganizationsQuery];

            if (int.TryParse(orgId, out int organizationId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, organizationId.ToString());
            }

            if (bool.TryParse(allOrgs, out bool allOrganizations) && allOrganizations)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, HubConstants.NotificationAllOrganizationsGroup);
            }

            await base.OnConnectedAsync();
        }
    }
}