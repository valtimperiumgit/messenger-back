namespace Messenger.ViewModels
{
    public class ChatsViewModel
    {
        public Chat chat { get; set; }

        public ClientViewModel chatUser { get; set; }

        public ChatMessege lastMessage { get; set; }
    }
}
