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
                var chatRooms = _chatRooms.ToList();

                foreach (var room in chatRooms)
                {
                    if(room.CurrentUsers.Count == 0)
                    {
                        DeleteChatRoom(room);
                    }
                }

                return _chatRooms.AsReadOnly();
            }                
        }

        public IEnumerable<ChatUser> ChatUsers
        {
            get
            {
                return ChatRooms.SelectMany(i=>i.CurrentUsers.Where(j=>j.IsActive));
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

        public IChatRoom AddChatRoom(IChatRoom chatRoom)
        {
            var foundChatRoom = ChatRooms.FirstOrDefault(i=>(i.ChatRoomName == chatRoom.ChatRoomName && i.ChatRoomMode == chatRoom.ChatRoomMode) || (i.ChatRoomID == chatRoom.ChatRoomID));

            if(foundChatRoom == null)
            {
                _chatRooms.Add(chatRoom);
                return chatRoom;
            }
            else
            {
                return null;
            }
        }

        public List<IChatRoom> GetChatRooms(RoomMode roomMode)
        {
            var chatRooms = ChatRooms.Where(i=>i.ChatRoomMode == roomMode).ToList();

            foreach (var room in chatRooms)
            {
                var activeUsers = room.CurrentUsers.Count(i=>i.IsActive);

                if(activeUsers == 0)
                {
                    DeleteChatRoom(room.ChatRoomID);                    
                }
            }

            chatRooms = ChatRooms.Where(i=>i.ChatRoomMode == roomMode).ToList();

            return chatRooms;
        }

        public void DeleteChatRoom(RoomMode roomMode, string chatRoomName)
        {
            var chatRoom = ChatRooms.FirstOrDefault(i => i.ChatRoomName == chatRoomName && i.ChatRoomMode == roomMode);

            if(chatRoom != null)
            {
                _chatRooms.Remove(chatRoom);
            }
        }

        public void DeleteChatRoom(Guid chatRoomId)
        {
            var chatRoom = ChatRooms.FirstOrDefault(i => i.ChatRoomID == chatRoomId);

            if(chatRoom != null)
            {
                _chatRooms.Remove(chatRoom);
            }
        }

        public void DeleteChatRoom(IChatRoom chatRoom)
        {
            _chatRooms.Remove(chatRoom);
        }

        public IChatRoom GetChatRoomByID(Guid chatRoomId)
        {
            return ChatRooms.FirstOrDefault(i => i.ChatRoomID == chatRoomId);
        }

        public List<IChatRoom> GetSessionChatRooms(string sessionId, RoomMode chatRoomMode)
        {
            return ChatRooms.Where(i => i.ChatRoomMode == chatRoomMode && i.CurrentUsers.Any(j=>j.SessionID == sessionId)).ToList();
        }

        public IChatRoom GetChatRoom(RoomMode roomMode, string chatRoomName)
        {
            return ChatRooms.FirstOrDefault(i=>i.ChatRoomMode == roomMode && i.ChatRoomName == chatRoomName);
        }

        public IChatRoom GetChatRoomWithUsers(RoomMode roomMode, List<ChatUser> chatUsers)
        {
            return ChatRooms.FirstOrDefault(i => i.ChatRoomMode == roomMode && chatUsers.TrueForAll(j=>i.CurrentUsers.Contains(j)));
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
                            var newChatRoom = new PrivateChatRoom(chatRoomName);
                            newChatRoom.JoinChatRoom(chatUser);                            

                            chatRoom = AddChatRoom(newChatRoom);
                        }
                    }
                break;
                case RoomMode.Public:
                    {
                        chatRoom = ChatRooms.FirstOrDefault(i => i.ChatRoomName == chatRoomName && i.ChatRoomMode == roomMode);

                        if(chatRoom == null)
                        {
                            var newChatRoom = new PublicChatRoom(chatRoomName);
                            newChatRoom.JoinChatRoom(chatUser);    

                            chatRoom = AddChatRoom(newChatRoom);
                        }
                    }
                break;
            }

            if(chatRoom != null)
            {
                var foundChatUser = chatRoom.CurrentUsers.FirstOrDefault(i => i.SessionID == chatUser.SessionID);

                if (foundChatUser == null)
                {
                    chatRoom.JoinChatRoom(chatUser);                                
                }
                else
                {
                    if (foundChatUser.IsActive)
                    {
                        if (!string.IsNullOrEmpty(chatUser.NickName) && chatUser.NickName != foundChatUser.NickName)
                        {
                            foundChatUser.NickName = chatUser.NickName;
                        }
                    }
                    else
                    {
                        chatRoom.RemoveUser(foundChatUser);

                        foreach (var room in ChatRooms)
                        {
                            room.RemoveUser(foundChatUser);
                        } 

                        return null;
                    }
                }
            }

            return chatRoom;
        }

        private ChatManager()
        {
        }
    }
}
