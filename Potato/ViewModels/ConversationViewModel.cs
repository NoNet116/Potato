using Potato.DbContext.Models.Entity;

namespace Potato.ViewModels
{
    public class ConversationViewModel
    {
        public User Companion { get; set; }
        public Message LastMessage { get; set; }
        public string CurrentUserId { get; set; }
    }
}
