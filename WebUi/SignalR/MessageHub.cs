using System;
using System.Linq;
using System.Threading.Tasks;
using WebUi.Dto;
using WebUi.Entities;
using WebUi.Extensions;
using WebUi.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace WebUi.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _tracker;
        // private readonly IUnitOfWork _unitOfWork;
        public MessageHub(IMapper mapper,
            IMessageRepository messageRepository,
            IUserRepository userRepository,
            IHubContext<PresenceHub> presenceHub,
            PresenceTracker tracker)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _tracker = tracker;
            _presenceHub = presenceHub;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
           //var group =
           await AddToGroup(groupName);
            //await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await _messageRepository.
              //_unitOfWork.MessageRepository.
                GetMessageThread(Context.User.GetUsername(), otherUser);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
          //  if (_unitOfWork.HasChanges()) await _unitOfWork.Complete();

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {//when disconnect SignalR Auto remove them from the group
           // var group =
           await RemoveFromMessageGroup();
           // await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("You cannot send messages to yourself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) throw new HubException("Not found user");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);

            var group = await _messageRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            //else
            //{
            //    var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
            //    if (connections != null)
            //    {
            //        await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
            //            new { username = sender.UserName, knownAs = sender.KnownAs });
            //    }
            //}

            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync())
            {
              //  var groupName = GetGroupName(sender.UserName, recipient.UserName);
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
            //if (await _unitOfWork.Complete())
            //{
                
            //}
        }

        private async Task<bool> AddToGroup(string groupName)
        {
            var group = await _messageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if (group == null)
            {
                group = new Group(groupName);
                _messageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            return await _messageRepository.SaveAllAsync();

            //throw new HubException("Failed to join group");
        }

        private async Task RemoveFromMessageGroup()// to be removed
        {
          //  var group = await _messageRepository.GetGroupForConnection(connectionId);
          var connection = await _messageRepository.GetConnection(Context.ConnectionId);// group.Connections.FirstOrDefault(x => x.ConnectionId == connectionId);
            _messageRepository.RemoveConnection(connection);
            await _messageRepository.SaveAllAsync();

            //throw new HubException("Failed to remove from group");
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}