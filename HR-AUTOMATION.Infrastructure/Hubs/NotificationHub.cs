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

            // Prefer the organizationId carried in the authenticated token claim; fall back to the
            // "orgId" query string for clients that connect without a token.
            string? orgClaim = Context.User?.FindFirst(HubConstants.OrganizationIdClaim)?.Value;
            StringValues? orgId = context?.Request.Query[HubConstants.NotificationOrganizationQuery];
            StringValues? allOrgs = context?.Request.Query[HubConstants.NotificationAllOrganizationsQuery];

            if (int.TryParse(orgClaim, out int organizationId) || int.TryParse(orgId, out organizationId))
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