using System;

namespace FrameworkLibrary
{
    public enum ChatMessageMode
    {
        System = 0,
        User = 1
    }

    public class ChatMessage
    {
        public ChatUser ChatUser { get; }
        public string Message { get; }
        public DateTime DateCreated { get; } = DateTime.Now;
        public ChatMessageMode MessageMode { get; } = ChatMessageMode.User;

        public ChatMessage(ref ChatUser chatUser, string message, ChatMessageMode messageMode = ChatMessageMode.User)
        {
            ChatUser = chatUser;
            Message = message;
            MessageMode = messageMode;
        }
    }
}
