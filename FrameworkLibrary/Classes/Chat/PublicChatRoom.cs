using System.Collections.Generic;

namespace FrameworkLibrary
{
    public class PublicChatRoom : ChatRoom
    {
        public PublicChatRoom(string chatRoomName) : base(chatRoomName, RoomMode.Public)
        {
        }

        public override void JoinChatRoom(ChatUser chatUser)
        {
            base.JoinChatRoom(chatUser);
        }

    }
}
