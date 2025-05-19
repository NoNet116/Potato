namespace Potato.Helpers
{
    public class ChatAjaxRequest
    {
        public string Id { get; set; }
        public MessageDto NewMessage { get; set; }

        public class MessageDto
        {
            public string Text { get; set; }
        }
    }

}
