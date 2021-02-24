using System;
using System.Threading.Tasks;
using WebUi.Extensions;
using WebUi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace WebUi.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _tracker;
        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {

         //   var isOnline =
                await _tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
            //if (isOnline)
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());

            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            //await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //var isOffline =
            await _tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);

            //if (isOffline)
            var currentUsers = await _tracker.GetOnlineUsers();

                await Clients.Others.SendAsync("UserIsOffline", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }
    }
}