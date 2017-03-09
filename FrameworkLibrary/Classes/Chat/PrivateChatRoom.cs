using System.Collections.Generic;

namespace FrameworkLibrary
{
    public class PrivateChatRoom : ChatRoom
    {
        public PrivateChatRoom(string chatRoomName) : base(chatRoomName, RoomMode.Private)
        {
        }

        public override void JoinChatRoom(ChatUser chatUser)
        {
            if (chatUser.GetLoggedInUser() != null)
            {
                base.JoinChatRoom(chatUser);
            }            
        }
    }
}
