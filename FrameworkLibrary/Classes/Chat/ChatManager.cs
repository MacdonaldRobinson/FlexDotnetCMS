using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkLibrary
{
    public class ChatManager
    {
        private static ChatManager _chatManager = null;
        private List<IChatRoom>_chatRooms { get; set; } = new List<IChatRoom>();

        public ReadOnlyCollection<IChatRoom> ChatRooms
        {
            get
            {
                return _chatRooms.AsReadOnly();
            }                
        }

        public static ChatManager GetInstance()
        {
            if (_chatManager == null)
            {
                _chatManager = new ChatManager();
            }

            return _chatManager;
        }

        public void DeleteChatRoom(RoomMode roomMode, string chatRoomName)
        {
            var chatRoom = ChatRooms.FirstOrDefault(i => i.ChatRoomName == chatRoomName && i.ChatRoomMode == roomMode);

            if(chatRoom != null)
            {
                _chatRooms.Remove(chatRoom);
            }
        }

        public IChatRoom GetChatRoomByID(Guid chatRoomId)
        {
            return _chatRooms.Find(i => i.ChatRoomID == chatRoomId);
        }

        public List<IChatRoom> GetSessionChatRooms(string sessionId, RoomMode chatRoomMode)
        {
            return _chatRooms.Where(i => i.ChatRoomMode == chatRoomMode && i.CurrentUsers.Any(j=>j.SessionID == sessionId)).ToList();
        }

        public IChatRoom GetOrCreateChatRoom(RoomMode roomMode, string chatRoomName, ChatUser chatUser)
        {
            IChatRoom chatRoom = null;

            switch (roomMode)
            {
                case RoomMode.Private:
                    {
                        chatRoom = ChatRooms.FirstOrDefault(i => i.ChatRoomName == chatRoomName && i.ChatRoomMode == roomMode && i.CurrentUsers.Any(j => j.SessionID == chatUser.SessionID));

                        if (chatRoom == null)
                        {
                            var newChatRoom = new PrivateChatRoom(chatRoomName, chatUser);
                            _chatRooms.Add(newChatRoom);

                            chatRoom = newChatRoom;
                        }
                    }
                    break;
                case RoomMode.Public:
                    {
                        chatRoom = ChatRooms.FirstOrDefault(i => i.ChatRoomName == chatRoomName && i.ChatRoomMode == roomMode);

                        if(chatRoom == null)
                        {
                            var newChatRoom = new PublicChatRoom(chatRoomName, chatUser);
                            _chatRooms.Add(newChatRoom);

                            chatRoom = newChatRoom;
                        }
                        else
                        {
                            var foundChatUser = chatRoom.CurrentUsers.FirstOrDefault(i => i.SessionID == chatUser.SessionID);
                            if (foundChatUser == null)
                            {
                                chatRoom.JoinChatRoom(chatUser);                                
                            }
                            else
                            {
                                if(!string.IsNullOrEmpty(chatUser.NickName) && chatUser.NickName != foundChatUser.NickName)
                                {
                                    foundChatUser.NickName = chatUser.NickName;
                                }
                            }
                        }
                    }
                    break;
            }

            return chatRoom;
        }

        private ChatManager()
        {
        }
    }
}
