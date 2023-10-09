using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VibeScopyAPI.Dto;
using VibeScopyAPI.Infrastructure;

namespace VibeScopyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatHubController : BaseController
    {
        private readonly VibeScopUnitOfWork _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatHubController(VibeScopUnitOfWork context, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpPost("channelCount")]
        public async Task<IActionResult> ChannelCount(string channelName)
        {
            await _hubContext.Clients.Group(channelName).SendAsync("ReceiveMessage", "", "");
            return Ok();
        }

    }
}