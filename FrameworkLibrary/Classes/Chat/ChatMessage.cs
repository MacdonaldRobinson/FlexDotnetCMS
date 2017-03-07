using System;

namespace FrameworkLibrary
{
    public class ChatMessage
    {
        public ChatUser ChatUser { get; }
        public string Message { get; }
        public DateTime DateCreated { get; } = DateTime.Now;
        public ChatMessage(ref ChatUser chatUser, string message)
        {
            ChatUser = chatUser;
            Message = message;
        }
    }
}
