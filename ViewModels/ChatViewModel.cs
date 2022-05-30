namespace Messenger.ViewModels
{
    public class ChatViewModel
    {
        public Chat chat { get; set; }
        public List<ClientViewModel> members { get; set; }
        
        public ClientViewModel chatUser { get; set; }
        public List<ChatMessege> chatMesseges { get; set; }
    }
}
