using System.Collections.Generic;

namespace FrameworkLibrary
{
    public class PublicChatRoom : ChatRoom
    {
        public PublicChatRoom(string chatRoomName, ChatUser chatUser) : base(chatRoomName, new List<ChatUser>() { chatUser }, RoomMode.Public)
        {
        }

        public override void JoinChatRoom(ChatUser chatUser)
        {
            _currentUsers.Add(chatUser);
        }
    }
}
