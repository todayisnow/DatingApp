using System.Collections.Generic;
using System.Threading.Tasks;
using WebUi.Dto;
using WebUi.Entities;
using WebUi.Extensions;
using WebUi.Helpers;
using WebUi.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUi.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public MessagesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {

            var username = User.GetUsername();
            if (username == createMessageDto.RecipientUsername.ToLower())
            {
                return BadRequest("You cant send to yourself");
            }

            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);
            if (recipient == null) return NotFound();
            var message = new Message
            {
                SenderUsername = sender.UserName,
                Sender = sender,
                RecipientUsername = recipient.UserName,
                Recipient = recipient,
                Content = createMessageDto.Content
            };
            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.Complete()) return Ok(_mapper.Map<MessageDto>(message));
            return BadRequest("failed to send msg");

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery]
            MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();

            var messages = await _unitOfWork.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize,
                messages.TotalCount, messages.TotalPages);

            return messages;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();

            var message = await _unitOfWork.MessageRepository.GetMessage(id);

            if (message.Sender.UserName != username && message.Recipient.UserName != username)
                return Unauthorized();

            if (message.Sender.UserName == username) message.SenderDeleted = true;

            if (message.Recipient.UserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
                _unitOfWork.MessageRepository.DeleteMessage(message);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem deleting the message");
        }


        //[HttpGet("thread/{username}")]
        //public async  Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        //{
        //    var currentUername = User.GetUsername();
        //    return Ok(await _unitOfWork.MessageRepository.GetMessageThread(currentUername, username));
        //}

    }
}