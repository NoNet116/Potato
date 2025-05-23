using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Potato.DbContext.Models.Entity;
using Potato.DbContext.Repository;
using Potato.Helpers;
using Potato.ViewModels;

namespace Potato.Controllers
{
    public class MessageController : Controller
    {
        private IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Chat")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Chat(string id)
        {
            var currentuser = User;

            var result = await _userManager.GetUserAsync(currentuser);
            var friend = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

            var mess = repository.GetMessages(result, friend);
            
            var model = new ChatViewModel()
            {
                You = result,
                ToWhom = friend,
                History = mess.OrderBy(x => x.Id).ToList(),
                
            };
            return View("Chat", model);
        }

        [Route("NewMessage")]
        [HttpPost]
        public async Task<IActionResult> NewMessage([FromBody] ChatAjaxRequest chat)
        {
            var sender = await _userManager.GetUserAsync(User);
            var recipient = await _userManager.FindByIdAsync(chat.Id);

            if (recipient == null)
                return NotFound();

            var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

            var item = new Message
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                Text = chat.NewMessage.Text,
            };

            repository.Create(item);

            await _hubContext.Clients.User(recipient.Id)
            .SendAsync("ReceiveMessage", sender.FullName, item.Text, item.Timestamp.ToString("o")); // ISO


            return Ok();
        }

        [Route("Messages")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Messages()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

            // Получаем все сообщения, где текущий пользователь — либо отправитель, либо получатель
            var allMessages = repository
                .GetAll()
                .Where(m => m.SenderId == currentUser.Id || m.RecipientId == currentUser.Id)
                .ToList();

            // Группируем по пользователю-собеседнику
            var conversations = allMessages
    .Select(m => new
    {
        Companion = m.SenderId == currentUser.Id ? m.Recipient : m.Sender,
        Message = m
    })
    .GroupBy(x => x.Companion.Id)
    .Select(g => new ConversationViewModel
    {
        Companion = g.First().Companion,
        LastMessage = g.OrderByDescending(m => m.Message.Timestamp).First().Message,
        CurrentUserId = currentUser.Id // 👈 передаём сюда
    })
    .OrderByDescending(x => x.LastMessage.Timestamp)
    .ToList();

            return View("Messages", conversations);
        }


    }
}
