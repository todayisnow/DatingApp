using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace WebUi.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers =
            new Dictionary<string, List<string>>();

        public Task UserConnected(string username, string connectionId)
        {
            bool isOnline = false;
            lock (OnlineUsers)// becaue dic is not thread safe so we lock it to prevent concurrent adding
            {
                if (OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers[username].Add(connectionId);
                }
                else
                {
                    OnlineUsers.Add(username, new List<string> { connectionId });
                    isOnline = true;
                }
            }

            return Task.CompletedTask;//.FromResult(isOnline);
        }

        public Task UserDisconnected(string username, string connectionId)
        {
            bool isOffline = false;
            lock (OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(username)) return Task.CompletedTask;//.FromResult(isOffline);

                OnlineUsers[username].Remove(connectionId);
                if (OnlineUsers[username].Count == 0)
                {
                    OnlineUsers.Remove(username);
                    isOffline = true;
                }
            }

            return Task.CompletedTask;// .FromResult(isOffline);
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;
            lock (OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

            return Task.FromResult(onlineUsers);
        }

        public Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds;
            lock (OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(username);
            }

            return Task.FromResult(connectionIds);
        }
    }
}