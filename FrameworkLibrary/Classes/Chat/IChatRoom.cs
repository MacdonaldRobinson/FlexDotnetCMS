using System;
using System.Collections.ObjectModel;

namespace FrameworkLibrary
{
    public interface IChatRoom
    {
        Guid ChatRoomID { get; }
        string ChatRoomName { get; }
        ReadOnlyCollection<ChatUser> CurrentUsers { get; }
        ReadOnlyCollection<ChatMessage> ChatMessages { get; }
        RoomMode ChatRoomMode { get; }
        void DeleteChatMessages();
        void AddMessage(ChatUser chatUser, string message);
        void JoinChatRoom(ChatUser chatUser);
        void RemoveUser(ChatUser chatUser);
        ChatUser GetUserInChatRoom(ChatUser chatUser);
    }
}
